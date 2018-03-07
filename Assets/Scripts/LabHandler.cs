using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabHandler : MonoBehaviour
{

    public GameObject leaveLabDoor;
    public GameObject spliceGO;
    public GameObject cloneGO;

    public GameObject spliceSignGO;
    public GameObject cloneSignGO;

    Coroutine cameraMovementCoroutine;
    // Use this for initialization
    void Awake()
    {
        cameraMovementCoroutine = null;
    }

    // Update is called once per frame
    void Update()
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
                switchToClone();
            }
        }
        if (spliceGO.GetComponent<MouseOverObj>().isMouseOver)
        {
            //TODO: add visualization of  mouse over
            if (Input.GetMouseButtonDown(0))
            {
                switchToSplice();
            }
        }

        if (cloneSignGO.GetComponent<MouseOverObj>().isMouseOver  )
        {
            //TODO: add visualization of  mouse over
            if (Input.GetMouseButtonDown(0))
            {
                switchToMainLab();
            }
        }

        if (spliceSignGO.GetComponent<MouseOverObj>().isMouseOver)
        {
            //TODO: add visualization of  mouse over
            if (Input.GetMouseButtonDown(0))
            {
                switchToMainLab();
            }
        }
    }

    void switchToMainLab()
    {
        if (cameraMovementCoroutine != null)
        {
            StopCoroutine(cameraMovementCoroutine);
        }
        cameraMovementCoroutine = StartCoroutine(moveCameraTowards(Vector3.zero - Vector3.forward * 5, 2.0f));
    }

    void switchToClone()
    {
        if (cameraMovementCoroutine != null)
        {
            StopCoroutine(cameraMovementCoroutine);
        }
        cameraMovementCoroutine = StartCoroutine(moveCameraTowards(cloneGO.transform.position - Vector3.forward * 4,2.0f));
    }

    void switchToSplice()
    {
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

        for (float f =0; f < duration; f+=Time.deltaTime)
        {
            float pd = f / duration;
            pd *= pd;
            Camera.main.transform.position = startPos + moveVector * pd;
            yield return null;
        }
    }
}
