using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed {

	public string name { get; }
	public Specie specie { get; }
	public int daysGrown;
	public Sprite image { get; }

    public enum SEED_STATE
    {
        SEED,
        GROWN,
        GONE_BAD
    }

    //another hacky solution for reasons..
    public Seed(Seed seed)
    {
        name = seed.name;
        specie = seed.specie;
        daysGrown = seed.daysGrown; //hopefully no consequences
        image = seed.image;
    }

	public Seed (string name, Specie specie)
	{
		this.name = name;
		this.specie = specie;
		daysGrown = 0;
        image = specie.image;
	}

    public SEED_STATE getSeedState()
    {
        Debug.Log(daysGrown);
        if (daysGrown < specie.growTime)
        {
            Debug.Log("a");
            return SEED_STATE.SEED;
        }
        if (daysGrown <specie.growTime+specie.durability)
        {
            Debug.Log("b");
            return SEED_STATE.GROWN;
        }
        Debug.Log("c");
        return SEED_STATE.GONE_BAD;
    }



}
