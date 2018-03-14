using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloning : MonoBehaviour{
    private Specie specie;
    private Seed seed;
    
    //INITIALIZE VARIABLES AND ADD TO GAMESTATE
    //Called on click (Clone!-button)
    public void Clone(Specie specie, int numWorkers)
    {
        this.specie = specie;
        seed = new Seed(specie.name + " seed", specie);
        GameState.instance.setSeeds(seed, CalculateNumSeeds(numWorkers));
    }

    //CALCULATE NUMBER OF SEEDS TO OUTPUT FROM CLONE JOB
    //TODO: Upgrade calclulation formula?
    public int CalculateNumSeeds(int workersAllocated){
        return specie.seedsPerWorker * workersAllocated;
    }   

}
