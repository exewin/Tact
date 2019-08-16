using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	public static int mercActive = 0;
	public static List<Visibility> humans = new List<Visibility>();
	private int idAllocator=1;
		
	void Awake()
	{
		Application.targetFrameRate = 60;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
		{
			Visibility e = g.GetComponent<Visibility>();
			humans.Add(e);
			e.SetID(idAllocator, humans);
			idAllocator++;
		}
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Hostile"))
		{
			Visibility e = g.GetComponent<Visibility>();
			humans.Add(e);
			e.SetID(idAllocator, humans);
			idAllocator++;
		}
	}
	
	public static void RemoveFromList(GameObject g)
	{
		humans.Remove(g.GetComponent<Visibility>());
	}
}
