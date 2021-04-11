using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Object
{
	public float m_Speed;

	public void Update()
	{
		transform.Translate(m_Speed * transform.localScale.x * Time.deltaTime, 0, 0);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			if (collision.GetComponent<Player>().m_MainState == State.Down)
				return;
			Deactivate();
			collision.GetComponent<Player>().DealDamage(10, new Vector2(400 * transform.localScale.x, 0), 0.3f, 0.2f, 0.1f);
			SoundManager.Instance.PlaySound("Hit");
		}
	}
}
