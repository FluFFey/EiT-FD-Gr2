using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey)
        {
            SceneHandler.instance.changeScene(SceneHandler.SCENES.MAIN_MENU);
        }
	}
}
