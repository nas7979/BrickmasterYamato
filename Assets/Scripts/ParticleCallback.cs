using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCallback : Object
{
	public void OnEnable()
	{
		GetComponent<ParticleSystem>().Play();
	}

	public void OnParticleSystemStopped()
	{
		Deactivate();
	}
}
