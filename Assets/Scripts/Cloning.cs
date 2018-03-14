using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloning : MonoBehaviour{
    private int workersAllocated;
    private Specie specie;

    //Initialize variables and GameState
    private void Awake(Specie specie, int numWorkers)
    {
        this.specie = specie;
        workersAllocated = setWorkersAllocated(numWorkers);
    }

    private int setWorkersAllocated(int numWorkers)
    {
        //TODO: Implement method
        throw new NotImplementedException();
    }

    //Calculate number of seeds from clone job
    int CalculateNumSeeds(){
        //Calculation based on people allocated and seeds per person.
        return 1;
    }

    //Return key, value in seeds Dictionary in GameState.
    void InitializeJob(int numWorkers, int id){
        //True if Job successfully instantiated
        bool b = GameState.instance.addJob(JobType.CLONE, numWorkers, id);
        if (b) { CalculateNumSeeds(); }
    }
    

}
