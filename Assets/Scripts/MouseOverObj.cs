using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverObj : MonoBehaviour
{
    public bool isMouseOver;

    private void OnMouseOver()
    {
        isMouseOver = true;
    }
    private void OnMouseExit()
    {
        isMouseOver = false;
    }
}
