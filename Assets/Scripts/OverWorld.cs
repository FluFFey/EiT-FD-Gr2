using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overworld : MonoBehaviour
{
    private const int numberOfInteractables = 6;
    public GameObject[] children;
    private enum highlightableObjects
    {
        LABORATORY,
        FOREST,
        FARM,
        BUSH,
        POND,
        BOTTLE,
        NO_HIGHLIGHT
    }
    private void Start()
    {
        //children = new GameObject[numberOfInteractables];
        ////first child is 1, 0 is current gameObject
        //for (int i = 1; i < numberOfInteractables;i++)
        //{
        //    children[i] = transform.GetChild(i).gameObject;
        //}
    }
    void Update ()
    {
		
	}

    private void OnMouseDown()
    {
        highlightableObjects highlightedButton = highlightableObjects.NO_HIGHLIGHT;
        for (int i =0; i < numberOfInteractables; i++)
        {
            if (children[i].GetComponent<MouseOverObj>().isMouseOver)
            {
                highlightedButton = (highlightableObjects)i;
            }
        }

        switch (highlightedButton)
        {
            case highlightableObjects.LABORATORY:
                SceneManager.LoadScene("Laboratory", LoadSceneMode.Single);
                SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
                break;
            case highlightableObjects.FOREST:

                break;
            case highlightableObjects.FARM:
                SceneManager.LoadScene("Forest", LoadSceneMode.Single);
                SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
                break;
            case highlightableObjects.BUSH:
                break;
            case highlightableObjects.POND:
                break;
            case highlightableObjects.BOTTLE:
                break;
            case highlightableObjects.NO_HIGHLIGHT:
                break;
            default:
                print("invalid highlightable object type");
                break;
        }
    }
}
