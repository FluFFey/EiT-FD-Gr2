using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabHandler : MonoBehaviour
{

    public GameObject leaveLabDoor;
    // Use this for initialization
    void Start()
    {

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
    }
}
