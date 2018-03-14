using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryActivities : MonoBehaviour {
    List<Specie> knownSpecies, allSpecies, discoverableSpecies;
    static System.Random rnd = new System.Random();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update(){
        this.allSpecies = GameState.instance.GetAllSpecies();
        this.knownSpecies = GameState.instance.GetKnownSpecies();
    }

    public void Hunt(int numWorkers){
        discoverableSpecies = FilterOnType(Specie.Type.FOREST);
        Specie discoveredSpecie = DiscoverSpecie(numWorkers);

        if (knownSpecies.Contains(discoveredSpecie)){
            //TODO: Subtract discoveredSpecies.foodpoints form the total need of food for that day.
        }

    }

    public void Fish(int numWorkers){
        discoverableSpecies = FilterOnType(Specie.Type.WATER);
        Specie discoveredSpecie = DiscoverSpecie(numWorkers);

        if (knownSpecies.Contains(discoveredSpecie))
        {
            //TODO: Subtract discoveredSpecies.foodpoints form the total need of food for that day.
        }
    }

    public Specie DiscoverSpecie(int numWorkers){
        //Filter list based on numWorkers
        //TODO: TUNE RELATIONSHIP BETWEEN numWorkers and RARITY
        //TODO: SPECIAL CASES COULD TRIGGER LEGENDARY UNINTENDEDLY
        if(numWorkers <= 2){
            discoverableSpecies = FilterOnRarity(Specie.Rarity.COMMON);
        }else if(numWorkers <= 4){
            discoverableSpecies = FilterOnRarity(Specie.Rarity.RARE);
        }
        else if (numWorkers <= 5){
            discoverableSpecies = FilterOnRarity(Specie.Rarity.EPIC);
        }
        else{
            discoverableSpecies = FilterOnRarity(Specie.Rarity.LEGENDARY);
        }
        //Return a random index from the filtered list
        int randomIndex = rnd.Next(discoverableSpecies.Count);
        return discoverableSpecies[randomIndex];
    }

    //RETURNS A LIST OF SPECIES FILTERED ON THE DESIRED TYPE
    public List<Specie> FilterOnType(Specie.Type type){
        List<Specie> filteredSpecies = new List<Specie>();
        foreach (Specie specie in discoverableSpecies)
        {
            filteredSpecies.Add(specie);
        }
        return filteredSpecies;
    }

    //RETURNS A LIST OF SPECIES FILTERED ON THE DESIRED RARITY
    public List<Specie> FilterOnRarity(Specie.Rarity rarity){
        List<Specie> filteredSpecies = new List<Specie>();
        foreach (Specie specie in discoverableSpecies)
        {
            filteredSpecies.Add(specie);
        }
        return filteredSpecies;
    }
}
