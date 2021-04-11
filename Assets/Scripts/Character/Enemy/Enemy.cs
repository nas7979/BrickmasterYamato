using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
	[SerializeField] private Canvas m_Canvas = null;
	[SerializeField] private Image m_HpBar = null;
	public float m_AttackChance = 0.01f;
	public float m_AttackRange = 50f;
	public int m_AttackVariety = 1;

	protected override void Start()
	{
		base.Start();
		m_Canvas.worldCamera = CameraManager.Instance.Camera;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		m_Hp = m_MaxHp;
		if(m_HitBox != null)
		m_HitBox.gameObject.SetActive(false);
		GetComponent<SpriteRenderer>().material.SetInt("IsGlow", 0);
		m_HpBar.fillAmount = m_Hp / m_MaxHp;
	}

	public void Update()
	{
		m_Canvas.transform.localScale = new Vector3(transform.localScale.x == -1 ? -0.01f : 0.01f, 0.01f, 0.01f);
		if (m_MainState == State.Idle || m_MainState == State.Walk)
		{
			transform.localScale = new Vector3(Mathf.Sign(GameManager.Instance.m_Player.transform.position.x - transform.position.x), 1, 1);
			float Dist = Mathf.Abs(GameManager.Instance.m_Player.transform.position.x - transform.position.x);
			if (Dist <= m_AttackRange)
			{
				if (Random.Range(0f, 1f) / Time.deltaTime <= m_AttackChance)
				{
					m_CurAnim = string.Concat("Attack", Random.Range(1, m_AttackVariety + 1).ToString());
					m_MainState = State.Attack;
				}
				else
				{
					m_CurAnim = "Idle";
					m_MainState = State.Idle;
				}
			}
			else
			{
				m_CurAnim = "Walk";
				m_MainState = State.Walk;
				transform.Translate(new Vector3(m_Speed * transform.lossyScale.x, 0, 0) * Time.deltaTime);
			}
		}
	}

	public override void DealDamage(HitBox _HitBox)
	{
		base.DealDamage(_HitBox);
		if (_HitBox.m_Owner.CompareTag("Weapon"))
		{
			Weapon Wep = _HitBox.m_Owner.GetComponent<Weapon>();
			Wep.m_Duration -= _HitBox.m_Damage;
			if(Wep.m_Duration <= 0)
			{
				Wep.Destroy();
			}
			SoundManager.Instance.PlaySound("Enemy_Hit" + Random.Range(1, 5).ToString());

			for (int i = 0; i < 1; i++)
			{
				GameObject Temp = ObjectManager.Instance.AddObject("Brick", transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
				Temp.GetComponent<Brick>().m_Level = Wep.m_Level;
				Temp.GetComponent<SpriteRenderer>().material = _HitBox.m_Owner.GetComponent<SpriteRenderer>().material;
				Temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-500f, 500f), Random.Range(-500f, 500f)));
			}
		}
		m_HpBar.fillAmount = m_Hp / m_MaxHp;
		ObjectManager.Instance.AddObject("Particle_EnemyHit", transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0));
		if (m_Hp <= 0 && gameObject.activeInHierarchy)
		{
			Deactivate();
			ObjectManager.Instance.AddObject("Particle_EnemyDeath", transform.position);
			GameManager.Instance.m_Score += 100000;
			SoundManager.Instance.PlaySound("Virus_Explosion_Large" + Random.Range(1, 7).ToString());
		}
	}

	public override void DealDamage(float _Damage, Vector2 _Force, float _HitTime, float _StopTime, float _Shake)
	{
		base.DealDamage(_Damage, _Force, _HitTime, _StopTime, _Shake);
		m_HpBar.fillAmount = m_Hp / m_MaxHp;
		ObjectManager.Instance.AddObject("Particle_EnemyHit", transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0));
		if (m_Hp <= 0 && gameObject.activeInHierarchy)
		{
			Deactivate();
			ObjectManager.Instance.AddObject("Particle_EnemyDeath", transform.position);
			GameManager.Instance.m_Score += 100000;
			SoundManager.Instance.PlaySound("Virus_Explosion_Large" + Random.Range(1, 7).ToString());
		}
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
					if (Temp.m_IsPlayer == false)
						return;
					DealDamage(Temp);
					break;
				}
		}
	}

	public void FireBullet()
	{
		ObjectManager.Instance.AddObject("Nepent_Bullet", transform.position).transform.localScale = transform.localScale;
	}
}
