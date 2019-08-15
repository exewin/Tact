using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	public static int mercActive = 0;
	public static List<GameObject> enemies = new List<GameObject>();
	public static List<GameObject> mercs = new List<GameObject>();
		
	void Awake()
	{
		Application.targetFrameRate = 60;
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("Hostile"))
		{
			enemies.Add(e);
		}
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("Player"))
		{
			mercs.Add(e);
		}
	}
	
	public static void RemoveFromList(GameObject g)
	{
		enemies.Remove(g);
	}
}
