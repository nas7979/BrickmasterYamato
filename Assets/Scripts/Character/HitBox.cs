using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : Object
{
	public float m_Damage;
	public float m_HitTime;
	public float m_StopTime;
	public float m_Shake;
	public Vector2 m_Force;
	public GameObject m_Owner;
	public bool m_IsPlayer = false;

	public void Set(float _Damage, Vector2 _Force, float _HitTime, float _StopTime, float _Shake)
	{
		m_Damage = _Damage;
		m_Force = _Force;
		m_HitTime = _HitTime;
		m_StopTime = _StopTime;
		m_Shake = _Shake;
	}
}
