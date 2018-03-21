using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class LabHandler : MonoBehaviour
{
    enum LAB_VIEWS
    {
        LOBBY,
        CLONING,
        SPLICING
    }
    LAB_VIEWS currentView = LAB_VIEWS.LOBBY;

    //Splice related variables
    public GameObject topLevelSpliceUI;
    public SpliceScreen splicer;
    public Cloning cloner;
    public GameObject spliceGO;
    public GameObject spliceUIPanel;
    public GameObject leaveSplicing;
    public GameObject[] spliceMachines;
    public GameObject addSpliceWorkerButton;
    public GameObject removeSpliceWorkerButton;
    public GameObject mySplicesShelf;
    public GameObject spliceButton;
    public GameObject leftPropertyScreen;
    public GameObject rightPropertyScreen;
    private Specie firstInsertedSpecie;
    private Specie secondInsertedSpecie;
    private TextMesh statsText;
    private TextMesh numberOfWorkersText;
    private int selectedSplice;

    //Clone related variables
    public GameObject topLevelCloneUI;
    public GameObject cloneGO;
    public GameObject cloneUIPanel;
    public GameObject cloneMachine;
    public GameObject addCloneWorkerButton;
    public GameObject removeCloneWorkerButton;
    public GameObject cloneButton;
    public GameObject leaveCloning;
    private Specie specieToClone;

    //Other
    public GameObject draggableObject;
    public GameObject leaveLabDoor;
    private GameObject draggedObject;
    List<DisasterProperty> chosenDisasterProperties;
    List<Specie.PositiveProperty> chosenNormalProperties;
    int numberOfWorkers = 1;
    Coroutine cameraMovementCoroutine; //coroutine for moving camera
    private Vector3 defaultCamPos;
    public Vector3 spliceCamPos;
    public Vector3 cloneCamPos;

    // Use this for initialization
    void Awake()
    {
        chosenDisasterProperties = new List<DisasterProperty>();
        chosenNormalProperties = new List<Specie.PositiveProperty>();
        cameraMovementCoroutine = null;
        foreach (Specie specie in GameState.instance.knownSpecies)
        {
            GameObject newSpecieButton = Instantiate(draggableObject, spliceUIPanel.transform);
            newSpecieButton.GetComponentInChildren<Text>().text = specie.name;
            newSpecieButton.GetComponent<DragableUI>().setSpecie(specie);
        }

        //foreach (Specie specie in GameState.instance.mySplices)
        //{
        //    GameObject newSpliceButton = Instantiate(draggableObject, spliceUIPanel.transform);
        //    newSpliceButton.GetComponentInChildren<Text>().text = specie.name;
        //    print(specie.name);
        //    newSpliceButton.GetComponent<DragableUI>().setSpecie(specie);
        //}

        topLevelSpliceUI.SetActive(false);
        topLevelCloneUI.SetActive(false);

        firstInsertedSpecie = null;
        secondInsertedSpecie = null;
        specieToClone = null;
        numberOfWorkersText = GameObject.Find("numberOfAssignedWorkers").GetComponent<TextMesh>();
        updateNumberOfWorkersDisplay();
        statsText = GameObject.Find("randomStatsText").GetComponent<TextMesh>();
        defaultCamPos = Camera.main.transform.position;
        selectedSplice = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO:check state first before input probably
        switch (currentView)
        {
            case LAB_VIEWS.LOBBY:
                handleLabLobbyInput();
                break;
            case LAB_VIEWS.CLONING:
                handleCloneInput();
                break;
            case LAB_VIEWS.SPLICING:
                handleSpliceInput(); //TODO: split functions to handling and execution. currently does both
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Lobby input
    /// </summary>
    private void handleLabLobbyInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (leaveLabDoor.GetComponent<MouseOverObj>().isMouseOver)
            {
                //TODO: add visualization of  mouse over
                SceneHandler.instance.changeScene(SceneHandler.SCENES.OVERWORLD);
            }
            if (cloneGO.GetComponent<MouseOverObj>().isMouseOver)
            {
                //TODO: add visualization of  mouse over
                numberOfWorkers = 1;
                switchToClone();
            }
            if (spliceGO.GetComponent<MouseOverObj>().isMouseOver)
            {
                //TODO: add visualization of  mouse over
                numberOfWorkers = 1;
                switchToSplice();
            }
        }
    }
    /// <summary>
    /// handle clone input
    /// </summary>
    private void handleCloneInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //TODO: add visualization of  mouse over
            if (leaveCloning.GetComponent<MouseOverObj>().isMouseOver)
            {
                switchToMainLab();
            }

            if (addCloneWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                numberOfWorkers++;
                updateNumberOfWorkersDisplay();
            }

            if (removeCloneWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                numberOfWorkers--;
                updateNumberOfWorkersDisplay();
            }

            if (cloneMachine.GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
            {
                specieToClone = draggedObject.GetComponent<DragableUI>().getSpecie();
            }

            if (cloneButton.GetComponent<MouseOverObj>().isMouseOver && specieToClone != null)
            {
                if (GameState.instance.addJob(JobType.CLONE, numberOfWorkers, 0))
                {
                    cloner.Clone(specieToClone, numberOfWorkers);
                }
                numberOfWorkers = 0;
            }

        }
    }

    void updateNumberOfWorkersDisplay()
    {
        numberOfWorkers = numberOfWorkers > 3 ? 3 : numberOfWorkers;
        numberOfWorkers = numberOfWorkers < 1 ? 1 : numberOfWorkers;
        numberOfWorkersText.text = ((int)numberOfWorkers / 10).ToString() + " " + ((int)numberOfWorkers % 10).ToString();
    }

    void handlePropertySelection()
    {
        //TODO: add graphics for showing a button is pushed down
        for (int i = 0; i < leftPropertyScreen.transform.childCount; i++)
        {
            if (leftPropertyScreen.transform.GetChild(i).GetComponent<MouseOverObj>().isMouseOver)
            {
                if (i < firstInsertedSpecie.resistantProperties.Count)
                {
                    //basically toggling. TODO: Add dynamic function for this and other property type(s)
                    if (chosenDisasterProperties.Contains(firstInsertedSpecie.resistantProperties[i]))
                    {
                      //  print("leftresistance removed");
                        chosenDisasterProperties.Remove(firstInsertedSpecie.resistantProperties[i]);
                    }
                    else
                    {
                    //    print("leftresistance added");
                        chosenDisasterProperties.Add(firstInsertedSpecie.resistantProperties[i]);
                    }
                }
                else
                {
                    int normalPropertyNr = i - firstInsertedSpecie.resistantProperties.Count;
                    if (chosenNormalProperties.Contains(firstInsertedSpecie.positiveProperties[normalPropertyNr]))
                    {
                    //    print("leftnormal removed");
                        chosenNormalProperties.Remove(firstInsertedSpecie.positiveProperties[normalPropertyNr]);
                    }
                    else
                    {
                    //    print("leftnormal added");
                        chosenNormalProperties.Add(firstInsertedSpecie.positiveProperties[i]);
                    }
                    //chosenNormalProperties.Add(firstInsertedSpecie.positiveProperties[i - firstInsertedSpecie.resistantProperties.Count]);
                }
            }
            //ASSUMING BOTH PROPERTY SCREENS HAVE SAME NUMBER OF CHILDREN(which they should
            if (rightPropertyScreen.transform.GetChild(i).GetComponent<MouseOverObj>().isMouseOver)
            {
                if (i < secondInsertedSpecie.resistantProperties.Count)
                {
                    //basically toggling. TODO: Add dynamic function for this and other property type(s)
                    if (chosenDisasterProperties.Contains(secondInsertedSpecie.resistantProperties[i]))
                    {
                     //   print("rightresistance removed");
                        chosenDisasterProperties.Remove(secondInsertedSpecie.resistantProperties[i]);
                    }
                    else
                    {
                      //  print("rightresistance added");
                        chosenDisasterProperties.Add(secondInsertedSpecie.resistantProperties[i]);
                    }
                }
                else
                {
                    int normalPropertyNr = i - secondInsertedSpecie.resistantProperties.Count;
                    if (chosenNormalProperties.Contains(secondInsertedSpecie.positiveProperties[normalPropertyNr]))
                    {
                      //  print("rightnormal removed");
                        chosenNormalProperties.Remove(secondInsertedSpecie.positiveProperties[normalPropertyNr]);
                    }
                    else
                    {
                       // print("rightNormal added");
                        chosenNormalProperties.Add(secondInsertedSpecie.positiveProperties[i]);
                    }
                    //chosenNormalProperties.Add(firstInsertedSpecie.positiveProperties[i - firstInsertedSpecie.resistantProperties.Count]);
                }
            }
        }
    }

    /// <summary>
    /// handle splice input
    /// </summary>
    private void handleSpliceInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (leaveSplicing.GetComponent<MouseOverObj>().isMouseOver)
            {
                switchToMainLab();
            }

            if (addSpliceWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                numberOfWorkers++;
                updateNumberOfWorkersDisplay();
            }

            if (removeSpliceWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                numberOfWorkers--;
                updateNumberOfWorkersDisplay();
            }

            if (spliceButton.GetComponent<MouseOverObj>().isMouseOver && 
                firstInsertedSpecie != null && 
                secondInsertedSpecie != null && 
                selectedSplice != -1) //is -1 when nothing is scene is initiated
            {
                if (GameState.instance.addJob(JobType.SPLICE, numberOfWorkers, 1))
                {
                    splicer.spliceSpecies(firstInsertedSpecie, secondInsertedSpecie, numberOfWorkers, chosenDisasterProperties);
                }
            }
            handlePropertySelection();
            updateSyringe();
            findSelectedCustomSplice();
        }

        if (Input.GetMouseButtonUp(0))
        {

            if (spliceMachines[0].GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
            {
                chosenDisasterProperties.Clear();
                firstInsertedSpecie = draggedObject.GetComponent<DragableUI>().getSpecie();
                spliceMachines[0].transform.Find("spliceImage").GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<Image>().sprite;
                splicer.updateStatsDisplay(firstInsertedSpecie, secondInsertedSpecie, numberOfWorkers);
            }

            if (spliceMachines[1].GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
            {
                chosenDisasterProperties.Clear();
                secondInsertedSpecie = draggedObject.GetComponent<DragableUI>().getSpecie();
                spliceMachines[1].transform.Find("spliceImage").GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<Image>().sprite;
            }
            splicer.updateStatsDisplay(firstInsertedSpecie, secondInsertedSpecie, numberOfWorkers);
            resetDraggedObject();
        }
    }

    private void findSelectedCustomSplice()
    {
        //int numberOfCustomSplices;
        for (int i = 0; i < mySplicesShelf.transform.childCount; i++)
        {
            if (mySplicesShelf.transform.GetChild(i).GetComponent<MouseOverObj>().isMouseOver)
            {
                selectedSplice = i;
            }
        }
    }

    private void updateSyringe()
    {
        //TODO: Add update to sprøyte here
        //throw new NotImplementedException();
    }

    public void setDraggedObject(GameObject newDraggedObject)
    {
        draggedObject = newDraggedObject;
    }

    public void resetDraggedObject()
    {
        draggedObject = null;
    }

    void switchToMainLab()
    {
        currentView = LAB_VIEWS.LOBBY;
        topLevelCloneUI.SetActive(false);
        topLevelSpliceUI.SetActive(false);
        cloneGO.GetComponent<BoxCollider2D>().enabled = true;
        spliceGO.GetComponent<BoxCollider2D>().enabled = true;
        if (cameraMovementCoroutine != null)
        {
            StopCoroutine(cameraMovementCoroutine);
        }
        cameraMovementCoroutine = StartCoroutine(moveCameraTowards(defaultCamPos, 2.0f));
    }

    void switchToClone()
    {
        cloneGO.GetComponent<BoxCollider2D>().enabled = false;
        currentView = LAB_VIEWS.CLONING;
        topLevelCloneUI.SetActive(true);

        if (cameraMovementCoroutine != null)
        {
            StopCoroutine(cameraMovementCoroutine);
        }
        cameraMovementCoroutine = StartCoroutine(moveCameraTowards(cloneCamPos, 2.0f));
    }

    void switchToSplice()
    {
        currentView = LAB_VIEWS.SPLICING;
        spliceGO.GetComponent<BoxCollider2D>().enabled = false;
        topLevelSpliceUI.SetActive(true);
        if (cameraMovementCoroutine !=null)
        {
            StopCoroutine(cameraMovementCoroutine);
        }
        
        cameraMovementCoroutine = StartCoroutine(moveCameraTowards(spliceCamPos, 2.0f));
    }

    IEnumerator moveCameraTowards(Vector3 target, float duration = 1.0f)
    {
        Vector3 startPos = Camera.main.transform.position;
        Vector3 moveVector = target - startPos;
        for (float f =0; f < duration; f+=Time.deltaTime)
        {
            
            float pd = f / duration;
            pd *= pd;
            Camera.main.transform.position = startPos + moveVector * pd;
            yield return null;
        }
    }
}
