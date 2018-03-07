using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specie {

	public string name { get; set;}
	public int foodPoint { get; set;}
	public int durability { get; set; }
	public int growTime { get; set; }
	public int seedsPerWorker { get; set;}
	public List<DisasterProperty> resistantProperties { get; set;}
	//TODO: ADD NEGATIVE PROPS? 
	public Sprite image { get; set;}
	public Rarity rarity { get; set;}
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
