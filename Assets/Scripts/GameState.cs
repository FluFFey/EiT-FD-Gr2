﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


using System;


public class GameState : MonoBehaviour {

	public static GameState instance;

	//TODO: UPDATE BASED ON AUDUNS DRAWING
	public const int IMAGE_WIDTH = 5;
	public const int DISASTER_INTERVAL = 4;
	public const int TOTAL_DAYS = 20;
	public const int FOOD_POINT_PER_WORKER = 2;
    
	//HUD-ATTRIBUTES
	int survivors; 
	List<Job> currentJobs;
    Dictionary<Seed, int> inventorySeeds;
    public List<Specie> knownSpecies;
    List<Specie> unknownSpecies;
    public List<Specie> allSpecies;
	public List<Specie> mySplices { get; set; }
    int foodPointsConsumed;

    //CALENDAR-ATTRIBUTES
    int totalDays;
	int daysPassed;
	Dictionary<int,  NaturalDisaster> naturalDisasters;

    //BASE-SPECIES
    //TODO: ADD VALUES TO EACH SPECIE. 
    Specie[] baseSpecies = new Specie[5];

    //Unknown species
    Specie[] undiscoveredSpecies = new Specie[5];// = { new Specie() , new Specie(), new Specie(), new Specie(), new Specie()};
    //All species
    public Sprite beeSprite;
    public Sprite moleSprite;
    public Sprite cornSprite;
    public Sprite mooseSprite;
    public Sprite pufferfishSprite;

    public Sprite beeMoleSprite;
    public Sprite beeCornSprite;
    public Sprite moleCornSprite;
    public Sprite beeMooseSprite;
    public Sprite moleMooseSprite;
    public Sprite cornMooseSprite;
    public Sprite beePufferfishSprite;
    public Sprite molePufferfishSprite;
    public Sprite cornPufferfishSprite;
    public Sprite moosePufferfishSprite;

    // FARM-ATTRIBUTES.
    Dictionary<int, Seed> plantedSeeds;

    // Other
    public bool showHelpOverlay = true;
    public Specie firstInsertedSpecie;
    public Specie secondInsertedSpecie;


    // Update is called once per frame
    void Update () {
		
	}
	
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			DestroyObject (gameObject);
		}
		DontDestroyOnLoad (this);

		// init HUD-State
		this.survivors = 10;
		this.currentJobs = new List<Job> ();
		this.inventorySeeds = new Dictionary<Seed, int>();



        //// init Species-State
        //initSpeciesState();
		
        // init Calendar-State
        this.naturalDisasters = new Dictionary<int, NaturalDisaster>();
		this.totalDays = TOTAL_DAYS; //FIXME: Set correctly

		// init Farm-State
		this.plantedSeeds = new Dictionary<int, Seed>();

        // Set disasters at given interval
        
		setDisasters (DISASTER_INTERVAL);

	}

    private void initSpeciesState()
    {
        List<DisasterProperty> beeResistances = new List<DisasterProperty>();
        beeResistances.Add(DisasterProperty.EARTHQUAKE);
        List<Specie.PositiveProperty> beeProperties = new List<Specie.PositiveProperty>();
        beeProperties.Add(Specie.PositiveProperty.EXTRA_SEED_PER_WORKER);
        List<Specie.NegativeProperty> beeNegatives = new List<Specie.NegativeProperty>();
        beeNegatives.Add(Specie.NegativeProperty.LESS_FOOD_POINT);
        beeNegatives.Add(Specie.NegativeProperty.LESS_DURABILITY);

        Specie beeSpecie = new Specie(
            "bee",
            1, //foodpoint
            1, //durability
            1, //growtime
            beeResistances,
            beeProperties,
            beeNegatives,
            beeSprite,
            5, //seeds per worker
            Specie.Rarity.COMMON,
            Specie.Type.DEFAULT);

        List<DisasterProperty> moleResistances = new List<DisasterProperty>();
        moleResistances.Add(DisasterProperty.WIND);
        List<Specie.PositiveProperty> moleProperties = new List<Specie.PositiveProperty>();
        moleProperties.Add(Specie.PositiveProperty.EXTRA_DURABILITY);
        List<Specie.NegativeProperty> moleNegatives = new List<Specie.NegativeProperty>();
        moleNegatives.Add(Specie.NegativeProperty.REMOVES_RANDOM_RESISTANCE);
        moleNegatives.Add(Specie.NegativeProperty.EXTRA_GROWTIME);
        Specie moleSpecie = new Specie(
            "mole",
            3, //foodpoint
            5, //durability
            2, //growtime
            moleResistances,
            moleProperties,
            moleNegatives,
            moleSprite,
            3, //seeds per worker
            Specie.Rarity.COMMON,
            Specie.Type.DEFAULT);

        List<DisasterProperty> cornResistances = new List<DisasterProperty>();
        List<Specie.PositiveProperty> cornProperties = new List<Specie.PositiveProperty>();
        cornProperties.Add(Specie.PositiveProperty.LESS_GROWTIME);
        List<Specie.NegativeProperty> cornNegatives = new List<Specie.NegativeProperty>();
        cornNegatives.Add(Specie.NegativeProperty.REMOVES_RANDOM_RESISTANCE);

        Specie cornSpecie = new Specie(
            "corn",
            2, //foodpoint
            2, //durability
            2, //growtime
            cornResistances,
            cornProperties,
            cornNegatives,
            cornSprite,
            3, //seeds per worker
            Specie.Rarity.COMMON,
            Specie.Type.DEFAULT);

        List<DisasterProperty> mooseResistances = new List<DisasterProperty>();
        List<Specie.PositiveProperty> mooseProperties = new List<Specie.PositiveProperty>();
        mooseProperties.Add(Specie.PositiveProperty.EXTRA_FOOD_POINT);
        List<Specie.NegativeProperty> mooseNegatives = new List<Specie.NegativeProperty>();
        mooseNegatives.Add(Specie.NegativeProperty.EXTRA_GROWTIME);

        Specie mooseSpecie = new Specie(
            "moose",
            10, //foodpoint
            2, //durability
            5, //growtime
            mooseResistances,
            mooseProperties,
            mooseNegatives,
            mooseSprite,
            2, //seeds per worker
            Specie.Rarity.COMMON,
            Specie.Type.DEFAULT);

        List<DisasterProperty> pufferfishResistances = new List<DisasterProperty>();
        pufferfishResistances.Add(DisasterProperty.WATER);
        List<Specie.PositiveProperty> pufferfishProperties = new List<Specie.PositiveProperty>();
        List<Specie.NegativeProperty> pufferfishNegatives = new List<Specie.NegativeProperty>();
        pufferfishNegatives.Add(Specie.NegativeProperty.LESS_SEED_PER_WORKER);
        Specie pufferfishSpecie = new Specie(
            "pufferfish",
            1, //foodpoint
            1, //durability
            1, //growtime
            pufferfishResistances,
            pufferfishProperties,
            pufferfishNegatives,
            pufferfishSprite,
            1, //seeds per worker
            Specie.Rarity.COMMON,
            Specie.Type.DEFAULT);
       // baseSpecies = new Specie[5]; //TODO: hacky
        baseSpecies[0] = beeSpecie;
        baseSpecies[1] = moleSpecie;
        baseSpecies[2] = cornSpecie;
        baseSpecies[3] = mooseSpecie;
        baseSpecies[4] = pufferfishSpecie;

        this.knownSpecies = new List<Specie>(baseSpecies);
        this.unknownSpecies = new List<Specie>(undiscoveredSpecies);
        //FIXME: There probably is a better way to init all species.
        this.allSpecies = new List<Specie>(knownSpecies);
        this.allSpecies.AddRange(unknownSpecies);
        this.mySplices = new List<Specie>();

        //TODO: 5 hacky lines. A million ways to solve, but this was by far the fastest
        mySplices.Add(null);
        mySplices.Add(null);
        mySplices.Add(null);
        mySplices.Add(null);
        mySplices.Add(null);

    }

    private void initInitialFarmState()
    {
        Seed baseSeedOne = new Seed(baseSpecies[2].name+ " seed", baseSpecies[2]);
        Seed baseSeedTwo = new Seed(baseSpecies[3].name + " seed", baseSpecies[3]);
        Seed baseSeedThree = new Seed(baseSpecies[4].name + " seed", baseSpecies[4]);
        Seed baseSeedFour = new Seed(baseSpecies[2].name + " seed", baseSpecies[2]);
        Seed baseSeedFive = new Seed(baseSpecies[1].name + " seed", baseSpecies[1]);
        baseSeedOne.daysGrown = 2;
        baseSeedTwo.daysGrown = 3;
        baseSeedThree.daysGrown = 0;
        baseSeedFour.daysGrown = 1;
        baseSeedFive.daysGrown = 2;
        plantedSeeds.Add(0, new Seed(baseSeedTwo));
        plantedSeeds.Add(1, new Seed(baseSeedTwo));
        plantedSeeds.Add(6, new Seed(baseSeedThree));
        plantedSeeds.Add(7, new Seed(baseSeedThree));
        plantedSeeds.Add(9, new Seed(baseSeedThree));
        plantedSeeds.Add(12, new Seed(baseSeedFour));
        plantedSeeds.Add(13, new Seed(baseSeedFour));
        plantedSeeds.Add(15, new Seed(baseSeedFour));
        plantedSeeds.Add(16, new Seed(baseSeedFour));
        plantedSeeds.Add(2, new Seed(baseSeedFive));
        plantedSeeds.Add(3, new Seed(baseSeedFive));
        plantedSeeds.Add(4, new Seed(baseSeedFive));
        //these are pregrown so they are "ok"
        plantedSeeds.Add(14, baseSeedOne);
        plantedSeeds.Add(17, baseSeedOne);
        plantedSeeds.Add(18, baseSeedOne);
        plantedSeeds.Add(20, baseSeedOne);
        plantedSeeds.Add(22, baseSeedOne);
        plantedSeeds.Add(23, baseSeedOne);

        setSeeds(new Seed(baseSeedFour), 5);
        mySplices[1] = baseSpecies[2];
        firstInsertedSpecie = baseSpecies[2];
        secondInsertedSpecie = baseSpecies[0];

    }

    void Start()
    {
        // init Species-State
        initSpeciesState();
        initInitialFarmState();
        //List <DisasterProperty> pufferResistances = new List<DisasterProperty>();
        //pufferResistances.Add(DisasterProperty.WATER);
        //List<DisasterProperty> mooseResistances = new List<DisasterProperty>();



        //List<DisasterProperty> moleResistances = new List<DisasterProperty>();
        //beeResistances.Add(DisasterProperty.WIND);


        ////temporary. TODO: delete when base species are properly implemented
        //baseSpecies[0].resistantProperties = new List<DisasterProperty>();
        //baseSpecies[1].resistantProperties = new List<DisasterProperty>();
        //baseSpecies[2].resistantProperties = new List<DisasterProperty>();
        //baseSpecies[0].positiveProperties = new List<Specie.PositiveProperty>();
        //baseSpecies[1].positiveProperties = new List<Specie.PositiveProperty>();
        //baseSpecies[2].positiveProperties = new List<Specie.PositiveProperty>();
        //baseSpecies[0].negativeProperties = new List<Specie.NegativeProperty>();
        //baseSpecies[1].negativeProperties = new List<Specie.NegativeProperty>();
        //baseSpecies[2].negativeProperties = new List<Specie.NegativeProperty>();

        //baseSpecies[0].image = moleSprite;

        //baseSpecies[0].resistantProperties.Add(DisasterProperty.EARTHQUAKE);
        //baseSpecies[0].resistantProperties.Add(DisasterProperty.WIND);
        //baseSpecies[0].positiveProperties.Add(Specie.PositiveProperty.EXTRA_DURABILITY);
        //baseSpecies[0].positiveProperties.Add(Specie.PositiveProperty.EXTRA_SEED_PER_WORKER);
        //baseSpecies[0].negativeProperties.Add(Specie.NegativeProperty.EXTRA_GROWTIME);


        //baseSpecies[1].resistantProperties.Add(DisasterProperty.EARTHQUAKE);
        //baseSpecies[1].resistantProperties.Add(DisasterProperty.WIND);
        //baseSpecies[1].resistantProperties.Add(DisasterProperty.WATER);
        //baseSpecies[1].positiveProperties.Add(Specie.PositiveProperty.LESS_GROWTIME);
        //baseSpecies[1].negativeProperties.Add(Specie.NegativeProperty.LESS_DURABILITY);

        //baseSpecies[2].resistantProperties.Add(DisasterProperty.WATER);
        //baseSpecies[2].positiveProperties.Add(Specie.PositiveProperty.EXTRA_FOOD_POINT);
        //baseSpecies[2].negativeProperties.Add(Specie.NegativeProperty.REMOVES_RANDOM_RESISTANCE);
    }


    // -------------------------JOB-METHODS--------------------------------------
    public bool addJob(JobType jobType, int numWorkers, int id) {
		//if insufficient survivors or available workers -> cancel job.
		if ((survivors - this.currentJobs.Sum(j => j.numWorkers)) < numWorkers) {
			return false;
		} else {
			//create job
			this.currentJobs.Add(new Job(jobType, id, numWorkers));
			return true;
		}
	}	

	//TO BE TRIGGED WHEN NEXT DAY IS PRESSED. 
	public 	void resetJobs() {
		//new day, new jobs.
		this.currentJobs.Clear();
	}

    //GETTERS AND SETTERS
    public List<Specie> GetKnownSpecies()
    {
        return knownSpecies;
    }
    public void AddKnownSpecies(Specie specie){
        this.knownSpecies.Add(specie);
        this.unknownSpecies.Remove(specie);
    }

    public List<Specie> GetUnknownSpecies(){
        return unknownSpecies;
    }

    public List<Specie> GetAllSpecies()
    {
        return allSpecies;
    }

    public List<Specie> GetMySplices(){
        return mySplices;
    }
    public void AddMySplices(Specie specie)
    {
        this.mySplices.Add(specie);
    }

    public int GetAvailableWorkers(){
        return survivors - this.currentJobs.Sum(j => j.numWorkers);
    }
    
    public Dictionary<Seed, int> getInventorySeeds()
    {
        return inventorySeeds;
    }

    public void setSeeds(Seed seed, int numSeeds)
    {
        if (inventorySeeds.ContainsKey(seed)){
            inventorySeeds[seed] += numSeeds;
        }
        else{
            inventorySeeds.Add(seed, numSeeds);
        }
    }

    public Dictionary<int, Seed> getPlantedSeeds()
    {
        return plantedSeeds;
    }

    public int getFoodPointsConsumed(){
        return this.foodPointsConsumed;
    }

    public void addFoodPointsConsumed(int fp){
        this.foodPointsConsumed += fp;
    }

	//------------------------CALENDAR-STATE-----------------------------

	public void pressNextDay() {
		// DEATHLOGIC. TODO: Apply return value in window?
		print("Num dead = " + (this.survivors-calculateSurvivors()));

		// ADD ACTIVATION OF A NATURAL DISASTER IF PRESENT
		if (checkDisaster (daysPassed)) {
			checkSeedResistance(daysPassed);
			//TODO: NOTIFY?
		}
			
		// Seeds have now grown for another day. Some may even be ripe. 
		incrementSeedDaysGrown ();

		// INCEREMENT DAY AND RESET FOODPOINTSCONSUMED
		daysPassed++;
		this.foodPointsConsumed = 0;
        resetJobs(); //Isn't this what we want? reset jobs every day
		//TODO: CHECK IF ANY SEEDS ARE RIPE.

		// ADD LOGIC TO CHECK IF GAME IS WON/OVER.
        if (daysPassed == 20)
        {
            SceneHandler.instance.changeScene(SceneHandler.SCENES.VICTORY);
        }
        if (survivors <= 0)
        {
            SceneHandler.instance.changeScene(SceneHandler.SCENES.LOSS);
        }
	}

	// method that loops through each seed and checks it corresponding resitances vs the disastertype.
	// if no resitance it removes the seed from plantedSeeds
	public void checkSeedResistance(int disasterDay) {
		NaturalDisaster disasterType = this.naturalDisasters[disasterDay];
		//loop through all seeds and check resistance.
		List<int> removableItems = new List<int>();
		foreach (var item in this.plantedSeeds) {
			bool resistant = false;
			foreach (DisasterProperty resistance in item.Value.specie.resistantProperties) {
				if (resistance == disasterType.property) {
					resistant = true;
					break;
				}
			}
			//remove seed from item if no resistance
			if (!resistant) {
				removableItems.Add(item.Key);
			}
		}

		foreach (var key in removableItems) {
            //this.plantedSeeds.Remove (key);
            this.plantedSeeds[key].daysGrown = 500; //TODO: HACKY! Needs fix if this is ever to be built upon (also changed from original design of "removing" stuff 
        }

	}

    public int getDaysPassed()
    {
        return daysPassed;
    }


	//SETS PREDEFINED DISASTERS AND ADDS A RANDOM DISASTER TO EACH EVENT.
	public void setDisasters(int interval) {
		// FOR EACH X DAY, SET TO A RANDOM DISASTER. 
		for (int i = interval; i <= totalDays; i += interval) {
			DisasterProperty type = EnumUtil.RandomEnumValue<DisasterProperty> ();
			switch (type) 
			{
			//TODO: ADD CASE FOR EACH PROPERTY THAT IS ADDED. 
			case DisasterProperty.WIND:
				this.naturalDisasters.Add (i, new NaturalDisaster ("Hurricane", type));
				break;
			case DisasterProperty.EARTHQUAKE:
				this.naturalDisasters.Add (i, new NaturalDisaster ("Earthquake", type));
				break;
			case DisasterProperty.WATER:
				this.naturalDisasters.Add (i, new NaturalDisaster ("Tsunami", type));
				break;
			default:
				print("Default case! NOTHING ADDED :((");
				break;
			}
		}
	}
		
	public bool checkDisaster(int day) {
		return this.naturalDisasters.ContainsKey (day);
	}

    public int getFirstDisasterDate()
    {
        int firstDate = 1000;

        foreach (int date in naturalDisasters.Keys)
        {
            if (date > daysPassed && date < firstDate)
            {
                firstDate = date;
            }
        }
        return firstDate;
    }

    public NaturalDisaster getDisaster(int day)
    {
        return naturalDisasters[day];
    }

    public int calculateSurvivors() {
		float rest = this.survivors*FOOD_POINT_PER_WORKER - this.foodPointsConsumed;
		this.survivors = this.survivors - Mathf.CeilToInt (rest / (float)FOOD_POINT_PER_WORKER);
		return this.survivors;
	}


	//------------------------FARM-STATE-----------------------------

	// plants seed at desired index if available slot
	public void plantSeed(Seed seed, int index) {
		if (emptyFarmSlot (index)) {
            //NOTE: need new Seed here because otherwise the "planted seeds" will refer to the same seed instead of individual ones
            this.plantedSeeds.Add (index, new Seed(seed));
            inventorySeeds[seed]--;
            if (inventorySeeds[seed]==0)
            {
                inventorySeeds.Remove(seed);
            }
		}
	}

	public bool emptyFarmSlot(int index) {
		//int x = this.IMAGE_WIDTH % index;
		//int y = this.IMAGE_WIDTH / index;
		return !this.plantedSeeds.ContainsKey (index);
	
	}

	// method that increments number of days grown for each seed. 
	// to be called when next day is clicked
	public void incrementSeedDaysGrown() {
        foreach (Seed seed in this.plantedSeeds.Values) {
			seed.daysGrown++;
            //print(seed.name);
            //print(seed.daysGrown);
		}
	}

	// TODO CONSIDER NON-VOID
	public bool eatFood(int index) {
		if(checkRipeSeed(index)){
			Seed seed = this.plantedSeeds[index];
			this.plantedSeeds.Remove(index);
			Specie food = seed.specie;
			this.foodPointsConsumed += food.foodPoint;
            return true;
		}
        return false;
    }

	public bool checkRipeSeed(int index) {
		if (!emptyFarmSlot (index)) {
			//TODO: IMPLEMENT DURABILITY ALSO? WHERE TO SET EDIBLE? 
			if (this.plantedSeeds [index].daysGrown >= this.plantedSeeds [index].specie.growTime) {
				//TODO: IS THIS ATTRIBUTE EVEN NECESSARY?
				this.plantedSeeds [index].specie.edible = true;
				return true;
			}	
		}
		return false;
	}

    public void getHUDData(out int outSurvivors, out int outNumberOfWorkers, out int outDaysPassed)
    {
        outSurvivors = survivors;
        outNumberOfWorkers = 0;
        foreach (Job job in currentJobs)
        {
            outNumberOfWorkers += job.numWorkers;
        }
        outDaysPassed = daysPassed;

    }

    enum baseSpeciesValues
    {
        BEE = 0,
        MOLE = 1,
        CORN = 3,
        MOOSE = 7,
        PUFFERFISH = 15
    }
    enum splicedSpeciesValues
    {
        BEEBEE=0,
        BEEMOLE=1,
        MOLEMOLE=2,
        BEECORN=3,
        MOLECORN=4,
        CORNCORN=6,
        BEEMOOSE=7,
        MOLEMOOSE=8,
        CORNMOOSE=10,
        MOOSEMOOSE=14,
        BEEPUFFERFISH=15,
        MOLEPUFFERFISH=16,
        CORNPUFFERFISH=18,
        MOOSEPUFFERFISH=22,
        PUFFERFISHPUFFERFISH=30
    }

    public string getSpecieName(string baseSpecie1, string baseSpecie2)
    {
        string returnString = "";
        int switchValue = 0;

        switch (baseSpecie1)
        {
            case "bee":
                switchValue += (int)baseSpeciesValues.BEE;
                break;
            case "mole":
                switchValue += (int)baseSpeciesValues.MOLE;
                break;
            case "corn":
                switchValue += (int)baseSpeciesValues.CORN;
                break;
            case "moose":
                switchValue += (int)baseSpeciesValues.MOOSE;
                break;
            case "pufferfish":
                switchValue += (int)baseSpeciesValues.PUFFERFISH;
                break;
            default:
                break;
        }

        switch (baseSpecie2)
        {
            case "bee":
                switchValue += (int)baseSpeciesValues.BEE;
                break;
            case "mole":
                switchValue += (int)baseSpeciesValues.MOLE;
                break;
            case "corn":
                switchValue += (int)baseSpeciesValues.CORN;
                break;
            case "moose":
                switchValue += (int)baseSpeciesValues.MOOSE;
                break;
            case "pufferfish":
                switchValue += (int)baseSpeciesValues.PUFFERFISH;
                break;
            default:
                break;
        }

        switch (switchValue)
        {
            case (int)splicedSpeciesValues.BEEBEE:
                returnString = "bee";
                break;
            case (int)splicedSpeciesValues.BEEMOLE:
                returnString = "boole";
                break;
            case (int)splicedSpeciesValues.MOLEMOLE:
                returnString = "mole";
                break;
            case (int)splicedSpeciesValues.BEECORN:
                returnString = "ceern";
                break;
            case (int)splicedSpeciesValues.MOLECORN:
                returnString = "cole";
                break;
            case (int)splicedSpeciesValues.CORNCORN:
                returnString = "corn";
                break;
            case (int)splicedSpeciesValues.BEEMOOSE:
                returnString = "booze";
                break;
            case (int)splicedSpeciesValues.MOLEMOOSE:
                returnString = "mole";
                break;
            case (int)splicedSpeciesValues.CORNMOOSE:
                returnString = "moorn";
                break;
            case (int)splicedSpeciesValues.MOOSEMOOSE:
                returnString = "moose";
                break;
            case (int)splicedSpeciesValues.BEEPUFFERFISH:
                returnString = "beeferfish";
                break;
            case (int)splicedSpeciesValues.MOLEPUFFERFISH:
                returnString = "puffermole";
                break;
            case (int)splicedSpeciesValues.CORNPUFFERFISH:
                returnString = "cornerfish";
                break;
            case (int)splicedSpeciesValues.MOOSEPUFFERFISH:
                returnString = "moosefish";
                break;
            case (int)splicedSpeciesValues.PUFFERFISHPUFFERFISH:
                returnString = "pufferfish";
                break;
        }

        return returnString;
    }

    public Sprite getSpecieSprite(string baseSpecie1, string baseSpecie2)
    {
        int switchValue = 0;
        Sprite returnSprite = null;
        switch(baseSpecie1)
        {
            case "bee":
                switchValue += (int)baseSpeciesValues.BEE;
                break;
            case "mole":
                switchValue += (int)baseSpeciesValues.MOLE;
                break;
            case "corn":
                switchValue += (int)baseSpeciesValues.CORN;
                break;
            case "moose":
                switchValue += (int)baseSpeciesValues.MOOSE;
                break;
            case "pufferfish":
                switchValue += (int)baseSpeciesValues.PUFFERFISH;
                break;
            default:
                break;
        }

        switch (baseSpecie2)
        {
            case "bee":
                switchValue += (int)baseSpeciesValues.BEE;
                break;
            case "mole":
                switchValue += (int)baseSpeciesValues.MOLE;
                break;
            case "corn":
                switchValue += (int)baseSpeciesValues.CORN;
                break;
            case "moose":
                switchValue += (int)baseSpeciesValues.MOOSE;
                break;
            case "pufferfish":
                switchValue += (int)baseSpeciesValues.PUFFERFISH;
                break;
            default:
                break;
        }

        switch (switchValue)
        {
            case (int)splicedSpeciesValues.BEEBEE:
                returnSprite = beeSprite;
                break;
            case (int)splicedSpeciesValues.BEEMOLE:
                returnSprite = beeMoleSprite;
                break;
            case (int)splicedSpeciesValues.MOLEMOLE:
                returnSprite = moleSprite;
                break;
            case (int)splicedSpeciesValues.BEECORN:
                returnSprite = beeCornSprite;
                break;
            case (int)splicedSpeciesValues.MOLECORN:
                returnSprite = moleCornSprite;
                break;
            case (int)splicedSpeciesValues.CORNCORN:
                returnSprite = cornSprite;
                break;
            case (int)splicedSpeciesValues.BEEMOOSE:
                returnSprite = beeMooseSprite;
                break;
            case (int)splicedSpeciesValues.MOLEMOOSE:
                returnSprite = moleMooseSprite;
                break;
            case (int)splicedSpeciesValues.CORNMOOSE:
                returnSprite = cornMooseSprite;
                break;
            case (int)splicedSpeciesValues.MOOSEMOOSE:
                returnSprite = mooseSprite;
                break;
            case (int)splicedSpeciesValues.BEEPUFFERFISH:
                returnSprite = beePufferfishSprite;
                break;
            case (int)splicedSpeciesValues.MOLEPUFFERFISH:
                returnSprite = molePufferfishSprite;
                break;
            case (int)splicedSpeciesValues.CORNPUFFERFISH:
                returnSprite = cornPufferfishSprite;
                break;
            case (int)splicedSpeciesValues.MOOSEPUFFERFISH:
                returnSprite = moosePufferfishSprite;
                break;
            case (int)splicedSpeciesValues.PUFFERFISHPUFFERFISH:
                returnSprite = pufferfishSprite;
                break;
        }
        return returnSprite;
    }

}