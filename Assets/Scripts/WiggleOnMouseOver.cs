using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseOverObj))]
public class WiggleOnMouseOver : MonoBehaviour {

    private Vector3 originalRot;
	// Use this for initialization
	void Start () {
        originalRot = transform.eulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
        transform.parent.eulerAngles = originalRot;
        if (GetComponent<MouseOverObj>().isMouseOver)
        {
            float newZRot = Mathf.Sin(Time.time*5)*2;
            Vector3 newRot = new Vector3(originalRot.x, 0, newZRot );
            transform.parent.eulerAngles = newRot;
        }
        
    }
}
