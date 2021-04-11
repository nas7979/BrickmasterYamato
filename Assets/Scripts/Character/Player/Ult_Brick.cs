using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ult_Brick : Object
{
	private void Update()
	{
		transform.Translate(0, -40 * Time.deltaTime, 0);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Enemy"))
		{
			float Dist = collision.transform.position.x - transform.position.x;
			collision.GetComponent<Enemy>().DealDamage(50, new Vector2(100 * Mathf.Sign(Dist), 100), 1f, 0, 0.1f);
		}
		if(collision.CompareTag("Ground"))
		{
			Deactivate();
			ObjectManager.Instance.AddObject("UltExplosion", transform.position);
			if (Random.Range(0, 3) == 0)
			{
				SoundManager.Instance.PlaySound("Grenade_Explode" + Random.Range(1, 4).ToString());
			}
		}
	}
}
