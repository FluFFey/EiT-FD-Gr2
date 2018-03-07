using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overworld : MonoBehaviour
{
    private const int numberOfInteractables = 6;
    public GameObject[] highlightableChildren;
    GameObject[] UIPopUps;

    private enum HIGHLIGHTABLE_OBJECTS
    {
        LABORATORY,
        FOREST,
        FARM,
        BUSH,
        POND,
        BOTTLE,
        NO_HIGHLIGHT
    }

    private enum UI_POP_UPS
    {
        FOREST,
        POND,
        BUSH,
        BOTTLE,
        NO_UI_POP_UPS
    }


    private void Start()
    {
        UIPopUps = new GameObject[(int)UI_POP_UPS.NO_UI_POP_UPS];
        //first child is 1, 0 is current gameObject
        UIPopUps[(int)UI_POP_UPS.FOREST] = GameObject.Find("ForestUI");
        UIPopUps[(int)UI_POP_UPS.POND] = GameObject.Find("PondUI");
        UIPopUps[(int)UI_POP_UPS.BUSH] = GameObject.Find("BushUI");
        UIPopUps[(int)UI_POP_UPS.BOTTLE] = GameObject.Find("MessageInABottleUI");
        for (int i = 0; i < (int)UI_POP_UPS.NO_UI_POP_UPS; i++)
        {
            UIPopUps[i].SetActive(false);
        }

        //children = new GameObject[numberOfInteractables];
        ////first child is 1, 0 is current gameObject
        //for (int i = 1; i < numberOfInteractables;i++)
        //{
        //    children[i] = transform.GetChild(i).gameObject;
        //}
    }
    void Update ()
    {
        handleInput();

    }

    private void disableUIPopups(UI_POP_UPS exclusion = UI_POP_UPS.NO_UI_POP_UPS)
    {
        for (int i = 0; i < (int)UI_POP_UPS.NO_UI_POP_UPS; i++)
        {
            if (i != (int)exclusion)
            {
                UIPopUps[i].SetActive(false);
            }
        }
    }

    private void handleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HIGHLIGHTABLE_OBJECTS highlightedButton = HIGHLIGHTABLE_OBJECTS.NO_HIGHLIGHT;
            for (int i = 0; i < numberOfInteractables; i++)
            {
                if (highlightableChildren[i].GetComponent<MouseOverObj>().isMouseOver)
                {
                    highlightedButton = (HIGHLIGHTABLE_OBJECTS)i;
                }
            }
 
            switch (highlightedButton)
            {
                case HIGHLIGHTABLE_OBJECTS.LABORATORY:
                    SceneHandler.instance.changeScene(SceneHandler.SCENES.LAB);
                    break;
                case HIGHLIGHTABLE_OBJECTS.FOREST:
                    disableUIPopups(UI_POP_UPS.FOREST);
                    UIPopUps[(int)UI_POP_UPS.FOREST].SetActive(true);
                    break;
                case HIGHLIGHTABLE_OBJECTS.FARM:
                    SceneHandler.instance.changeScene(SceneHandler.SCENES.FARM);
                    break;
                case HIGHLIGHTABLE_OBJECTS.BUSH:
                    disableUIPopups(UI_POP_UPS.BUSH);
                    UIPopUps[(int)UI_POP_UPS.BUSH].SetActive(true);
                    break;
                case HIGHLIGHTABLE_OBJECTS.POND:
                    disableUIPopups(UI_POP_UPS.POND);
                    UIPopUps[(int)UI_POP_UPS.POND].SetActive(true);
                    break;
                case HIGHLIGHTABLE_OBJECTS.BOTTLE:
                    disableUIPopups(UI_POP_UPS.BOTTLE);
                    UIPopUps[(int)UI_POP_UPS.BOTTLE].SetActive(true);
                    break;
                case HIGHLIGHTABLE_OBJECTS.NO_HIGHLIGHT:
                    break;
                default:
                    print("invalid highlightable object type");
                    break;
            }
        }
    }

}
