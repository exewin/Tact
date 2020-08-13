using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	public static int mercActive = 0;
	public static List<Visibility> humans = new List<Visibility>();
	public static List<GameObject> mercs = new List<GameObject>();
	private int idAllocator=1;
		
	void Awake()
	{
		Application.targetFrameRate = 60;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
		{
			mercs.Add(g);
			g.GetComponent<StatsMerc>().SetID(idAllocator);
			Visibility e = g.GetComponent<Visibility>();
			humans.Add(e);
			e.SetID(idAllocator);
			idAllocator++;
		}
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Hostile"))
		{
			Visibility e = g.GetComponent<Visibility>();
			humans.Add(e);
			e.SetID(idAllocator);
			idAllocator++;
		}
	}
	
	public static void RemoveFromList(GameObject g)
	{
		humans.Remove(g.GetComponent<Visibility>());
	}
}
