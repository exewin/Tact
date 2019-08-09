using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour 
{
	private float speed;
	public void Info(float velocity)
	{
		speed = velocity;
	}
	private void FixedUpdate()
	{
		transform.Translate(0,0,speed*Time.deltaTime);
	}
}
