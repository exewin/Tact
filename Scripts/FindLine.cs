using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLine : MonoBehaviour 
{

	public bool Line(Transform a, Transform b, LayerMask c)
	{
		
		if (Physics.Linecast(b.position, b.position, c))
		{
			return false;
		}
		else
			return true;
		
	}

}
