using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalScript : MonoBehaviour 
{
	
	const int TEAM_LIMIT = 6;
	
	public static int mercActive = 0;
	public GameObject highlight;
	
	public List<MercStats> mercs = new List<MercStats>();
	
	//UI
	public Image[] UITeam = new Image[6];
	public Text UINickname;
	public Image UIPortrait;
	public Text UIHpText;
	public RectTransform UIHpBar;
	
	void Start()
	{
		//Store mercs in List
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("Player"))
		{
			mercs.Add(e.GetComponent<MercStats>());
		}	
		for(int i=0;i<Mathf.Min(TEAM_LIMIT,mercs.Count);i++)
		{
			UITeam[i].sprite = mercs[i].portrait;
		}
		
		
		
		UIControl();
	}
	
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
	
	//Highlight portrait in UI
	void SetActiveMercHighlight(int i)
	{
		if(i<3)
			highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(82*i,0);
		else
			highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(82*(i-3),-82);
		
		UIControl();
	}
	
	//Update UI
	public void UIControl()
	{
		UINickname.text = mercs[mercActive].nickname;
		UIPortrait.sprite = mercs[mercActive].portrait;
		
		UIHpText.text = mercs[mercActive].hp + "/"+ mercs[mercActive].maxHp;
		UIHpBar.localScale = new Vector3((float)mercs[mercActive].hp/(float)mercs[mercActive].maxHp,1f,1f);
		
	}
	
}
