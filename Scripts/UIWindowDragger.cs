using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWindowDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
	
	[SerializeField] private RectTransform toDrag;

	public void OnBeginDrag(PointerEventData eventData)
    {
		//may be useful?
    }

	public void OnDrag(PointerEventData eventData)
	{
		toDrag.position += (Vector3)eventData.delta;
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		float realWidth = Screen.width/1920f;
		float realHeight = Screen.height/1080f;
		
		//window snapping works only for pivot 0,0 and TopLeft anchor
		
		if(toDrag.position.x<0)
			toDrag.position = new Vector3(0, toDrag.position.y, toDrag.position.z);

		if(toDrag.position.x>=Screen.width-(toDrag.sizeDelta.x*realWidth))
			toDrag.position = new Vector3(Screen.width-(toDrag.sizeDelta.x*realWidth), toDrag.position.y, toDrag.position.z);		
		
		if(toDrag.position.y<0)
			toDrag.position = new Vector3(toDrag.position.x, 0, toDrag.position.z);

		if(toDrag.position.y>=Screen.height-(toDrag.sizeDelta.y*realHeight))
			toDrag.position = new Vector3(toDrag.position.x, Screen.height-(toDrag.sizeDelta.y*realHeight), toDrag.position.z);
	}
}
