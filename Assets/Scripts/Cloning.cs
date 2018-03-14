using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloning : MonoBehaviour{
    
    //INITIALIZE VARIABLES AND ADD TO GAMESTATE
    //To be called on click (Clone!-button)
    public void Clone(Specie specie, int numWorkers)
    {
        Seed seed = new Seed(specie.name + " seed", specie);
        GameState.instance.setSeeds(seed, CalculateNumSeeds(specie, numWorkers));
    }

    //CALCULATE NUMBER OF SEEDS TO OUTPUT FROM CLONE JOB
    //TODO: Upgrade calclulation formula?
    public int CalculateNumSeeds(Specie specie, int workersAllocated){
        return specie.seedsPerWorker * workersAllocated;
    }   

}
