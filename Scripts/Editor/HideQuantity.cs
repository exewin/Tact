using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item),true), CanEditMultipleObjects]
public class HideQuantity : Editor 
{
	
	public override void OnInspectorGUI()
	{
		Item item = (Item)target;
		
		if (item.stackable)
		{
			item.quantity = EditorGUILayout.IntField("Start Quantity", item.quantity);
		}
		base.OnInspectorGUI();
	}
	
}
