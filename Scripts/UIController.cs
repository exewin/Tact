﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class UIController : MonoBehaviour 
{
	private StatsMerc mercScript;
	[SerializeField] private CursorController cursorControl;
	[SerializeField] private InventoryController invetoryControl;
	
	//UI
	[SerializeField] private Image[] UITeam = new Image[6];
	[SerializeField] private Text UINickname;
	[SerializeField] private Image UIPortrait;
	[SerializeField] private Text UIHpText;
	[SerializeField] private RectTransform UIHpBar;	
	[SerializeField] private Text UIApText;
	[SerializeField] private RectTransform UIApBar;
	
	//UI Stats
	[SerializeField] private Text UIStatStrength;
	[SerializeField] private Text UIStatAccuracy;
	[SerializeField] private Text UIStatAgility;
	[SerializeField] private Text UIStatReflex;
	
	//UI Equipment
	[SerializeField] private Image UIWeapon;
	[SerializeField] private Text UIWeaponCapacity;
	[SerializeField] private Image UIArmor;
	[SerializeField] private Image UIHelmet;
	
	[SerializeField] private Button UIBurstSingle;
	[SerializeField] private Button UIBurstBurst;
	[SerializeField] private Button UIBurstAuto;
	[SerializeField] private Text UIBurstSingleText;
	[SerializeField] private Text UIBurstBurstText;
	[SerializeField] private Text UIBurstAutoText;
	
	//UI State
	[SerializeField] private Button UIStand;
	[SerializeField] private Button UICrouch;
	[SerializeField] private Button UICrawl;

	//UI necessary gos
	[SerializeField] private Sprite transparent;
	[SerializeField] private GameObject highlight;
	[SerializeField] private GameObject backpack;
	
	private void Start()
	{
		int i = 0;
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("Player"))
		{
			mercScript = e.GetComponent<StatsMerc>();
			cursorControl.mercs[i]=mercScript;
			UITeam[i].sprite = mercScript.portrait;
			i++;
		}
		SelectMerc(0);
		UIControl();
	}
	

	private void Update()
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
		if(i>=GameController.mercs.Count)
			return;
		
		if(GameController.mercs[i] == null)
			return;
		
		GameController.mercActive=i;
		mercScript=GameController.mercs[i].GetComponent<StatsMerc>();
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
		invetoryControl.GetMerc(GameController.mercs[GameController.mercActive]);
	}
	
	public void MercDeath(int id)
	{
		UITeam[id].sprite = null; // CZACHA??? TODO
	}
	
	//Update UI
	public void UIControl()
	{
		invetoryControl.UpdateInventory();
		
		UINickname.text = mercScript.nickname;
		UIPortrait.sprite = mercScript.portrait;
		
		UIHpText.text = mercScript.hp + "/" + mercScript.maxHp;
		UIHpBar.localScale = new Vector3((float)mercScript.hp/(float)mercScript.maxHp,1f,1f);
		
		UIStatAccuracy.text = mercScript.accuracy + "";
		UIStatStrength.text = mercScript.strength + "";
		UIStatAgility.text = mercScript.agility + "";
		UIStatReflex.text = mercScript.reflex + "";
		
		if(mercScript.weapon)
		{
			UIWeapon.sprite = mercScript.weapon.image;
			UIWeaponCapacity.text = mercScript.weapon.bulletsLeft + "/" + mercScript.weapon.capacity;
			if(mercScript.weapon.single)
				UIBurstSingle.interactable = true;
			else
				UIBurstSingle.interactable = false;

			if(mercScript.weapon.burst)
				UIBurstBurst.interactable = true;
			else
				UIBurstBurst.interactable = false;
		
			if(mercScript.weapon.auto)
				UIBurstAuto.interactable = true;
			else
				UIBurstAuto.interactable = false;

			
			if(mercScript.weapon.mode == burstMode.single)
			{
				UIBurstSingleText.color = Color.red;
				UIBurstBurstText.color = Color.white;
				UIBurstAutoText.color = Color.white;
			}
			else if(mercScript.weapon.mode == burstMode.burst)
			{
				UIBurstSingleText.color = Color.white;
				UIBurstBurstText.color = Color.red;
				UIBurstAutoText.color = Color.white;
			}
			else if(mercScript.weapon.mode == burstMode.auto)
			{
				UIBurstSingleText.color = Color.white;
				UIBurstBurstText.color = Color.white;
				UIBurstAutoText.color = Color.red;
			}
		}
		else
		{
			UIBurstBurst.interactable = false;
			UIBurstSingle.interactable = false;
			UIBurstAuto.interactable = false;
			UIWeapon.sprite = transparent;
			UIWeaponCapacity.text = "";
		}
		if(mercScript.armor)
			UIArmor.sprite = mercScript.armor.image;
		else
			UIArmor.sprite = transparent;
		if(mercScript.helmet)
			UIHelmet.sprite = mercScript.helmet.image;
		else
			UIHelmet.sprite = transparent;
	}
	
	public void UIAp()
	{
		UIApText.text = mercScript.ap.ToString("F0") + "/" + mercScript.maxAp;
		UIApBar.localScale = new Vector3((float)mercScript.ap/(float)mercScript.maxAp,1f,1f);
	}
	
}
