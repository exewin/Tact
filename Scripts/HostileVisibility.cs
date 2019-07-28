using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileVisibility : MonoBehaviour 
{

	//?
	void FixedUpdate()
	{
		GetComponent<Renderer>().enabled = false;
	}



}