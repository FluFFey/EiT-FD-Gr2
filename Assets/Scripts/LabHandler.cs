using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LabHandler : MonoBehaviour
{
    enum LAB_VIEWS
    {
        LOBBY,
        CLONING,
        SPLICING
    }
    LAB_VIEWS currentView = LAB_VIEWS.LOBBY;
    Coroutine syringeCoroutine = null;

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
    public Sprite displayButtonDownSprite;
    public Sprite displayButtonUpSprite;
    public Sprite propertyButtonUp;
    public Sprite propertyButtonDown;
    public GameObject syringeHandle;
    private Specie firstInsertedSpecie;
    private Specie secondInsertedSpecie;
    public TextMesh numberOfSpliceWorkersText;
    private int selectedSplice;

    //Clone related variables
    public GameObject cloneGO;
    public GameObject addCloneWorkerButton;
    public GameObject removeCloneWorkerButton;
    public GameObject cloneButton;
    public GameObject leaveCloning;
    public GameObject cloneSplicesShelf;
    public TextMesh numberOfCloneWorkersText;
    private Specie specieToClone;

    //Other
    public GameObject draggableObject;
    public GameObject leaveLabDoor;
    private GameObject draggedObject;
    List<DisasterProperty> chosenDisasterProperties;
    List<Specie.PositiveProperty> chosenNormalProperties;
    int numberOfWorkers = 1;
    Coroutine cameraRotationCoroutine; //coroutine for moving camera
    private Vector3 defaultCamPos;
    public Vector3 spliceCamPos;
    public Vector3 cloneCamPos;
    private bool cameraRotationFinished;
    public GameObject goToSpliceGO;
    public GameObject goToCloneGO;
    

    // Use this for initialization
    void Awake()
    {
        chosenDisasterProperties = new List<DisasterProperty>();
        chosenNormalProperties = new List<Specie.PositiveProperty>();
        cameraRotationCoroutine = null;
        foreach (Specie specie in GameState.instance.knownSpecies)
        {
            GameObject newSpecieButton = Instantiate(draggableObject, spliceUIPanel.transform);
            

            if (specie.image !=null)
            {
                newSpecieButton.GetComponent<Image>().sprite = specie.image;
            }
            newSpecieButton.GetComponent<DragableUI>().setSpecie(specie);
        }

        for (int i = 0; i < GameState.instance.mySplices.Count; i++)
        {
            if (GameState.instance.mySplices[i] != null)
            {
                mySplicesShelf.transform.GetChild(i).GetChild(1).GetComponent<SpriteRenderer>().sprite = GameState.instance.mySplices[i].image;
                cloneSplicesShelf.transform.GetChild(i).GetChild(1).GetComponent<SpriteRenderer>().sprite = GameState.instance.mySplices[i].image;
            }
        }

        topLevelSpliceUI.SetActive(false);

        firstInsertedSpecie = null;
        secondInsertedSpecie = null;
        specieToClone = null;

        defaultCamPos = Camera.main.transform.position;
        selectedSplice = -1;

        for (int i = 0; i < 2; i++)
        {
            switch (i)
            {
                case 0:
                    firstInsertedSpecie = GameState.instance.firstInsertedSpecie;
                    spliceMachines[i].transform.Find("spliceImage").GetComponent<SpriteRenderer>().sprite = GameState.instance.firstInsertedSpecie.image;
                    break;
                case 1:
                    secondInsertedSpecie = GameState.instance.secondInsertedSpecie;
                    spliceMachines[i].transform.Find("spliceImage").GetComponent<SpriteRenderer>().sprite = GameState.instance.secondInsertedSpecie.image;
                    break;
                default:
                    break;
            }
        }
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
                SoundManager.instance.playSound(SoundManager.SOUNDS.LEAVE_LAB);
            }
            if (goToCloneGO.GetComponent<MouseOverObj>().isMouseOver)
            {
                //TODO: add visualization of  mouse over
                numberOfWorkers = 1;
                updateNumberOfWorkersDisplay(true);
                SoundManager.instance.playSound(SoundManager.SOUNDS.DEFAULT_UI);
                switchToClone();
            }
            if (goToSpliceGO.GetComponent<MouseOverObj>().isMouseOver)
            {
                //TODO: add visualization of  mouse over
                numberOfWorkers = 1;
                updateNumberOfWorkersDisplay();
                SoundManager.instance.playSound(SoundManager.SOUNDS.DEFAULT_UI);
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
                if (!cloner.isSeedOnFloor())
                {
                    if (selectedSplice != -1)
                    {
                        cloneSplicesShelf.transform.GetChild(selectedSplice).GetComponent<MouseOverObj>().lockedOutline = false;
                        cloneSplicesShelf.transform.GetChild(selectedSplice).GetComponent<MouseOverObj>().removeOutline();
                        selectedSplice = -1;
                    }
                    cloner.resetSeedDisplay();
                    SoundManager.instance.playSound(SoundManager.SOUNDS.DEFAULT_UI);
                    switchToMainLab();
                }
                else
                {
                    StartCoroutine(blinkingOutline(cloner.getPacketGO()));
                }
                
            }

            if (addCloneWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                numberOfWorkers++;
                updateNumberOfWorkersDisplay(true);
                if (selectedSplice != -1)
                {
                    cloner.updateNumSeeds(GameState.instance.mySplices[selectedSplice], numberOfWorkers);
                }
                SoundManager.instance.playSound(SoundManager.SOUNDS.DEFAULT_UI);
            }

            if (removeCloneWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                numberOfWorkers--;
                updateNumberOfWorkersDisplay(true);
                if (selectedSplice !=-1)
                {
                    cloner.updateNumSeeds(GameState.instance.mySplices[selectedSplice], numberOfWorkers);
                }
                SoundManager.instance.playSound(SoundManager.SOUNDS.DEFAULT_UI);
            }

            if (cloneButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                if (specieToClone != null)
                {
                    if (!cloner.isSeedOnFloor())
                    {
                        if (GameState.instance.addJob(JobType.CLONE, numberOfWorkers, 0))
                        {
                            specieToClone = GameState.instance.mySplices[selectedSplice];
                            cloner.Clone(specieToClone, numberOfWorkers);
                            SoundManager.instance.playSound(SoundManager.SOUNDS.CLONE);
                        }
                        else
                        {
                            //TODO: HUD ERROR
                        }
                        numberOfWorkers = 1;
                        cloner.updateNumSeeds(GameState.instance.mySplices[selectedSplice], numberOfWorkers);
                        updateNumberOfWorkersDisplay(true);
                    }
                    else
                    {
                        StartCoroutine(blinkingOutline(cloner.getPacketGO()));
                        SoundManager.instance.playSound(SoundManager.SOUNDS.ERROR_LAB);
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        StartCoroutine(blinkingOutline(cloneSplicesShelf.transform.GetChild(i).gameObject));
                        SoundManager.instance.playSound(SoundManager.SOUNDS.ERROR_LAB);
                    }
                }
                
            }
            if (findSelectedCustomSplice(true))
            {
                if (GameState.instance.mySplices[selectedSplice] != null)
                {
                    specieToClone = GameState.instance.mySplices[selectedSplice];
                    splicer.updateStatsDisplay(specieToClone, null, numberOfWorkers, true);
                    cloner.updateNumSeeds(GameState.instance.mySplices[selectedSplice], numberOfWorkers);
                }
                else
                {
                    specieToClone = null;
                }
                SoundManager.instance.playSound(SoundManager.SOUNDS.SELECT_SHELF_OBJ);
            }

        }
    }

    void updateNumberOfWorkersDisplay(bool cloneWorkers = false)
    {
        TextMesh textToUpdate = cloneWorkers ? numberOfCloneWorkersText : numberOfSpliceWorkersText;
        numberOfWorkers = numberOfWorkers > 3 ? 3 : numberOfWorkers;
        numberOfWorkers = numberOfWorkers < 1 ? 1 : numberOfWorkers;
        textToUpdate.text = ((int)numberOfWorkers / 10).ToString() + " " + ((int)numberOfWorkers % 10).ToString();
    }

    void handlePropertySelection()
    {
        for (int i = 0; i < leftPropertyScreen.transform.childCount; i++)
        {
            if (leftPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<MouseOverObj>().isMouseOver)
            {
                SoundManager.instance.playSound(SoundManager.SOUNDS.DEFAULT_UI); //TODO: not sure about this
                if (i < firstInsertedSpecie.resistantProperties.Count)
                {
                    //basically toggling. TODO: Add dynamic function for this and other property type(s)
                    if (chosenDisasterProperties.Contains(firstInsertedSpecie.resistantProperties[i]))
                    {
                        //  print("leftresistance removed");
                        leftPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonDown;
                        leftPropertyScreen.transform.GetChild(i).GetComponent<TextMesh>().color = Color.black;

                        chosenDisasterProperties.Remove(firstInsertedSpecie.resistantProperties[i]);
                    }
                    else
                    {
                        //    print("leftresistance added");
                        leftPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonDown;
                        leftPropertyScreen.transform.GetChild(i).GetComponent<TextMesh>().color = Color.green;
                        chosenDisasterProperties.Add(firstInsertedSpecie.resistantProperties[i]);
                    }
                }
                else
                {
                    //TODO: not in use, only used on non-resistant properties
                    int normalPropertyNr = i - firstInsertedSpecie.resistantProperties.Count;
                    if (chosenNormalProperties.Contains(firstInsertedSpecie.positiveProperties[normalPropertyNr]))
                    {
                        //    print("leftnormal removed");
                        leftPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonDown;
                        leftPropertyScreen.transform.GetChild(i).GetComponent<TextMesh>().color = Color.black;
                        chosenNormalProperties.Remove(firstInsertedSpecie.positiveProperties[normalPropertyNr]);
                    }
                    else
                    {
                        //    print("leftnormal added");
                        leftPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonDown;
                        leftPropertyScreen.transform.GetChild(i).GetComponent<TextMesh>().color = Color.green;
                        chosenNormalProperties.Add(firstInsertedSpecie.positiveProperties[normalPropertyNr]);
                    }
                }
                updateSyringe();
            }
            //ASSUMING BOTH PROPERTY SCREENS HAVE SAME NUMBER OF CHILDREN(which they should
            if (rightPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<MouseOverObj>().isMouseOver)
            {
                if (i < secondInsertedSpecie.resistantProperties.Count)
                {
                    //basically toggling. TODO: Add dynamic function for this and other property type(s)
                    if (chosenDisasterProperties.Contains(secondInsertedSpecie.resistantProperties[i]))
                    {
                        //   print("rightresistance removed");
                        rightPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonDown;
                        rightPropertyScreen.transform.GetChild(i).GetComponent<TextMesh>().color = Color.black;
                        chosenDisasterProperties.Remove(secondInsertedSpecie.resistantProperties[i]);
                    }
                    else
                    {
                        //  print("rightresistance added");
                        rightPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonDown;
                        rightPropertyScreen.transform.GetChild(i).GetComponent<TextMesh>().color = Color.green;
                        
                        chosenDisasterProperties.Add(secondInsertedSpecie.resistantProperties[i]);
                    }
                }
                else
                {
                    //TODO: not in use. only used when using non-resistant properties
                    int normalPropertyNr = i - secondInsertedSpecie.resistantProperties.Count;
                    if (chosenNormalProperties.Contains(secondInsertedSpecie.positiveProperties[normalPropertyNr]))
                    {
                        //  print("rightnormal removed");
                        rightPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonDown;
                        rightPropertyScreen.transform.GetChild(i).GetComponent<TextMesh>().color = Color.black;
                        chosenNormalProperties.Remove(secondInsertedSpecie.positiveProperties[normalPropertyNr]);
                    }
                    else
                    {
                        // print("rightNormal added");
                        rightPropertyScreen.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonDown;
                        rightPropertyScreen.transform.GetChild(i).GetComponent<TextMesh>().color = Color.green;
                        chosenNormalProperties.Add(secondInsertedSpecie.positiveProperties[normalPropertyNr]);
                    }
                }
                updateSyringe();
            }
        }
    }

    private void updateSyringe()
    {
        if (syringeCoroutine !=null)
        {
            StopCoroutine(syringeCoroutine); //TODO: test if this actually does what it is supposed to. probably does if you read this in a few weeks
        }
        syringeCoroutine = StartCoroutine(coroutineForSyringe());
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
                if (selectedSplice != -1)
                {
                    mySplicesShelf.transform.GetChild(selectedSplice).GetComponent<MouseOverObj>().lockedOutline = false;
                    mySplicesShelf.transform.GetChild(selectedSplice).GetComponent<MouseOverObj>().removeOutline();
                    selectedSplice = -1;
                }
                switchToMainLab();
                SoundManager.instance.playSound(SoundManager.SOUNDS.DEFAULT_UI);
            }

            if (addSpliceWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                numberOfWorkers++;
                updateNumberOfWorkersDisplay();
                updateSyringe();
                SoundManager.instance.playSound(SoundManager.SOUNDS.DEFAULT_UI);
            }

            if (removeSpliceWorkerButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                numberOfWorkers--;
                updateNumberOfWorkersDisplay();
                updateSyringe();
                SoundManager.instance.playSound(SoundManager.SOUNDS.DEFAULT_UI);
            }

            if (spliceButton.GetComponent<MouseOverObj>().isMouseOver)
            {
                if (firstInsertedSpecie != null)
                {
                    if (secondInsertedSpecie != null)
                    {
                        if (selectedSplice != -1)
                        {
                            if (GameState.instance.addJob(JobType.SPLICE, numberOfWorkers, 1))
                            {
                                Specie newSplice = splicer.spliceSpecies(firstInsertedSpecie, secondInsertedSpecie, numberOfWorkers, chosenDisasterProperties, chosenNormalProperties);
                                GameState.instance.mySplices[selectedSplice] = newSplice;
                                setSpliceGraphics(selectedSplice, newSplice.image);
                                numberOfWorkers = 1;
                                updateNumberOfWorkersDisplay();
                                SoundManager.instance.playSound(SoundManager.SOUNDS.SPLICE);
                            }
                            else
                            {
                                //TODO: add blinking på worker logo i hud ?
                            }
                        }
                        else
                        {
                            for(int i =0; i < 5; i++)
                            {
                                StartCoroutine(blinkingOutline(mySplicesShelf.transform.GetChild(i).gameObject));
                                SoundManager.instance.playSound(SoundManager.SOUNDS.ERROR_LAB);
                            }
                        }
                    }
                    else
                    {
                        StartCoroutine(blinkingOutline(spliceMachines[1]));
                        SoundManager.instance.playSound(SoundManager.SOUNDS.ERROR_LAB);
                    }
                }
                else
                {
                    StartCoroutine(blinkingOutline(spliceMachines[0]));
                    SoundManager.instance.playSound(SoundManager.SOUNDS.ERROR_LAB);
                }
            }

            handlePropertySelection();
            if (findSelectedCustomSplice())
            {
                SoundManager.instance.playSound(SoundManager.SOUNDS.SELECT_SHELF_OBJ);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Placing species in specie slots if mouse is over
            for (int i =0; i < 2; i++)
            {
                if (spliceMachines[i].GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
                {
                    chosenDisasterProperties.Clear();
                    chosenNormalProperties.Clear();
                    updateSyringe();
                    switch (i)
                    {
                        case 0:
                            firstInsertedSpecie = draggedObject.GetComponent<DragableUI>().getSpecie();
                            break;
                        case 1:
                            secondInsertedSpecie = draggedObject.GetComponent<DragableUI>().getSpecie();
                            break;
                        default:
                            break;
                    }
                    spliceMachines[i].transform.Find("spliceImage").GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<Image>().sprite;
                    SoundManager.instance.playSound(SoundManager.SOUNDS.FARM);//TODO: maybe have own sound, but think farm works
                    //splicer.updateStatsDisplay(firstInsertedSpecie, secondInsertedSpecie, numberOfWorkers); //redundant?
                }
            }
            //TODO: DELETE LATER. (this should not be necessary anymore, but keeping for test)
            //if (spliceMachines[0].GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
            //{
            //    chosenDisasterProperties.Clear();
            //    firstInsertedSpecie = draggedObject.GetComponent<DragableUI>().getSpecie();
            //    spliceMachines[0].transform.Find("spliceImage").GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<Image>().sprite;
            //    splicer.updateStatsDisplay(firstInsertedSpecie, secondInsertedSpecie, numberOfWorkers);
            //}

            //if (spliceMachines[1].GetComponent<MouseOverObj>().isMouseOver && draggedObject != null)
            //{
            //    chosenDisasterProperties.Clear();
            //    secondInsertedSpecie = draggedObject.GetComponent<DragableUI>().getSpecie();
            //    spliceMachines[1].transform.Find("spliceImage").GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<Image>().sprite;
            //}
            splicer.updateStatsDisplay(firstInsertedSpecie, secondInsertedSpecie, numberOfWorkers);
            resetDraggedObject();
        }
    }

    IEnumerator blinkingOutline(GameObject objectToBlink, float blinkDuration =1.25f)
    {
        MouseOverObj blinkScript = objectToBlink.GetComponent<MouseOverObj>();
        blinkScript.lockedOutline = true;
        float blinksPerSecond = 6;
        for (float f =0; f < blinkDuration;f+=Time.deltaTime)
        {
            if (((int)(f* blinksPerSecond)) % 2==0)
            {
                blinkScript.addOutline();
            }
            else
            {
                blinkScript.removeOutline();
            }
            yield return null;
        }
        if (!blinkScript.isMouseOver)
        {
            blinkScript.removeOutline();
        }
        blinkScript.lockedOutline = false;
    }

    IEnumerator coroutineForSyringe()
    {
        
        float fullyOutY = 4.76f;
        float fullyInY = 1.19f;
        float syringeUpdateTime = 1.0f;
        
        float availablePoints = (numberOfWorkers+1)*splicer.workerValue;//TODO: don't know why +1 is needed

        float usedPoints = chosenDisasterProperties.Count * splicer.disasterPropertyCost +
                           chosenNormalProperties.Count * splicer.postivePopertyCost;
        float startPos = syringeHandle.transform.localPosition.y;
        float fillPercent = Mathf.Max((availablePoints - usedPoints) /12, 0.35f); //should  be 0 and not 0.33. probably same reason as why 1 is needed above
        float newPos = fillPercent * (fullyOutY - fullyInY);

        for (float f =0; f < syringeUpdateTime; f +=Time.deltaTime)
        {
            float percentDone = f / syringeUpdateTime;
            Vector3 newSyringePos = syringeHandle.transform.localPosition;
            newSyringePos.y = startPos + (newPos - startPos) * percentDone;
            syringeHandle.transform.localPosition = newSyringePos;
            yield return null;
        }
        
    }

    private void setSpliceGraphics(int selectedSplice, Sprite sprite)
    {
        mySplicesShelf.transform.GetChild(selectedSplice).GetChild(1).GetComponent<SpriteRenderer>().sprite = sprite;
        cloneSplicesShelf.transform.GetChild(selectedSplice).GetChild(1).GetComponent<SpriteRenderer>().sprite = sprite;
    }

    //returns true if a splice is moused overed
    private bool findSelectedCustomSplice(bool forCloning = false)
    {
        GameObject shelfObject = forCloning ? cloneSplicesShelf : mySplicesShelf;
        int oldSelection = selectedSplice;
        for (int i = 0; i < shelfObject.transform.childCount; i++)
        {
            if (shelfObject.transform.GetChild(i).GetComponent<MouseOverObj>().isMouseOver)
            {
                selectedSplice = i;
                shelfObject.transform.GetChild(i).GetComponent<MouseOverObj>().lockedOutline = true;
                if (oldSelection != -1)
                {
                    shelfObject.transform.GetChild(oldSelection).GetComponent<MouseOverObj>().lockedOutline = false;
                    shelfObject.transform.GetChild(oldSelection).GetComponent<MouseOverObj>().removeOutline();
                }
                return true;
            }
        }
        return false;
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
        topLevelSpliceUI.SetActive(false);
        //cloneGO.GetComponent<BoxCollider2D>().enabled = true;
        //spliceGO.GetComponent<BoxCollider2D>().enabled = true;
        if (cameraRotationCoroutine != null)
        {
            StopCoroutine(cameraRotationCoroutine);
        }
        //cameraMovementCoroutine = StartCoroutine(moveCameraTowards(defaultCamPos));
        switch(currentView)
        {
            case LAB_VIEWS.SPLICING:
                cameraRotationCoroutine = StartCoroutine(rotateCamera(-90));
                break;
            case LAB_VIEWS.CLONING:
                cameraRotationCoroutine = StartCoroutine(rotateCamera(90));
                break;
            default:
                print("Error. Invalid switch parameter");
                break;
        }
        currentView = LAB_VIEWS.LOBBY;
    }

    void switchToClone()
    {
        //cloneGO.GetComponent<BoxCollider2D>().enabled = false;
        currentView = LAB_VIEWS.CLONING;

        if (cameraRotationCoroutine != null)
        {
            StopCoroutine(cameraRotationCoroutine);
        }
        cameraRotationCoroutine = StartCoroutine(rotateCamera(-90));
    }

    void switchToSplice()
    {
        currentView = LAB_VIEWS.SPLICING;
        //spliceGO.GetComponent<BoxCollider2D>().enabled = false;
        topLevelSpliceUI.SetActive(true);
        if (cameraRotationCoroutine !=null)
        {
            StopCoroutine(cameraRotationCoroutine);
        }

        cameraRotationCoroutine = StartCoroutine(rotateCamera(90));
    }

    IEnumerator moveCameraTowards(Vector3 target, float duration = 0.5f)
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

    IEnumerator rotateCamera(float degreeInY, float duration = 0.5f)
    {
        cameraRotationFinished = false;
        Vector3 startRot = Camera.main.transform.eulerAngles;
        
        for (float f = 0; f < duration; f += Time.deltaTime)
        {
            float pd = f / duration;
            pd *= pd;
            Camera.main.transform.eulerAngles = startRot + Vector3.up * degreeInY * pd;
            yield return null;
        }
        Camera.main.transform.eulerAngles = startRot + Vector3.up * degreeInY;
        cameraRotationFinished = true;
    }

}
