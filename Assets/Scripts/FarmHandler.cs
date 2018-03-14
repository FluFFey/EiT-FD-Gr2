using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmHandler : MonoBehaviour {

    public GameObject leaveFarmSign;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (leaveFarmSign.GetComponent<MouseOverObj>().isMouseOver)
        {
            //TODO: add visualization of  mouse over
            if (Input.GetMouseButtonDown(0))
            {
                SceneHandler.instance.changeScene(SceneHandler.SCENES.OVERWORLD);
            }
        }
	}

    //void OnMouseOver()
    //{
    //    if (leaveFarmSign.GetComponent<MouseOverObj>().isMouseOver)
    //    {

    //    }
    //    return;
    //}

}
