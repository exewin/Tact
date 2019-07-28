using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour 
{
	
	public static int mercActive = 0;
	public GameObject highlight;
	
	
	void Update()
	{
		//Select merc by numeric keypad
		for(int i=0;i<6;i++)
		{
			if(Input.GetKeyDown(""+(i+1)))
			{
				mercActive=i;
				SetActiveMercHighlight(i);
			}
		}
	}
	
	//Select merc by UI portaits
	public void SelectMerc(int i)
	{
		mercActive=i;
		SetActiveMercHighlight(i);
	}
	
	
	void SetActiveMercHighlight(int i)
	{
		if(i<3)
			highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(82*i,0);
		else
			highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(82*(i-3),-82);
	}
	
}
