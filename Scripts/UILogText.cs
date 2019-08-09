using UnityEngine;
using UnityEngine.UI;

public class UILogText : MonoBehaviour 
{
	
	public void SetText(string s)
	{
		GetComponent<Text>().text = s+".";
	}

}
