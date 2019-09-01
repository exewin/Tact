using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBackpackSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private int index;
	private Item info;
	private InventoryController inv;
	
	private Vector3 iconPos;
	private Transform draggingArea;
	[SerializeField] private Image icon;
	[SerializeField] private Text quantity;
	
	public RectTransform area;
	
	public void Assign(InventoryController controller, Item item, int i, Transform drag, RectTransform weaponT)
	{
		area = weaponT;
		draggingArea = drag;
		inv = controller;
		index = i;
		info = item;
		icon.sprite = info.image;
		iconPos = icon.transform.localPosition;
		if(item.stackable)
		{
			quantity.gameObject.SetActive(true);
			quantity.text = ""+item.quantity;
		}
	}
	
	public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
		icon.transform.SetParent(draggingArea);
    }

	public void OnDrag(PointerEventData eventData)
	{
		icon.transform.position = eventData.position;
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		
		if(RectTransformUtility.RectangleContainsScreenPoint(area, Input.mousePosition, null))
		{
			Debug.Log("yes");
			Destroy(icon);
			inv.EquipItem(index);
		}
		else
		{
			Debug.Log("no");
			icon.transform.SetParent(transform);
			icon.transform.localPosition = iconPos;
		}
	}
	

}
