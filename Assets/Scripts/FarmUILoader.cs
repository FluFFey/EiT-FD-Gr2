using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FarmUILoader : MonoBehaviour {

    public GameObject seedPacket;
	// Use this for initialization
	void Awake ()
    {
        int counter = 0;
		foreach(Seed seed in GameState.instance.getInventorySeeds().Keys)
        {
            GameObject newPacket = Instantiate(seedPacket,transform);
            newPacket.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = seed.specie.image;

            //newPacket.transform.localPosition = new Vector3(175+(counter*175), 50, 0);
            newPacket.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(175 + (counter * 175), 0);
            //print(newPacket.transform.localPosition.x);
            newPacket.GetComponent<DragableSprite>().setSeed(seed);
            newPacket.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            newPacket.GetComponent<DragableSprite>().init(newPacket);
            newPacket.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            newPacket.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = GameState.instance.getInventorySeeds()[seed].ToString();
            counter++;
        }
	}

	// Update is called once per frame
	void Update () {
		
	}
}
