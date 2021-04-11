using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Object
{
	public float m_Level = 0;
	public Rigidbody2D m_Rigid;

	private void Start()
	{
		m_Rigid = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (transform.position.y > 8)
		{
			Deactivate();
			if (GameManager.Instance.m_Bricks != 50)
			{
				GameManager.Instance.m_Bricks++;
			}
			else
			{
				GameManager.Instance.m_Score += 50000;
			}
		}
	}

	public void OnTriggerStay2D(Collider2D collision)
	{
		switch (collision.gameObject.tag)
		{
			case "Enemy":
				{
					Enemy Temp = collision.GetComponent<Enemy>();
					if (Temp.m_MainState == State.Down)
						return;
					if (m_Rigid.velocity.magnitude > 7)
					{
						Temp.DealDamage(1, Temp.m_SubState == State.Air ? m_Rigid.velocity : new Vector2(m_Rigid.velocity.x, 0), 1f + m_Level * 0.3f, 0, 0.1f);
						Deactivate();
						SoundManager.Instance.PlaySound("Sword_Hit" + Random.Range(1, 4).ToString());
					}

				break;
				}
		}
	}
}
