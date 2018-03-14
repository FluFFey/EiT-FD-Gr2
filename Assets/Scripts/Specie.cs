using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specie {

	public string name { get; set; }
	public int foodPoint { get; set; }
	public int durability { get; set; }
	public int growTime { get; set; }
	public int seedsPerWorker { get; set; }
	public List<DisasterProperty> resistantProperties { get; set;}
	public List<PositiveProperty> positiveProperties;
	public List<NegativeProperty> dormantNegativeProperties;
	public Sprite image { get; set;}
	public Rarity rarity { get; set;}
	public bool edible { get; set;}

	public Specie (string name, int foodPoint, int durability, int growTime, List<DisasterProperty> properties, List<PositiveProperty> positiveProps, List<NegativeProperty> negativeProps, Sprite image, int seedsPerWorker,int tag, Rarity rarity)
	{
		this.name = name;
		this.foodPoint = foodPoint;
		this.durability = durability;
		this.growTime = growTime;
		this.resistantProperties = properties;
		this.positiveProperties = positiveProps;
		this.dormantNegativeProperties = negativeProps;
		this.seedsPerWorker = seedsPerWorker;
		this.image = image;
		this.rarity = rarity;
		this.edible = false;
	}

	public Specie(){
	}

    public Specie(string name)
    {
        this.name = name;
    }
    //FIXME: TEMP FIX UNTIL BASESPECIES IS SET IN GAMESTATE.



    public enum Rarity {
		COMMON, RARE, EPIC, LEGENDARY
	}

	// PROPERTIES WHICH MAY(!) BE TRANSFERED TO NEW SPLICE
	public enum PositiveProperty { 
		//TODO: INSERTall properties
		EXTRA_FOOD_POINT, LESS_GROWTIME, EXTRA_DURABILITY, EXTRA_SEED_PER_WORKER
	}

	// NEGATIVE PROPERTIES WHICH MAY(!) BE TRANSFERED TO NEW SPLICE
	public enum NegativeProperty { 
		//TODO: POISONOUS? OTHER PROPS?
		REMOVES_RANDOM_RESISTANCE, LESS_FOOD_POINT, EXTRA_GROWTIME, LESS_DURABILITY, EXTRA_SEED_PER_WORKER
	}

}
