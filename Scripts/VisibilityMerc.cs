using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityMerc : Visibility
{
	
	protected override void ActionTrue(Visibility human)
	{
		if(human.tag == "Hostile")
		{
			VisibilityHostile hostile = (VisibilityHostile) human; 
			hostile.Visible();
			if(id-1==GameController.mercActive)
			{
				hostile.ChangeColor(Color.red);
			}
		}
	}	
	
	protected override void ActionFalse(Visibility human)
	{
		if(id-1==GameController.mercActive)
		{
			if(human.tag == "Hostile")
			{
				VisibilityHostile hostile = (VisibilityHostile) human; 
				hostile.ChangeColor(Color.black);
			}
		}
	}
	
}