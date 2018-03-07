using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specie {

	public string name { get; }
	public int foodPoint { get; }
	public int durability { get; }
	public int growTime { get; }
	public int seedsPerWorker { get;}
	public List<DisasterProperty> resistantProperties { get; set;}
	//TODO: ADD NEGATIVE PROPS? 
	public Sprite image { get; }
	public Rarity rarity { get; }
	public bool edible { get; set;}

	public Specie (string name, int foodPoint, int durability, int growTime, List<DisasterProperty> properties, Sprite image, int seedsPerWorker,int tag, Rarity rarity)
	{
		this.name = name;
		this.foodPoint = foodPoint;
		this.durability = durability;
		this.growTime = growTime;
		this.resistantProperties = properties;
		this.seedsPerWorker = seedsPerWorker;
		this.image = image;
		this.rarity = rarity;
		this.edible = false;
	}

	public Specie(){
	}

	//FIXME: TEMP FIX UNTIL BASESPECIES IS SET IN GAMESTATE.



	public enum Rarity {
		COMMON, RARE, EPIC, LEGENDARY
	}

}
