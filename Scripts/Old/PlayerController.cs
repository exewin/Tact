using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	int id;
	public Camera cam;

	public void SetID(int i)
	{
		id = i;
	}
	
}



/*
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
*/