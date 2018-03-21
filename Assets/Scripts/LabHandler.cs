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
    public GameObject spliceButton;
    private Specie firstInsertedSpecie;
    private Specie secondInsertedSpecie;

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
    List<DisasterProperty> chosenProperties;
    int numberOfWorkers = 0;
    Coroutine cameraMovementCoroutine; //coroutine for moving camera
    private Vector3 defaultCamPos;
    public Vector3 spliceCamPos;
    public Vector3 cloneCamPos;

    // Use this for initialization
    void Awake()
    {
        chosenProperties = new List<DisasterProperty>();
        cameraMovementCoroutine = null;
        foreach (Specie specie in GameState.instance.knownSpecies)
        {
            GameObject newSpecieButton = Instantiate(draggableObject, spliceUIPanel.transform);
            newSpecieButton.GetComponentInChildren<Text>().text = specie.name;
            print(specie.name);
            newSpecieButton.GetComponent<DragableUI>().setSpecie(specie);
        }

        foreach (Specie specie in GameState.instance.mySplices)
        {
            GameObject newSpliceButton = Instantiate(draggableObject, spliceUIPanel.transform);
            newSpliceButton.GetComponentInChildren<Text>().text = specie.name;
            print(specie.name);
            newSpliceButton.GetComponent<DragableUI>().setSpecie(specie);
        }


        topLevelSpliceUI.SetActive(false);
        topLevelCloneUI.SetActive(false);

        firstInsertedSpecie = null;
        secondInsertedSpecie = null;
        specieToClone = null;
        defaultCamPos = Camera.main.transform.position;
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
                specieToClone = draggedObject.GetComponent<DragableUI>().getSpecie();
            }
        }

        if (cloneButton.GetComponent<MouseOverObj>().isMouseOver && specieToClone != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameState.instance.addJob(JobType.CLONE,numberOfWorkers, 0))
                {
                    cloner.Clone(specieToClone, numberOfWorkers);
                }
                numberOfWorkers = 0;
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
        if (Input.GetMouseButtonUp(0))
        {

            if (spliceMachines[0].GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
            {
                chosenProperties.Clear();
                splicer.setFirstSpecie(draggedObject.GetComponent<DragableUI>().getSpecie());
                spliceMachines[0].transform.Find("spliceImage").GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<Image>().sprite;
                //better handled from splicer probably
                //if (secondInsertedSpecie != null)
                //{
                //    //instantiate screen to select shit from
                //}
            }

            if (spliceMachines[1].GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
            {
                chosenProperties.Clear();
                splicer.setSecondSpecie(draggedObject.GetComponent<DragableUI>().getSpecie());
                spliceMachines[1].transform.Find("spliceImage").GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<Image>().sprite;
                //better handled from splicer probably
                //if (firstInsertedSpecie != null)
                //{
                //    //instantiate screen to select shit from
                //}
            }
            resetDraggedObject();
        }
        if (spliceButton.GetComponent<MouseOverObj>().isMouseOver && firstInsertedSpecie != null && secondInsertedSpecie != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameState.instance.addJob(JobType.SPLICE,numberOfWorkers,1))
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
