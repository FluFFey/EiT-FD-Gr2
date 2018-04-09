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
    Dictionary<string, Vector3> positionsForPlants;
    Dictionary<string, Vector3> rotationForPlants;
    Dictionary<string, Vector3> scaleForPlants;
    private bool feasting = false; //true during day/night transition
    public TextMesh cropInfoBoardTextMesh;
    // Use this for initialization

    void Start ()
    {

        positionsForPlants = new Dictionary<string, Vector3>();
        positionsForPlants.Add("bee seed", new Vector3(-0.03f,0.92f,-0.86f));
        positionsForPlants.Add("boole seed", new Vector3(0.0f, 1.45f, 0.0f));
        positionsForPlants.Add("mole seed", new Vector3(-0.06f, 0.27f, -0.72f));
        positionsForPlants.Add("ceern seed", new Vector3(-0.24f, 1.35f, 0));
        positionsForPlants.Add("cole seed", new Vector3(-0.15f, 0.59f, 0));
        positionsForPlants.Add("corn seed", new Vector3(-0.21f, 0.73f, -1.56f));
        positionsForPlants.Add("booze seed", new Vector3(0.21f, 0.77f, 0));
        //positionsForPlants.Add("mole", Vector3.zero); //name overlap. bad idea in hindsight. this should be for moose/mole (two first/two last letters)
        positionsForPlants.Add("moorn seed", new Vector3(0.0f, 1.45f, 0.0f));
        positionsForPlants.Add("moose seed", new Vector3(0.61f, 0.35f, -1.65f));
        positionsForPlants.Add("beeferfish seed", new Vector3(0.21f, 0.69f, 0.0f));
        positionsForPlants.Add("puffermole seed", new Vector3(-0.08f, 0.34f, 0.0f));
        positionsForPlants.Add("cornerfish seed", new Vector3(0.3f, 0.36f, 0.0f));
        positionsForPlants.Add("moosefish seed", new Vector3(0.28f, 0.59f, 0.0f));
        positionsForPlants.Add("pufferfish seed", new Vector3(-0.15f, 0.52f, 0.0f));
        positionsForPlants.Add("seed", Vector3.zero);
        positionsForPlants.Add("rot", new Vector3(-0.11f,0.92f,-1.42f));

        rotationForPlants = new Dictionary<string, Vector3>();
        rotationForPlants.Add("bee seed", new Vector3(-45, 0, 0));
        rotationForPlants.Add("boole seed", new Vector3(-45, 0, 0));
        rotationForPlants.Add("mole seed", new Vector3(-45, 0, 0));
        rotationForPlants.Add("ceern seed", new Vector3(-33, 0, 0));
        rotationForPlants.Add("cole seed", new Vector3(-45, 0, 0));
        rotationForPlants.Add("corn seed", new Vector3(-66, 0, 0));
        rotationForPlants.Add("booze seed", new Vector3(-33, 0, 0));
        rotationForPlants.Add("moorn seed", new Vector3(-33, 0, 0));
        rotationForPlants.Add("moose seed", new Vector3(-66, 0, 0));
        rotationForPlants.Add("beeferfish seed", new Vector3(-33, 0, 0));
        rotationForPlants.Add("puffermole seed", new Vector3(-33, 0, 0));
        rotationForPlants.Add("cornerfish seed", new Vector3(-33, 0, 0));
        rotationForPlants.Add("moosefish seed", new Vector3(-33, 0, 0));
        rotationForPlants.Add("pufferfish seed", new Vector3(-45, 0, 0));
        rotationForPlants.Add("seed", Vector3.zero);
        rotationForPlants.Add("rot", new Vector3(-45, 0, 0));

        scaleForPlants = new Dictionary<string, Vector3>();
        scaleForPlants.Add("bee seed", Vector3.one*0.9f);
        scaleForPlants.Add("boole seed", Vector3.one);
        scaleForPlants.Add("mole seed", Vector3.one*0.9f);
        scaleForPlants.Add("ceern seed", Vector3.one);
        scaleForPlants.Add("cole seed", Vector3.one);
        scaleForPlants.Add("corn seed", Vector3.one*0.75f);
        scaleForPlants.Add("booze seed", Vector3.one);
        scaleForPlants.Add("moorn seed", Vector3.one);
        scaleForPlants.Add("moose seed", Vector3.one*0.75f);
        scaleForPlants.Add("beeferfish seed", Vector3.one);
        scaleForPlants.Add("puffermole seed", Vector3.one);
        scaleForPlants.Add("cornerfish seed", Vector3.one);
        scaleForPlants.Add("moosefish seed", Vector3.one);
        scaleForPlants.Add("pufferfish seed", Vector3.one);
        scaleForPlants.Add("seed", Vector3.one);
        scaleForPlants.Add("rot", Vector3.one*0.75f);

        Dictionary<int, Seed> seeds = GameState.instance.getPlantedSeeds();
        foreach (int seedIndex in seeds.Keys)
        {
            updateSeed(seeds[seedIndex], seedIndex);
            //Seed.SEED_STATE state = seeds[seedIndex].getSeedState();

            //switch(state)
            //{
            //    case Seed.SEED_STATE.SEED:
            //        transform.GetChild(seedIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = seedSprite;
            //        transform.GetChild(seedIndex).GetChild(0).localPosition = positionsForPlants["seed"];
            //        transform.GetChild(seedIndex).GetChild(0).localEulerAngles = rotationForPlants["seed"];
            //        transform.GetChild(seedIndex).GetChild(0).localScale = scaleForPlants["seed"];
            //        break;
            //    case Seed.SEED_STATE.GROWN:
            //        transform.GetChild(seedIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = getPlantedSprite(seeds[seedIndex]);
            //        transform.GetChild(seedIndex).GetChild(0).localPosition = positionsForPlants[seeds[seedIndex].name];
            //        transform.GetChild(seedIndex).GetChild(0).localEulerAngles = rotationForPlants[seeds[seedIndex].name];
            //        transform.GetChild(seedIndex).GetChild(0).localScale = scaleForPlants[seeds[seedIndex].name];
            //        break;
            //    case Seed.SEED_STATE.GONE_BAD:
            //        transform.GetChild(seedIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = goneBadSprite;
            //        transform.GetChild(seedIndex).GetChild(0).localPosition = positionsForPlants["rot"];
            //        transform.GetChild(seedIndex).GetChild(0).localEulerAngles = rotationForPlants["rot"];
            //        transform.GetChild(seedIndex).GetChild(0).localScale = scaleForPlants["rot"];
            //        break;
            //}            
        }
    }

    void updateSeed(Seed seedToUpdate, int seedIndex)
    {
        Seed.SEED_STATE state = seedToUpdate.getSeedState();
        switch (state)
        {
            case Seed.SEED_STATE.SEED:
                transform.GetChild(seedIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = seedSprite;
                transform.GetChild(seedIndex).GetChild(0).localPosition = positionsForPlants["seed"];
                transform.GetChild(seedIndex).GetChild(0).localEulerAngles = rotationForPlants["seed"];
                transform.GetChild(seedIndex).GetChild(0).localScale = scaleForPlants["seed"];
                break;
            case Seed.SEED_STATE.GROWN:
                transform.GetChild(seedIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = getPlantedSprite(seedToUpdate);
                transform.GetChild(seedIndex).GetChild(0).localPosition = positionsForPlants[seedToUpdate.name];
                transform.GetChild(seedIndex).GetChild(0).localEulerAngles = rotationForPlants[seedToUpdate.name];
                transform.GetChild(seedIndex).GetChild(0).localScale = scaleForPlants[seedToUpdate.name];
                break;
            case Seed.SEED_STATE.GONE_BAD:
                transform.GetChild(seedIndex).GetChild(0).GetComponent<SpriteRenderer>().sprite = goneBadSprite;
                transform.GetChild(seedIndex).GetChild(0).localPosition = positionsForPlants["rot"];
                transform.GetChild(seedIndex).GetChild(0).localEulerAngles = rotationForPlants["rot"];
                transform.GetChild(seedIndex).GetChild(0).localScale = scaleForPlants["rot"];
                break;
        }
    }


    IEnumerator endDay()
    {
        feasting = true;
        //float nightTransitionTime = 3.0f;
        SoundManager.instance.playSound(SoundManager.SOUNDS.FEAST);
        StartCoroutine(CameraScript.instance.fade(false, 2, 0.8f));
        yield return new WaitForSeconds(2.25f);
        StartCoroutine( MusicManager.instance.silenceMusic());
        yield return new WaitForSeconds(1.25f);
        SoundCaller sc = SoundManager.instance.getSoundCaller();
        AudioSource nightSoundSource = sc.findFreeAudioSource();
        float originalVolume = nightSoundSource.volume;
        nightSoundSource.clip = SoundManager.instance.normalNightSound;
        nightSoundSource.Play();

        GameState.instance.pressNextDay();

        yield return new WaitForSeconds(1.0f);
        float extraSecondsWaited = 0;
        Dictionary<int, Seed> seeds = GameState.instance.getPlantedSeeds();
        List<int> seedPositions = new List<int>();
        foreach(int seedPos in seeds.Keys)
        {
            seedPositions.Add(seedPos);
        }
        while(seedPositions.Count >0)
        {
            int seedToPop = seedPositions[UnityEngine.Random.Range(0, seedPositions.Count)];
            if(seeds[seedToPop].daysGrown == seeds[seedToPop].specie.growTime)
            {
                SoundManager.instance.playSound(SoundManager.SOUNDS.FARM);
            }
            updateSeed(seeds[seedToPop], seedToPop);
            float delay = UnityEngine.Random.Range(0.2f, 0.95f);
            yield return new WaitForSeconds(delay);
            extraSecondsWaited += delay;
            seedPositions.Remove(seedToPop);
        }


        yield return new WaitForSeconds(3.25f-extraSecondsWaited);
        nightSoundSource.Stop();
        yield return new WaitForSeconds(2.25f);
        StartCoroutine(MusicManager.instance.setVolume(0.1f));
        StartCoroutine(CameraScript.instance.fade(true, 2.5f));
        yield return new WaitForSeconds(2.0f);
        feasting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (feasting)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (leaveFarmSign.GetComponent<MouseOverObj>().isMouseOver)
            {
                SceneHandler.instance.changeScene(SceneHandler.SCENES.OVERWORLD);
                SoundManager.instance.playSound(SoundManager.SOUNDS.SELECT_SHELF_OBJ);
            }

            if (FeastSign.GetComponent<MouseOverObj>().isMouseOver)
            {
                StartCoroutine(endDay());
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
                            SoundManager.instance.playSound(SoundManager.SOUNDS.FARM);
                        }
                    }
                    //player trying to harvest here
                    if (draggedObject == null && plantedSeeds.ContainsKey(i))
                    {
                        Seed.SEED_STATE state = plantedSeeds[i].getSeedState();
                        switch (state)
                        {
                            case Seed.SEED_STATE.SEED:
                                break;
                            case Seed.SEED_STATE.GROWN:
                                GameState.instance.eatFood(i);
                                removeSeed(i);
                                SoundManager.instance.playSound(SoundManager.SOUNDS.HARVEST);
                                break;
                            case Seed.SEED_STATE.GONE_BAD:
                                removeSeed(i);
                                plantedSeeds.Remove(i);
                                SoundManager.instance.playSound(SoundManager.SOUNDS.HARVEST_ROTTEN);
                                break;
                            default:
                                break;
                        }                        
                        
                    }
                }
            }
            draggedObject = null;
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
        transform.GetChild(fieldIndex).GetChild(0).localPosition = Vector3.zero;
        transform.GetChild(fieldIndex).GetChild(0).eulerAngles = Vector3.zero;
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
