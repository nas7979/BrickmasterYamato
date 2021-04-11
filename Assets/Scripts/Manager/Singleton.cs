using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance = null;

	public static T Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}

	protected virtual void Awake()
	{
		GameObject Find = GameObject.Find(typeof(T).Name);
		if(Find != gameObject && instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = GetComponent<T>();
			DontDestroyOnLoad(gameObject);
		}
	}
}
