using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour 
{
	private const int LOG_LIMIT = 30;
	
	public GameObject singleText;
	public Transform parentLog;
	
	private List<GameObject> texts;
	
	void Start()
	{
		texts = new List<GameObject>();
	}
	
	public void Send(string s)
	{
		if(texts.Count == LOG_LIMIT)
		{
			GameObject temp = texts[0];
			Destroy(temp);
			texts.Remove(temp);
		}
		
		GameObject newText = Instantiate(singleText) as GameObject;
		newText.GetComponent<UILogText>().SetText(s);
		newText.transform.SetParent(parentLog,false);
		
		texts.Add(newText);
	}
	
}
