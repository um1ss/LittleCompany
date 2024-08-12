using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickName : MonoBehaviour
{
    public event Action<string> Click;

    public void ClickButton()
    {
        Click?.Invoke(gameObject.name);
		
    }

	private void OnMouseEnter()
	{
		
	}

	private void OnMouseExit()
	{
		
	}
}
