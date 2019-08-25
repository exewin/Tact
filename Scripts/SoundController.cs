using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour 
{

	public AudioClip emptySound;

	public void PlayEmpty(Transform pos)
	{
		PlayAtPoint(emptySound, pos);
	}
	
	public void PlayAtPoint(AudioClip sound, Transform pos)
	{
		GameObject g = new GameObject();
		g.transform.position = pos.position;
		AudioSource a = g.AddComponent<AudioSource>();
		a.spatialBlend = 0;
		a.clip = sound;
		a.Play(); // start the sound
        Destroy(g, a.clip.length); //
	}
	
	
}
