using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed {

	public string name { get; }
	public Specie specie { get; }
	public int daysGrown;
	public Sprite image { get; }

	public Seed (string name, Specie specie)
	{
		this.name = name;
		this.specie = specie;
		daysGrown = 0;
		//TODO: SET IMAGE
	}




}
