﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class UIController : MonoBehaviour 
{

	List<GameObject> mercs = new List<GameObject>();
	StatsMerc mercScript;
	public CursorController cursorControl;
	
	//UI
	public Image[] UITeam = new Image[6];
	public Text UINickname;
	public Image UIPortrait;
	public Text UIHpText;
	public RectTransform UIHpBar;
	
	//UI Stats
	public Text UIStatStrength;
	public Text UIStatAccuracy;
	public Text UIStatAgility;
	public Text UIStatReflex;
	
	//UI Equipment
	public Image UIWeapon;
	public Image UIArmor;
	public Image UIHelmet;
	public Image UIAmmo;
	public Text UIAmmoQuantity;
	
	public Sprite transparent; //using blank transparent sprite for empty equipment slot
	public GameObject highlight; //UI team highlight
	public GameObject backpack; //inventory gameobject
	
	void Start()
	{
		//Store mercs in List
		int i = 0;
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("Player"))
		{
			mercs.Add(e);
			mercs[i].SendMessage("SetID", i);
			mercScript = e.GetComponent<StatsMerc>();
			cursorControl.mercs[i]=mercScript;
			UITeam[i].sprite = mercScript.portrait;
			i++;
		}
		SelectMerc(0);
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
	}
	
	private void SelectMerc(int i)
	{
		GameController.mercActive=i;
		mercScript=mercs[i].GetComponent<StatsMerc>();
		SetActiveMercHighlight(i);
	}
	
	//Highlight portrait in UI
	private void SetActiveMercHighlight(int i)
	{
		if(i<3)
			highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(82*i,0);
		else
			highlight.GetComponent<RectTransform>().anchoredPosition = new Vector2(82*(i-3),-82);
		
		SendMerc();
		UIControl();
	}
	
	//Backpack button
	public void Backpack()
	{
		if(!backpack.activeSelf)
			backpack.SetActive(true);
		else
			backpack.SetActive(false);
	}
	
	private void SendMerc()
	{
		GetComponent<InventoryController>().GetMerc(mercs[GameController.mercActive]);
	}
	

	
	//Update UI
	public void UIControl()
	{
		
		UINickname.text = mercScript.nickname;
		UIPortrait.sprite = mercScript.portrait;
		
		UIHpText.text = mercScript.hp + "/" + mercScript.maxHp;
		UIHpBar.localScale = new Vector3((float)mercScript.hp/(float)mercScript.maxHp,1f,1f);
		
		UIStatAccuracy.text = mercScript.accuracy + "";
		UIStatStrength.text = mercScript.strength + "";
		UIStatAgility.text = mercScript.agility + "";
		UIStatReflex.text = mercScript.reflex + "";
		
		if(mercScript.weapon)
			UIWeapon.sprite = mercScript.weapon.image;
		else
			UIWeapon.sprite = transparent;
		if(mercScript.armor)
			UIArmor.sprite = mercScript.armor.image;
		else
			UIArmor.sprite = transparent;
		if(mercScript.helmet)
			UIHelmet.sprite = mercScript.helmet.image;
		else
			UIHelmet.sprite = transparent;
		if(mercScript.ammo)
		{
			UIAmmo.sprite = mercScript.ammo.image;
			UIAmmoQuantity.text = ""+mercScript.ammo.quantity;
		}
		else
		{
			UIAmmo.sprite = transparent;
			UIAmmoQuantity.text = "";
		}
		
	}
	
}