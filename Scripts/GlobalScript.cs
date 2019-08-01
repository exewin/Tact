using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalScript : MonoBehaviour 
{
	
	const int TEAM_LIMIT = 6;
	
	public static int mercActive = 0;
	public GameObject highlight;
	
	public List<GameObject> mercs = new List<GameObject>();
	MercStats mercScript;
	
	//UI
	public Image[] UITeam = new Image[6];
	public Text UINickname;
	public Image UIPortrait;
	public Text UIHpText;
	public RectTransform UIHpBar;
	
	public GameObject backpack;
	
	void Start()
	{
		//Store mercs in List
		int i = 0;
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("Player"))
		{
			mercs.Add(e);
			mercScript = e.GetComponent<MercStats>();
			UITeam[i].sprite = mercScript.portrait;
			i++;
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
				SelectMerc(i);
				SetActiveMercHighlight(i);
			}
		}
		//Show backpack (key i)
		if(Input.GetKeyDown(KeyCode.I))
		{
			Backpack();
		}
	}
	
	//Select merc by UI portaits
	public void SelectMercByPortrait(int i)
	{
		SelectMerc(i);
		SetActiveMercHighlight(i);
	}
	
	private void SelectMerc(int i)
	{
		mercActive=i;
		mercScript=mercs[i].GetComponent<MercStats>();
	}
	
	//Highlight portrait in UI
	private void SetActiveMercHighlight(int i)
	{
		if(i<3)
			highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(82*i,0);
		else
			highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(82*(i-3),-82);
		
		UIControl();
	}
	
	//Backpack button
	public void Backpack()
	{
		if(!backpack.active)
			backpack.SetActive(true);
		else
			backpack.SetActive(false);
	}
	
	private void SendInventory()
	{
		GetComponent<InventoryController>().GetInventory(mercs[mercActive].GetComponent<Inventory>().items);
	}
	
	//Update UI
	private void UIControl()
	{
		UINickname.text = mercScript.nickname;
		UIPortrait.sprite = mercScript.portrait;
		
		UIHpText.text = mercScript.hp + "/"+ mercScript.maxHp;
		UIHpBar.localScale = new Vector3((float)mercScript.hp/(float)mercScript.maxHp,1f,1f);
		
		SendInventory();
		
	}
	
}
