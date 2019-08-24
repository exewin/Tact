using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour 
{
	
	[SerializeField] private GameObject singleText;
	[SerializeField] private Transform parentLog;
	
	private List<GameObject> texts;
	
	private void Start()
	{
		texts = new List<GameObject>();
	}
	
	public void Send(string s)
	{
		if(texts.Count == Formulas.LOG_LIMIT)
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
