using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseOverObj))]
public class DragableSprite : MonoBehaviour {

    GameObject dragObj;
    private Seed seed;
    private FarmHandler farmHandlerScript;
    public GameObject objectToImitate;
    // Use this for initialization
    void Start ()
    {
        farmHandlerScript = GameObject.Find("FarmHandler").GetComponent<FarmHandler>();
    }

    public void init(GameObject originalGO)
    {
        objectToImitate = originalGO;
    }

	// Update is called once per frame
	void Update ()
    {
		if (Input.GetMouseButtonDown(0) && GetComponent<MouseOverObj>().isMouseOver)
        {
            farmHandlerScript.setDraggedObject(gameObject);
            dragObj = Instantiate(objectToImitate,transform.parent);
        }
        if (Input.GetMouseButton(0) && dragObj != null)
        {
            Vector3 input = Input.mousePosition;
            input.z = 100;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(input);
            newPos.z = 90;
            dragObj.transform.position = newPos;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Destroy(dragObj);
        }
        
    }

    internal void setSeed(Seed newSeed)
    {
        seed = newSeed;
    }

    public Seed getSeed()
    {
        return seed;
    }
}
