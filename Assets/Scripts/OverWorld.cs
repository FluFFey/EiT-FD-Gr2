using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Overworld : MonoBehaviour
{
    private const int numberOfInteractables = 5;
    public GameObject[] highlightableChildren;
    public GameObject[] UIPopUps;
    public GameObject helpOverlay;
    public GameObject overlayToggle;

    private enum HIGHLIGHTABLE_OBJECTS
    {
        LABORATORY,
        FOREST,
        FARM,
        POND,
        BOTTLE,
        NO_HIGHLIGHT
    }

    private enum UI_POP_UPS
    {
        FOREST,
        POND,
        BOTTLE,
        NO_UI_POP_UPS
    }


    private void Start()
    {
        helpOverlay.SetActive(GameState.instance.showHelpOverlay);
        //UIPopUps = new GameObject[(int)UI_POP_UPS.NO_UI_POP_UPS];
        //first child is 1, 0 is current gameObject
        //UIPopUps[(int)UI_POP_UPS.FOREST] = GameObject.Find("ForestUI");
        //UIPopUps[(int)UI_POP_UPS.POND] = GameObject.Find("PondUI");
        //UIPopUps[(int)UI_POP_UPS.BUSH] = GameObject.Find("BushUI");
        //UIPopUps[(int)UI_POP_UPS.BOTTLE] = GameObject.Find("MessageInABottleUI");
        for (int i = 0; i < (int)UI_POP_UPS.NO_UI_POP_UPS; i++)
        {
            print(UIPopUps[i].name);
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

    public void toggleOverlay()
    {
        helpOverlay.SetActive(!helpOverlay.activeSelf);
        GameState.instance.showHelpOverlay = helpOverlay.activeSelf;
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
                    disableUIPopups();
                    SoundManager.instance.playSound(SoundManager.SOUNDS.ENTER_LAB);
                    SceneHandler.instance.changeScene(SceneHandler.SCENES.LAB);
                    break;
                case HIGHLIGHTABLE_OBJECTS.FOREST:
                    disableUIPopups(UI_POP_UPS.FOREST);
                    UIPopUps[(int)UI_POP_UPS.FOREST].SetActive(true);
                    break;
                case HIGHLIGHTABLE_OBJECTS.FARM:
                    disableUIPopups();
                    SceneHandler.instance.changeScene(SceneHandler.SCENES.FARM);
                    break;
                case HIGHLIGHTABLE_OBJECTS.POND:
                    disableUIPopups(UI_POP_UPS.POND);
                    UIPopUps[(int)UI_POP_UPS.POND].SetActive(true);
                    break;
                case HIGHLIGHTABLE_OBJECTS.BOTTLE:
                    disableUIPopups(UI_POP_UPS.BOTTLE);
                    //NaturalDisaster firstND = GameState.instance.getFirstDisasterDate();
                    int firstNDDate = GameState.instance.getFirstDisasterDate();
                    print(firstNDDate);
                    string disasterString ="";
                    switch (GameState.instance.getDisaster(firstNDDate).property)
                    {
                        case DisasterProperty.EARTHQUAKE:
                            disasterString = "n earthquake";
                            break;
                        case DisasterProperty.WATER:
                            disasterString = " tsunami";
                            break;
                        case DisasterProperty.WIND:
                            disasterString = " hurricane";
                            break;
                    }
                    if (firstNDDate - GameState.instance.getDaysPassed() == 1)
                    {
                        UIPopUps[(int)UI_POP_UPS.BOTTLE].GetComponentInChildren<Text>().text = "There will be a" + disasterString + " TONIGHT!";
                    }
                    else
                    {
                        UIPopUps[(int)UI_POP_UPS.BOTTLE].GetComponentInChildren<Text>().text = "There will be a" + disasterString + " in " + (firstNDDate - GameState.instance.getDaysPassed()).ToString() + " days!";
                    }
                    
                    UIPopUps[(int)UI_POP_UPS.BOTTLE].SetActive(true);
                    SoundManager.instance.playSound(SoundManager.SOUNDS.READ_MESSAGE);
                    break;
                case HIGHLIGHTABLE_OBJECTS.NO_HIGHLIGHT:
                    disableUIPopups();
                    break;
                default:
                    print("invalid highlightable object type");
                    break;
            }
        }
    }

}
