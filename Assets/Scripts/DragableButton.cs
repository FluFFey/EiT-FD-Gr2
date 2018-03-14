using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableButton : MonoBehaviour {

    Rect buttonRect = new Rect(10, 10, 100, 20);
    bool buttonPressed = false;
    Specie specie;
    
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnGUI()
    {
        if (buttonRect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.MouseDown)
            {
                buttonPressed = true;
            }
            if (Event.current.type == EventType.MouseUp)
            {
                buttonPressed = false;
            }
        }
        if (buttonPressed && Event.current.type == EventType.MouseDrag)
        {
            buttonRect.x += Event.current.delta.x;
            buttonRect.y += Event.current.delta.y;
        }
        GUI.Button(buttonRect, "Draggable Button");
    }

    internal void setSpecie(Specie newSpecie)
    {
        specie = newSpecie;
    }

    internal Specie getSpecie()
    {
        return specie;
    }

}
