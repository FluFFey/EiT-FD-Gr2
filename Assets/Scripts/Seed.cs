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

	public Seed (string name, Specie specie)
	{
		this.name = name;
		this.specie = specie;
		daysGrown = 0;
        image = specie.image;
	}

    public SEED_STATE getSeedState()
    {
        if (daysGrown < specie.growTime)
        {
            return SEED_STATE.SEED;
        }
        if (daysGrown <specie.growTime+specie.durability)
        {
            return SEED_STATE.GROWN;
        }
        return SEED_STATE.GONE_BAD;
    }



}
