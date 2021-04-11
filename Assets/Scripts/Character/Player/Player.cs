using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Character
{
	[SerializeField] private float m_JumpForce = 500;
	public Weapon m_Weapon = null;
	public float m_Dash = 0;
	public ParticleSystem m_DashParticle = null;
	private Vector2 m_DashVel = Vector2.zero;
	public float m_DashSpeed = 10;
	public float m_BrickGauge = 100;
	public Image m_HpBar = null;
	public Image m_BrickBar = null;
	public Image m_DurabilityBar = null;
	public TextMeshProUGUI m_Score = null;
	public TextMeshProUGUI m_Brick = null;
	public GameObject m_GameOverCanvas = null;
	public float m_Ult = 0;
	public Image m_DashText = null;
	public Image m_UltText = null;

	protected override void Start()
	{
		base.Start();
		GameManager.Instance.m_Player = gameObject;
		CameraManager.Instance.m_Camera = Camera.main;
		CameraManager.Instance.Camera.transform.position = transform.position;
		SoundManager.Instance.PlaySound("BGM");
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GameManager.Instance.GameStart();
		m_Weapon.Setting(this, (WepType)Random.Range(0, 3), 1);
		GetComponent<SpriteRenderer>().material.SetInt("IsGlow", 0);
	}

	private void Update()
	{
		CameraManager.Instance.Camera.transform.position = Vector3.Lerp(CameraManager.Instance.Camera.transform.position, transform.position + new Vector3(0, 0, -10), 0.1f * 60 * Time.unscaledDeltaTime);
		if (m_MainState == State.Idle || m_MainState == State.Walk)
		{
			if (Input.GetKey(KeyCode.A))
			{
				transform.Translate(new Vector3(-m_Speed, 0) * Time.deltaTime);
				transform.localScale = new Vector2(-1, 1);
				if (m_SubState == State.Ground)
				{
					m_CurAnim = "Walk";
				}
			}

			if (Input.GetKey(KeyCode.D))
			{
				transform.Translate(new Vector3(m_Speed, 0) * Time.deltaTime);
				transform.localScale = new Vector2(1, 1);
				if (m_SubState == State.Ground)
				{
					m_CurAnim = "Walk";
				}
			}

			if (m_SubState == State.Ground)
			{
				if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
				{
					m_CurAnim = "Idle";
				}

				if (Input.GetKeyDown(KeyCode.Space))
				{
					m_CurAnim = "Jump";
					m_SubState = State.Air;
					m_Rigid.drag = 0;
					m_Rigid.AddForce(new Vector2(0, m_JumpForce));
				}

				if(Input.GetKeyDown(KeyCode.LeftShift) && m_Dash <= 0)
				{
					m_Dash = 3;
					m_DashVel = new Vector2(transform.localScale.x * m_DashSpeed, 0);
					m_MainState = State.Dash;
					m_DashParticle.Play();
					m_CurAnim = "Dash";
					SoundManager.Instance.PlaySound("Dash");
				}
			}

			if (Input.GetKeyDown(KeyCode.Q) && m_Ult < 0)
			{
				if (GameManager.Instance.m_Bricks >= 15)
				{
					m_Ult = 20;
					GameManager.Instance.m_Bricks -= 15;
					m_CurAnim = "Ult";
					m_MainState = State.Down;
				}
			}

			if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				if(GameManager.Instance.m_Bricks >= 10)
				{
					GameManager.Instance.m_Bricks -= 10;
					m_Weapon.Setting(this, (WepType)Random.Range(0, 3), 2);
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				if (GameManager.Instance.m_Bricks >= 20)
				{
					GameManager.Instance.m_Bricks -= 20;
					m_Weapon.Setting(this, (WepType)Random.Range(0, 3), 3);
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				if (GameManager.Instance.m_Bricks >= 30)
				{
					GameManager.Instance.m_Bricks -= 30;
					m_Weapon.Setting(this, (WepType)Random.Range(0, 3), 4);
				}
			}
		}

		if(m_MainState == State.Dash)
		{
			transform.position += (Vector3)m_DashVel * Time.deltaTime;
			m_DashVel *= Mathf.Pow(0.95f, 60 * Time.deltaTime);
			if(Mathf.Abs(m_DashVel.x) <= m_Speed)
			{
				m_DashVel = Vector2.zero;
				m_MainState = State.Idle;
				m_DashParticle.Stop();
				m_CurAnim = "Idle";
			}
		}
		else
		{
			m_DashVel = Vector2.zero;
			m_DashParticle.Stop();
		}
		m_DashParticle.transform.localScale = transform.localScale;

		if (Input.GetKey(KeyCode.Mouse1) && m_BrickGauge >= 2)
		{
			m_BrickGauge -= 20f * Time.deltaTime;
			GameObject[] Temp = GameObject.FindGameObjectsWithTag("Brick");
			Vector3 Pos = CameraManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
			for(int i = 0; i < Temp.Length; i++)
			{
				Temp[i].transform.parent.GetComponent<Rigidbody2D>().AddForce((Pos - Temp[i].transform.position) * 20f);
			}
		}
		else
		{
			m_BrickGauge = Mathf.Clamp(m_BrickGauge + 8f * Time.deltaTime, 0, 100);
		}
		m_BrickBar.fillAmount = m_BrickGauge / 100f;
		m_HpBar.fillAmount = m_Hp / m_MaxHp;
		m_DurabilityBar.fillAmount = m_Weapon.m_Duration / 500;
		m_Score.text = "SCORE  " + GameManager.Instance.m_Score.ToString();
		m_Brick.text = "BRICK  " + GameManager.Instance.m_Bricks.ToString();

		if(m_Hp <= 0)
		{
			m_CurAnim = "Death";
			m_MainState = State.Down;
			StopAllCoroutines();
			GetComponent<SpriteRenderer>().material.SetInt("IsGlow", 0);
			CameraManager.Instance.Camera.transform.position = Vector3.Lerp(CameraManager.Instance.Camera.transform.position, transform.position + new Vector3(0, 0, -10), 0.1f * 60 * Time.unscaledDeltaTime);
		}
		else
		{
			Vector3 Pos = CameraManager.Instance.Camera.transform.position;
			Pos.x = Mathf.Clamp(Pos.x, -19, 19);
			Pos.y = Mathf.Clamp(Pos.y, -1, 1);
			CameraManager.Instance.Camera.transform.position = Pos;
		}

		m_Ult -= Time.deltaTime;
		m_Dash -= Time.deltaTime;
		m_UltText.fillAmount = m_Ult / 20f;
		m_DashText.fillAmount = m_Dash / 3f;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		switch (collision.gameObject.tag)
		{
			case "HitBox":
				{
					if (m_MainState == State.Down)
						return;
					HitBox Temp = collision.GetComponent<HitBox>();
					if (Temp.m_IsPlayer == true)
						return;
					DealDamage(Temp);
					SoundManager.Instance.PlaySound("Hit");
					break;
				}
		}
	}

	public void CreateUltBrick()
	{
		StartCoroutine(CR_CreateUltBrick());
	}

	private IEnumerator CR_CreateUltBrick()
	{
		float Dir = transform.localScale.x;
		for (int i = 0; i < 50; i++)
		{
			ObjectManager.Instance.AddObject("Ult_Brick", transform.position + new Vector3(Random.Range(-15f, 7f) * Dir, 10, 0)).transform.localEulerAngles = new Vector3(0, 0, 30 * Dir);
			CameraManager.Instance.Shake(0.5f, 0.1f);
			yield return new WaitForSeconds(0.05f);
		}
	}

	public void PlayImpactSound()
	{
		switch (m_Weapon.m_Type)
		{
			case WepType.Sword: SoundManager.Instance.PlaySound("Sword_Impact" + Random.Range(1, 4).ToString()); break;
			case WepType.Hammer: SoundManager.Instance.PlaySound("Hammer_Impact" + Random.Range(1, 5).ToString()); break;
			case WepType.Pickaxe: SoundManager.Instance.PlaySound("Hammer_Impact" + Random.Range(1, 5).ToString()); break;
		}
	}

	public void GameEnd()
	{
		m_GameOverCanvas.SetActive(true);
		m_GameOverCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "SCORE  " + GameManager.Instance.m_Score.ToString();
		GameManager.Instance.StopAllCoroutines();
		GameManager.Instance.m_Ingame = false;
	}
}
