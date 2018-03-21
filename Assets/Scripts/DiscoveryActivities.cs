using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryActivities : MonoBehaviour {
    List<Specie> knownSpecies, allSpecies, discoverableSpecies;
    static System.Random rnd = new System.Random();

    //initialization
    void Awake(){
        this.allSpecies = GameState.instance.GetAllSpecies();
        this.knownSpecies = GameState.instance.GetKnownSpecies();
    }

    public void Hunt(int numWorkers){
        discoverableSpecies = FilterOnType(Specie.Type.FOREST);
        Specie discoveredSpecie = DiscoverSpecie(numWorkers);

        if (knownSpecies.Contains(discoveredSpecie)){
            //Add to food points consumed for that day
            GameState.instance.addFoodPointsConsumed(discoveredSpecie.foodPoint);
        }
        else{
            //Add known specie
            GameState.instance.AddKnownSpecies(discoveredSpecie);
        }
    }

    public void Fish(int numWorkers){
        discoverableSpecies = FilterOnType(Specie.Type.WATER);
        Specie discoveredSpecie = DiscoverSpecie(numWorkers);

        if (knownSpecies.Contains(discoveredSpecie))
        {
            //Add food points to food points consumed for that day
            GameState.instance.addFoodPointsConsumed(discoveredSpecie.foodPoint);
        }
        else
        {
            //Add to known specie
            GameState.instance.AddKnownSpecies(discoveredSpecie);
        }
    }

    public Specie DiscoverSpecie(int numWorkers){
        //Filter list based on numWorkers
        //TODO: TUNE RELATIONSHIP BETWEEN numWorkers and RARITY
        //TODO: Consider making the if conditions dynamic with respect to numWorkers.
        if (numWorkers <= 1){
            discoverableSpecies = FilterOnRarity(Specie.Rarity.COMMON);
        }else if(numWorkers <= 2){
            discoverableSpecies = FilterOnRarity(Specie.Rarity.RARE);
        }
        else if (numWorkers <= 3){
            discoverableSpecies = FilterOnRarity(Specie.Rarity.EPIC);
        }
        else if (numWorkers > 3){
            discoverableSpecies = FilterOnRarity(Specie.Rarity.LEGENDARY);
        }
        //Return a random index from the filtered list
        int randomIndex = rnd.Next(discoverableSpecies.Count);
        return discoverableSpecies[randomIndex];
    }

    //RETURNS A LIST OF SPECIES FILTERED ON THE DESIRED TYPE
    public List<Specie> FilterOnType(Specie.Type type){
        List<Specie> filteredSpecies = new List<Specie>();
        discoverableSpecies.ForEach(specie =>
        {
            if(specie.type == type) { filteredSpecies.Add(specie); }
        });
        return filteredSpecies;
    }

    //RETURNS A LIST OF SPECIES FILTERED ON THE DESIRED RARITY
    public List<Specie> FilterOnRarity(Specie.Rarity rarity){
        List<Specie> filteredSpecies = new List<Specie>();
        switch (rarity){
            case Specie.Rarity.COMMON:
                discoverableSpecies.ForEach(specie =>
                {
                    if (specie.rarity == Specie.Rarity.COMMON){
                        filteredSpecies.Add(specie);
                    }
                });
                break;
            case Specie.Rarity.RARE:
                discoverableSpecies.ForEach(specie =>
                {
                    if (specie.rarity == Specie.Rarity.COMMON || specie.rarity == Specie.Rarity.RARE){
                        filteredSpecies.Add(specie);
                    }
                });
                break;
            case Specie.Rarity.EPIC:
                discoverableSpecies.ForEach(specie =>
                {
                    if (specie.rarity == Specie.Rarity.COMMON || specie.rarity == Specie.Rarity.RARE || specie.rarity == Specie.Rarity.EPIC){
                        filteredSpecies.Add(specie);
                    }
                });
                break;
            case Specie.Rarity.LEGENDARY:
                discoverableSpecies.ForEach(specie =>
                {
                    if (specie.rarity == Specie.Rarity.RARE || specie.rarity == Specie.Rarity.EPIC || specie.rarity == Specie.Rarity.LEGENDARY){
                        filteredSpecies.Add(specie);
                    }
                });
                break;
        }        
        return filteredSpecies;
    }
}
