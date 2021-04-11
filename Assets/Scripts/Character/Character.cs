using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
	Idle,
	Walk,
	Parry,
	Attack,
	Death,
	Hit,
	Ground,
	Down,
	Air,
	Dash
}

public class Character : Object
{
	protected readonly float GRAVITY = 4;
	[SerializeField] public float m_MaxHp;
	[SerializeField] public float m_Speed;
	[SerializeField] public HitBox m_HitBox;
	public float m_Hp;
	public State m_MainState;
	public State m_SubState;
	public Rigidbody2D m_Rigid;
	public Animator m_Anim;
	public string m_CurAnim = "Idle";
	public float m_HitStopTime = 0;

	protected virtual void Start()
	{
		m_Rigid = GetComponent<Rigidbody2D>();
		m_Anim = GetComponent<Animator>();
	}

	protected virtual void OnEnable()
	{
		m_SubState = State.Air;
		m_MainState = State.Idle;
		m_CurAnim = "Idle";
		m_Hp = m_MaxHp;
	}

	private void LateUpdate()
	{
		m_HitStopTime -= Time.deltaTime;
		if (m_HitStopTime <= 0 && m_MainState == State.Hit && m_SubState == State.Ground)
		{
			m_MainState = State.Idle;
			m_CurAnim = "Idle";
		}
		m_Anim.Play(m_CurAnim);
	}

	public virtual void DealDamage(HitBox _HitBox)
	{
		m_Hp -= _HitBox.m_Damage;
		if (_HitBox.m_HitTime > 0.15f)
		{
			m_MainState = State.Hit;
			m_CurAnim = "Hit";
		}
		CameraManager.Instance.Shake(_HitBox.m_StopTime + 0.5f, _HitBox.m_Shake);
		GameManager.Instance.HitStop(_HitBox.m_StopTime);
		transform.localScale = (new Vector3(-_HitBox.transform.lossyScale.x, 1, 1));
		AddForce(_HitBox.m_Force.x * _HitBox.transform.lossyScale.x, _HitBox.m_Force.y);
		GetComponent<SpriteRenderer>().material.SetInt("IsGlow", 1);
		if (m_Hp > 0)
		{
			if (m_HitStopTime <= _HitBox.m_HitTime)
			{
				m_HitStopTime = _HitBox.m_HitTime;
			}
			StartCoroutine(CR_Hit());
			StartCoroutine(CR_HitShake(_HitBox.m_StopTime - 0.05f, Mathf.Clamp(_HitBox.m_Damage * 0.04f, 0.2f, 1f)));
		}
	}

	public virtual void DealDamage(float _Damage, Vector2 _Force, float _HitTime, float _StopTime, float _Shake)
	{
		m_Hp -= _Damage;
		if (_HitTime > 0.15f)
		{
			m_MainState = State.Hit;
			m_CurAnim = "Hit";
		}
		CameraManager.Instance.Shake(_StopTime + 0.5f, _Shake);
		GameManager.Instance.HitStop(_StopTime);
		AddForce(_Force.x, _Force.y);
		GetComponent<SpriteRenderer>().material.SetInt("IsGlow", 1);
		if (m_Hp > 0)
		{
			if (m_HitStopTime <= _HitTime)
			{
				m_HitStopTime = _HitTime;
			}
			StartCoroutine(CR_Hit());
			StartCoroutine(CR_HitShake(_StopTime - 0.05f, Mathf.Clamp(_Damage * 0.04f, 0.2f, 1f)));
		}
	}

	public void AddForce(float _X, float _Y)
	{
		m_Rigid.AddForce(new Vector2(_X, _Y));
		if (_Y != 0)
		{
			m_SubState = State.Air;
			m_Rigid.drag = 0;
		}
	}

	public virtual HitBox AddHitBox()
	{
		HitBox Temp = ObjectManager.Instance.AddObject("HitBox", m_HitBox.transform.position).GetComponent<HitBox>();
		Temp.GetComponent<BoxCollider2D>().size = m_HitBox.GetComponent<BoxCollider2D>().size;
		Temp.Set(m_HitBox.m_Damage, m_HitBox.m_Force, m_HitBox.m_HitTime, m_HitBox.m_Shake, m_HitBox.m_StopTime);
		Temp.m_Owner = gameObject;
		return Temp;
	}

	protected virtual void OnAnimationEnd(string _Key)
	{
		m_Anim.speed = 1;
		if (_Key == "Idle")
		{
			m_MainState = State.Idle;
			m_CurAnim = "Idle";
		}
	}

	private IEnumerator CR_Hit()
	{
		yield return new WaitForSecondsRealtime(0.05f);
		GetComponent<SpriteRenderer>().material.SetInt("IsGlow", 0);
	}

	private IEnumerator CR_HitShake(float _Time, float _Force)
	{
		Vector3 Pos = transform.position;
		float ForcePerTime = _Force / _Time;
		while (_Time > 0)
		{
			_Time -= Time.unscaledDeltaTime;
			_Force -= ForcePerTime * Time.unscaledDeltaTime;
			transform.position = Pos + new Vector3(Random.Range(-_Force, _Force), Random.Range(-_Force, _Force));
			yield return null;
		}
		transform.position = Pos;
	}

	private IEnumerator CR_Down()
	{
		yield return new WaitForSeconds(Random.Range(0.75f, 1.25f));
		m_CurAnim = "Wake";
	}

	public virtual void OnCollisionEnter2D(Collision2D collision)
	{
		switch (collision.gameObject.tag)
		{
			case "Ground":
				{
					if (m_SubState == State.Air)
					{
						if (m_MainState == State.Hit)
						{
							if (collision.relativeVelocity.y > 5)
							{
								m_Rigid.AddForce(new Vector2(collision.relativeVelocity.x * -20, collision.relativeVelocity.y * 25));
								SoundManager.Instance.PlaySound("Down" + Random.Range(1, 3).ToString());
							}
							else if (collision.relativeVelocity.y >= 0)
							{
								m_MainState = State.Down;
								m_SubState = State.Ground;
								m_CurAnim = "Down";
								m_Rigid.drag = 4;
								if (m_Hp > 0)
								{
									StartCoroutine(CR_Down());
								}
								SoundManager.Instance.PlaySound("Down" + Random.Range(1, 3).ToString());
							}
						}
						else
						{
							m_MainState = State.Idle;
							m_SubState = State.Ground;
							m_CurAnim = "Idle";
							m_Rigid.drag = 4;
						}
					}
					break;
				}
		}
	}

	public void PlaySound(string _Key)
	{
		SoundManager.Instance.PlaySound(_Key);
	}
}