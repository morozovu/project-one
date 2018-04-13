﻿using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
	public ParticleSystem[] allParticles;


	public void Play()
	{
		foreach (ParticleSystem ps in allParticles)
		{
			ps.Stop();
			ps.Play();
		}
	}


	void Start()
	{
		allParticles = GetComponentsInChildren<ParticleSystem>();
	}
}
