using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	public static int mercActive = 0;
	public static List<GameObject> enemies = new List<GameObject>();
		
	void Awake()
	{
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("Hostile"))
		{
			enemies.Add(e);
		}
	}
	
	public static void RemoveFromList(GameObject g)
	{
		enemies.Remove(g);
	}
}
