using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FarmHandler : MonoBehaviour {

    public GameObject leaveFarmSign;
    public GameObject FeastSign;
    public Sprite seedSprite;
    public Sprite goneBadSprite;
    private GameObject draggedObject;
    private const int fieldSize = 25;
	// Use this for initialization
	void Start ()
    {
        Dictionary<int, Seed> seeds = GameState.instance.getPlantedSeeds();
        foreach (int seedIndex in seeds.Keys)
        {
            transform.GetChild(seedIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = getPlantedSprite(seeds[seedIndex]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (leaveFarmSign.GetComponent<MouseOverObj>().isMouseOver)
            {
                SceneHandler.instance.changeScene(SceneHandler.SCENES.OVERWORLD);
            }

            if (FeastSign.GetComponent<MouseOverObj>().isMouseOver)
            {
                SceneHandler.instance.changeScene(SceneHandler.SCENES.OVERWORLD);
                GameState.instance.pressNextDay();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Dictionary<int, Seed> plantedSeeds = GameState.instance.getPlantedSeeds();

            for (int i = 0; i < fieldSize; i++)
            {
                if (transform.GetChild(i).GetComponent<MouseOverObj>().isMouseOver)
                {
                    //player trying to plant here
                    if(draggedObject != null)
                    {
                        if (GameState.instance.emptyFarmSlot(i))
                        {
                            plantSeed(i);
                        }
                    }
                    //player trying to harvest here
                    if (draggedObject == null)
                    {
                        GameState.instance.eatFood(i);
                        removeSeed(i);
                    }
                }
            }
        }
    }

    //mostly graphical
    private void removeSeed(int fieldIndex)
    {
        transform.GetChild(fieldIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    }

    //mostly graphical
    private void plantSeed(int fieldIndex)
    {
        Seed seedToPlant = draggedObject.GetComponent<DragableSprite>().getSeed();
        int remainingSeeds = int.Parse(draggedObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text);
        remainingSeeds--;
        if (remainingSeeds==0)
        {
            Destroy(draggedObject,0.01f);
        }
        else
        {
            draggedObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = remainingSeeds.ToString();
        }
        GameState.instance.plantSeed(seedToPlant, fieldIndex);
        transform.GetChild(fieldIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = getPlantedSprite(seedToPlant);
    }

    private Sprite getPlantedSprite(Seed seed)
    {
        switch (seed.getSeedState())
        {
            case Seed.SEED_STATE.SEED:
                return seedSprite;
            case Seed.SEED_STATE.GROWN:
                return seed.image;
            case Seed.SEED_STATE.GONE_BAD:
                return goneBadSprite;
            default:
                print("INVALID SEED STATE! RETURNING GONE BAD SPRITE");
                return goneBadSprite;
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
}
