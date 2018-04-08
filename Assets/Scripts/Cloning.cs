using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloning : MonoBehaviour{
    
    //INITIALIZE VARIABLES AND ADD TO GAMESTATE
    //To be called on click (Clone!-button)
    public TextMesh seedText;
    public GameObject seedPrefab;
    
    public AudioClip cloneSound;
    private SoundCaller sc;
    private GameObject seedPacket;
    private Specie specieInPacket;
    int numberOfSeedsInPacket;
    void Awake()
    {
        sc = GetComponent<SoundCaller>();
    }
    public void Clone(Specie specie, int numWorkers)
    {
        //Seed seed = new Seed(specie.name + " seed", specie);
        StartCoroutine(createSeeds(specie));
        numberOfSeedsInPacket = specie.seedsPerWorker * numWorkers;
        //On pickup:
        //GameState.instance.setSeeds(seed, updateNumSeeds(specie, numWorkers)); 
    }

    public bool isSeedOnFloor()
    {
        return seedPacket != null;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (seedPacket !=null && seedPacket.GetComponent<MouseOverObj>().isMouseOver)
            {
                //play sound
                Seed seed = new Seed(specieInPacket.name + " seed", specieInPacket);
                GameState.instance.setSeeds(seed, numberOfSeedsInPacket);
                Destroy(seedPacket);
            }
        }
    }

    //CALCULATE NUMBER OF SEEDS TO OUTPUT FROM CLONE JOB
    //TODO: Upgrade calclulation formula?
    public int updateNumSeeds(Specie specie, int workersAllocated){
        int seedsCreated = specie.seedsPerWorker * workersAllocated;
        if (seedsCreated < 10)
        {
            seedText.text = "0" + seedsCreated.ToString();
        }
        else
        {
            seedText.text = seedsCreated.ToString();
        }
        return seedsCreated;
    }   

    IEnumerator createSeeds(Specie spliceToClone, float dropTime =0.5f)
    {
        //sc.attemptSound(cloneSound);
        seedPacket = Instantiate(seedPrefab, transform);
        specieInPacket = spliceToClone;
        seedPacket.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spliceToClone.image;
        Vector3 startPos = new Vector3(6.7f, -0.8f, 0);
        float goalY = -3.06f;
        float dropdist = goalY + 0.8f;
        seedPacket.transform.localPosition = startPos;
        yield return new WaitForSeconds(1.0f);
        for(float f=0; f < dropTime; f+=Time.deltaTime)
        {
            float pd = f / dropTime;
            pd *= pd;
            if (seedPacket!=null)
            {
                seedPacket.transform.localPosition = startPos + Vector3.up * dropdist * pd;
                yield return null;
            }
        }
        if (seedPacket != null)
        {
            seedPacket.transform.localPosition = new Vector3(6.7f, goalY, 0);
        }
    }

    internal void resetSeedDisplay()
    {
        seedText.text = "";
    }

    internal GameObject getPacketGO()
    {
        return seedPacket;
    }
}
