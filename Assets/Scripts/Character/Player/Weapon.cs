using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WepType
{
	Sword,
	Hammer,
	Pickaxe
}

public class Weapon : Object
{
	public float m_Mass;
	public float m_MaxDuration;
	public float m_Damage;
	public float m_HitTime;
	public float m_StopTime;
	public float m_Shake;
	public Vector2 m_Force;
	public Player m_Player;
	public float m_Duration;
	public WepType m_Type;
	public BoxCollider2D m_Collider;
	public Sprite[] m_Sprites;
	public Material[] m_Materials;
	public int m_Level;


	private void Start()
	{
		m_Collider = GetComponent<BoxCollider2D>();
	}

	public void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0) && (m_Player.m_MainState == State.Idle || m_Player.m_MainState == State.Walk))
		{
			SoundManager.Instance.PlaySound("Swing" + Random.Range(1, 7).ToString());
			m_Player.m_CurAnim = m_Type == WepType.Sword ? "Attack_Sword" : "Attack_Hammer";
			m_Player.m_MainState = State.Attack;

			switch (m_Level)
			{
				case 1:
					switch (m_Type)
					{
						case WepType.Sword: m_Player.m_Anim.speed = 2f; break;
						case WepType.Hammer: m_Player.m_Anim.speed = 0.65f; break;
						case WepType.Pickaxe: m_Player.m_Anim.speed = 1.1f; break;
					} break;
				case 2:
					switch (m_Type)
					{
						case WepType.Sword: m_Player.m_Anim.speed = 2.2f; break;
						case WepType.Hammer: m_Player.m_Anim.speed = 0.65f; break;
						case WepType.Pickaxe: m_Player.m_Anim.speed = 1.2f; break;
					}
					break;
				case 3:
					switch (m_Type)
					{
						case WepType.Sword: m_Player.m_Anim.speed = 2.4f; break;
						case WepType.Hammer: m_Player.m_Anim.speed = 0.65f; break;
						case WepType.Pickaxe: m_Player.m_Anim.speed = 1.3f; break;
					}
					break;
				case 4:
					switch (m_Type)
					{
						case WepType.Sword: m_Player.m_Anim.speed = 2.6f; break;
						case WepType.Hammer: m_Player.m_Anim.speed = 0.65f; break;
						case WepType.Pickaxe: m_Player.m_Anim.speed = 1.4f; break;
					}
					break;
			}
		}
	}

	public void Destroy()
	{
		GameManager.Instance.HitStop(1, 0);
		ObjectManager.Instance.AddObject("Particle_WepDestroy", transform.position).GetComponent<ParticleSystemRenderer>().material = m_Materials[m_Level - 1];
		Setting(m_Player, (WepType)Random.Range(0, 3), 1);
		SoundManager.Instance.PlaySound("Wep_Break" + Random.Range(1, 4).ToString());
	}

	public void Setting(Player _Player, WepType _Type, int _Level)
	{
		if (_Level != 1)
		{
			ObjectManager.Instance.AddObject("Particle_WepCreate", transform.position).GetComponent<ParticleSystemRenderer>().material = m_Materials[_Level - 1];
			SoundManager.Instance.PlaySound("Wep_Create");
		}
		m_Player = _Player;
		m_Type = _Type;
		m_Level = _Level;
		m_Duration = 500;
		m_Player.m_HitBox.m_Owner = gameObject;
		GetComponent<SpriteRenderer>().sprite = m_Sprites[(int)_Type];
		GetComponent<SpriteRenderer>().material = m_Materials[_Level - 1];
		if (_Type == WepType.Sword)
		{
			switch (m_Level)
			{
				case 1:
					m_Player.m_HitBox.m_Damage = 10;
					m_Player.m_HitBox.m_StopTime = 0.05f;
					m_Player.m_HitBox.m_HitTime = 0.05f;
					m_Player.m_HitBox.m_Shake = 0.05f;
					m_Player.m_HitBox.m_Force = new Vector2(150, 0); break;
				case 2:
					m_Player.m_HitBox.m_Damage = 11;
					m_Player.m_HitBox.m_StopTime = 0.05f;
					m_Player.m_HitBox.m_HitTime = 0.05f;
					m_Player.m_HitBox.m_Shake = 0.05f;
					m_Player.m_HitBox.m_Force = new Vector2(200, 0); break;
				case 3:
					m_Player.m_HitBox.m_Damage = 12;
					m_Player.m_HitBox.m_StopTime = 0.05f;
					m_Player.m_HitBox.m_HitTime = 0.05f;
					m_Player.m_HitBox.m_Shake = 0.05f;
					m_Player.m_HitBox.m_Force = new Vector2(250, 0); break;
				case 4:
					m_Player.m_HitBox.m_Damage = 13;
					m_Player.m_HitBox.m_StopTime = 0.05f;
					m_Player.m_HitBox.m_HitTime = 0.05f;
					m_Player.m_HitBox.m_Shake = 0.05f;
					m_Player.m_HitBox.m_Force = new Vector2(300, 0); break;
			}
		}
		else if (_Type == WepType.Hammer)
		{
			switch (m_Level)
			{
				case 1:
					m_Player.m_HitBox.m_Damage = 25;
					m_Player.m_HitBox.m_StopTime = 0.15f;
					m_Player.m_HitBox.m_HitTime = 0.6f;
					m_Player.m_HitBox.m_Shake = 0.1f;
					m_Player.m_HitBox.m_Force = new Vector2(50, 800); break;
				case 2:
					m_Player.m_HitBox.m_Damage = 35;
					m_Player.m_HitBox.m_StopTime = 0.15f;
					m_Player.m_HitBox.m_HitTime = 0.65f;
					m_Player.m_HitBox.m_Shake = 0.11f;
					m_Player.m_HitBox.m_Force = new Vector2(100, 900); break;
				case 3:
					m_Player.m_HitBox.m_Damage = 45;
					m_Player.m_HitBox.m_StopTime = 0.15f;
					m_Player.m_HitBox.m_HitTime = 0.7f;
					m_Player.m_HitBox.m_Shake = 0.12f;
					m_Player.m_HitBox.m_Force = new Vector2(150, 1000); break;
				case 4:
					m_Player.m_HitBox.m_Damage = 55;
					m_Player.m_HitBox.m_StopTime = 0.15f;
					m_Player.m_HitBox.m_HitTime = 0.75f;
					m_Player.m_HitBox.m_Shake = 0.13f;
					m_Player.m_HitBox.m_Force = new Vector2(200, 1100); break;
			}
		}
		else if (_Type == WepType.Pickaxe)
		{
			switch (m_Level)
			{
				case 1:
					m_Player.m_HitBox.m_Damage = 15;
					m_Player.m_HitBox.m_StopTime = 0.1f;
					m_Player.m_HitBox.m_HitTime = 0.1f;
					m_Player.m_HitBox.m_Shake = 0.07f;
					m_Player.m_HitBox.m_Force = new Vector2(300, 0); break;
				case 2:
					m_Player.m_HitBox.m_Damage = 18;
					m_Player.m_HitBox.m_StopTime = 0.1f;
					m_Player.m_HitBox.m_HitTime = 0.1f;
					m_Player.m_HitBox.m_Shake = 0.07f;
					m_Player.m_HitBox.m_Force = new Vector2(400, 0); break;
				case 3:
					m_Player.m_HitBox.m_Damage = 21;
					m_Player.m_HitBox.m_StopTime = 0.1f;
					m_Player.m_HitBox.m_HitTime = 0.1f;
					m_Player.m_HitBox.m_Shake = 0.07f;
					m_Player.m_HitBox.m_Force = new Vector2(500, 0); break;
				case 4:
					m_Player.m_HitBox.m_Damage = 24;
					m_Player.m_HitBox.m_StopTime = 0.1f;
					m_Player.m_HitBox.m_HitTime = 0.1f;
					m_Player.m_HitBox.m_Shake = 0.07f;
					m_Player.m_HitBox.m_Force = new Vector2(600, 0); break;
			}
		}
	}
}
