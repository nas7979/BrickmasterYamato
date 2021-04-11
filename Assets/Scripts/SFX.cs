using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : Object
{
	private AudioSource m_Audio = null;

	private void Start()
	{
		m_Audio = GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{
		if(m_Audio.isPlaying == false)
		{
			Deactivate();
		}
	}
}
