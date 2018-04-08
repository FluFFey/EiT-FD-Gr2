using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseOverObj))]
public class PulseOnMouseOver : MonoBehaviour
{

    private Vector3 originalScale;
    // Use this for initialization
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = originalScale;
        if (GetComponent<MouseOverObj>().isMouseOver)
        {
            Vector3 newScale = originalScale + Vector3.one*Mathf.Sin(Time.time * 5)*0.15f;
            //Vector3  = new Vector3(originalRot.x, 0, newScale);
            transform.localScale = newScale;
        }

    }
}
