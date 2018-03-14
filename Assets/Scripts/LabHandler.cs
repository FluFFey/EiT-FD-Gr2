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

    public GameObject topLevelCloneUI;
    public GameObject topLevelSpliceUI;

    public SpliceScreen splicer;
    public GameObject spliceGO;
    public GameObject spliceUIPanel;
    
    public GameObject leaveSplicing;
    public GameObject[] spliceMachines;
    public GameObject addSpliceWorkerButton;
    public GameObject removeSpliceWorkerButton;
    public GameObject spliceButton;
    private Specie firstInsertedSpecie;
    private Specie secondInsertedSpecie;

    public GameObject cloneGO;
    public GameObject cloneUIPanel;
    public GameObject cloneMachine;
    public GameObject addCloneWorkerButton;
    public GameObject removeCloneWorkerButton;
    public GameObject cloneButton;
    public GameObject leaveCloning;
    private Specie specieToClone;

    public GameObject draggableObject;
    public GameObject leaveLabDoor;

    private GameObject draggedObject;
    List<DisasterProperty> chosenProperties;
    int numberOfWorkers = 0;



    Coroutine cameraMovementCoroutine;

    // Use this for initialization
    void Awake()
    {
        chosenProperties = new List<DisasterProperty>();
        cameraMovementCoroutine = null;
        foreach (Specie specie in GameState.instance.knownSpecies)
        {
            GameObject newSpecieButton = Instantiate(draggableObject, spliceUIPanel.transform);
            newSpecieButton.GetComponentInChildren<Text>().text = specie.name;
            newSpecieButton.GetComponent<DragableUI>().setSpecie(specie);
        }

        foreach (Specie specie in GameState.instance.mySplices)
        {
            GameObject newSpliceButton = Instantiate(draggableObject, spliceUIPanel.transform);
            newSpliceButton.GetComponentInChildren<Text>().text = specie.name;
            newSpliceButton.GetComponent<DragableUI>().setSpecie(specie);
        }


        topLevelSpliceUI.SetActive(false);
        topLevelCloneUI.SetActive(false);
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
        if (Input.GetMouseButtonUp(0))
        {
            resetDraggedObject();
        }
    }
    /// <summary>
    /// Lobby input
    /// </summary>
    private void handleLabLobbyInput()
    {
        if (leaveLabDoor.GetComponent<MouseOverObj>().isMouseOver)
        {
            //TODO: add visualization of  mouse over
            if (Input.GetMouseButtonDown(0))
            {
                SceneHandler.instance.changeScene(SceneHandler.SCENES.OVERWORLD);
            }
        }
        if (cloneGO.GetComponent<MouseOverObj>().isMouseOver)
        {
            //TODO: add visualization of  mouse over
            if (Input.GetMouseButtonDown(0))
            {
                numberOfWorkers = 0;
                switchToClone();
            }
        }
        if (spliceGO.GetComponent<MouseOverObj>().isMouseOver)
        {
            //TODO: add visualization of  mouse over
            if (Input.GetMouseButtonDown(0))
            {
                numberOfWorkers = 0;
                switchToSplice();
            }
        }
    }
    /// <summary>
    /// handle clone input
    /// </summary>
    private void handleCloneInput()
    {
        if (leaveCloning.GetComponent<MouseOverObj>().isMouseOver)
        {
            //TODO: add visualization of  mouse over
            if (Input.GetMouseButtonDown(0))
            {
                switchToMainLab();
            }
        }

        if (addCloneWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                numberOfWorkers++;
            }
        }

        if (removeCloneWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                numberOfWorkers--;
            }
        }

        if (cloneMachine.GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                specieToClone = draggedObject.GetComponent<DragableButton>().getSpecie();
            }
        }

        if (cloneButton.GetComponent<MouseOverObj>().isMouseOver && cloneMachine != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //TODO: job handling here
                //TODO: add clone function here
                numberOfWorkers--;
            }
        }

    }
    /// <summary>
    /// handle splice input
    /// </summary>
    private void handleSpliceInput()
    {
        if (leaveSplicing.GetComponent<MouseOverObj>().isMouseOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                switchToMainLab();
            }
        }

        if (addSpliceWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                numberOfWorkers++;
            }
        }

        if (removeSpliceWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                numberOfWorkers--;
            }
        }

        if (spliceMachines[0].GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                chosenProperties.Clear();
                firstInsertedSpecie = draggedObject.GetComponent<DragableButton>().getSpecie();
                if (secondInsertedSpecie != null)
                {
                    //instantiate screen to select shit from
                }
            }
        }

        if (spliceMachines[1].GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                chosenProperties.Clear();
                secondInsertedSpecie = draggedObject.GetComponent<DragableButton>().getSpecie();
                if (firstInsertedSpecie != null)
                {
                    //instantiate screen to select shit from
                }
            }
        }

        if (spliceButton.GetComponent<MouseOverObj>().isMouseOver && firstInsertedSpecie != null && secondInsertedSpecie != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //TODO: job handling here
                int availableWorkers = 10;
                if (numberOfWorkers < availableWorkers)
                {
                    splicer.spliceSpecies(firstInsertedSpecie, secondInsertedSpecie, numberOfWorkers, chosenProperties);
                }

            }
        }
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
        cameraMovementCoroutine = StartCoroutine(moveCameraTowards(Vector3.back * 5, 2.0f));
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
        cameraMovementCoroutine = StartCoroutine(moveCameraTowards(cloneGO.transform.position - Vector3.forward * 4,2.0f));
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
        
        cameraMovementCoroutine = StartCoroutine(moveCameraTowards(spliceGO.transform.position - Vector3.forward * 4, 2.0f));
    }

    IEnumerator moveCameraTowards(Vector3 target, float duration = 1.0f)
    {
        Vector3 startPos = Camera.main.transform.position;
        Vector3 moveVector = target - startPos;
        print(moveVector);
        for (float f =0; f < duration; f+=Time.deltaTime)
        {
            
            float pd = f / duration;
            pd *= pd;
            Camera.main.transform.position = startPos + moveVector * pd;
            yield return null;
        }
    }
}
