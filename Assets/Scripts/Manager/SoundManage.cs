using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	Dictionary<string, AudioClip> m_Sounds = new Dictionary<string, AudioClip>();

	protected override void Awake()
	{
		base.Awake();
		AudioClip[] Temp;
		Temp = Resources.LoadAll<AudioClip>("Sound");
		for (int i = 0; i < Temp.Length; i++)
		{
			m_Sounds.Add(Temp[i].name, Temp[i]);
		}
	}

	public void PlaySound(string _Key)
	{
		AudioSource Temp = ObjectManager.Instance.AddObject("SFX", Vector3.zero).GetComponent<AudioSource>();
		Temp.clip = m_Sounds[_Key];
		Temp.Play();
	}
}
