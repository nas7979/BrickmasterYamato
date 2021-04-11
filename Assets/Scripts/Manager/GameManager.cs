using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public float m_Score;
	public bool m_Ingame = false;
	public GameObject m_Player = null;
	public int m_Bricks = 0;

	private void Start()
	{
		m_Score = 0;
	}

	public void GameStart()
	{
		m_Score = 0;
		m_Bricks = 0;
		m_Ingame = true;
		StartCoroutine(CR_CreateEnemy());
	}

	public void HitStop(float _Time, float _Speed = 0, bool _Gradually = false)
	{
		Time.timeScale = _Speed;
		//Time.fixedDeltaTime = 0.02f * Time.timeScale;
		StartCoroutine(CR_HitStop(_Time, _Gradually));
	}

	private IEnumerator CR_HitStop(float _Time, bool _Gradually)
	{
		if (_Gradually)
		{
			float TimePerTime = (1f - Time.timeScale) / _Time;
			while (_Time > 0)
			{
				_Time -= Time.unscaledDeltaTime;
				Time.timeScale += (TimePerTime * Time.unscaledDeltaTime);
				//Time.fixedDeltaTime = 0.02f * Time.timeScale;
				yield return null;
			}
		}
		else
		{
			yield return new WaitForSecondsRealtime(_Time);
		}
		//Time.fixedDeltaTime = 0.02f;
		Time.timeScale = 1;
	}

	private IEnumerator CR_CreateEnemy()
	{
		float Time = 4;
		while (m_Ingame)
		{
			yield return new WaitForSecondsRealtime(Time);
			if(m_Ingame == false)
				yield break;
			Time = Mathf.Max(Time - 0.01f, 2f);
			switch (Random.Range(0, 3))
			{
				case 0: ObjectManager.Instance.AddObject("Enemy_Slime", new Vector2(Random.Range(0, 2) == 0 ? -28 : 28, 0)); break;
				case 1: ObjectManager.Instance.AddObject("Enemy_Goblin", new Vector2(Random.Range(0, 2) == 0 ? -28 : 28, 0)); break;
				case 2: ObjectManager.Instance.AddObject("Enemy_Nepent", new Vector2(Random.Range(0, 2) == 0 ? -28 : 28, 0)); break;
			}
		}
	}
}
