using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class UIController : MonoBehaviour 
{
	private List<GameObject> mercs = new List<GameObject>();
	private StatsMerc mercScript;
	[SerializeField] private CursorController cursorControl;
	
	//UI
	[SerializeField] private Image[] UITeam = new Image[6];
	[SerializeField] private Text UINickname;
	[SerializeField] private Image UIPortrait;
	[SerializeField] private Text UIHpText;
	[SerializeField] private RectTransform UIHpBar;
	
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

	
	[SerializeField] private Sprite transparent;
	[SerializeField] private GameObject highlight;
	[SerializeField] private GameObject backpack;
	
	void Start()
	{
		int i = 0;
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("Player"))
		{
			mercs.Add(e);
			mercScript = e.GetComponent<StatsMerc>();
			cursorControl.mercs[i]=mercScript;
			UITeam[i].sprite = mercScript.portrait;
			i++;
		}
		SelectMerc(0);
		UIControl();
	}
	
	public void RemoveMerc(GameObject g)
	{
		mercs.Remove(g);
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
		if(i>=mercs.Count)
			return;
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
		
		GetComponent<InventoryController>().UpdateInventory();
		
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
	
}
