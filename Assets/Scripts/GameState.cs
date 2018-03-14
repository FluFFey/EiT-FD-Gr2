using UnityEngine;
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


	//HUD-ATTRIBUTES
	int survivors; 
	List<Job> currentJobs;
    Dictionary<Seed, int> seeds;
    List<Specie> knownSpecies;
    List<Specie> unknownSpecies;
	List<Specie> mySplices { get; set; }

	//CALENDAR-ATTRIBUTES
	int totalDays;
	int daysPassed;
	Dictionary<int,  NaturalDisaster> naturalDisasters;

	//BASE-SPECIES
	//TODO: ADD VALUES TO EACH SPECIE. 
	Specie[] baseSpecies = { new Specie() , new Specie(), new Specie(), new Specie(), new Specie()};
	//Unknown species
	Specie[] undiscoveredSpecies = { new Specie() , new Specie(), new Specie(), new Specie(), new Specie()};

	// FARM-ATTRIBUTES.
	Dictionary<int, Seed> plantedSeeds;

	// Use this for initialization
	void Start () {
	}
	
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
		this.seeds = new Dictionary<Seed, int>();

		// init Species-State
		this.knownSpecies = new List<Specie>(baseSpecies); 
		this.unknownSpecies = new List<Specie>(undiscoveredSpecies); 
		this.mySplices = new List <Specie>();

		// init Calendar-State
		this.naturalDisasters = new Dictionary<int, NaturalDisaster>();
		this.totalDays = 20; //FIXME: Set correctly

		// init Farm-State
		this.plantedSeeds = new Dictionary<int, Seed>();

		// Set disasters at given interval

		setDisasters (DISASTER_INTERVAL);

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
		this.currentJobs = new List<Job>();
	}

    //Getters and setters
    public List<Specie> GetKnownSpecies()
    {
        return knownSpecies;
    }
    public void AddKnownSpecies(Specie specie){
        this.knownSpecies.Add(specie);
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
    
    public Dictionary<Seed, int> getSeeds()
    {
        return seeds;
    }

    public void setSeeds(Seed seed, int numSeeds)
    {
        if (seeds.ContainsKey(seed)){
            seeds[seed] += numSeeds;
        }
        else{
            seeds.Add(seed, numSeeds);
        }
    }

	//------------------------CALENDAR-STATE-----------------------------

	public void pressNextDay() {
		// ADD DEATHLOGIC


		// ADD ACTIVATION OF A NATURAL DISASTER IF PRESENT
		if (checkDisaster (3)) {
			checkSeedResistance(3);
			//TODO: NOTIFY?
		}
			
		// INCEREMENT DAY
		daysPassed++;

		incrementSeedDaysGrown ();

		//TODO: CHECK IF ANY SEEDS ARE RIPE.

		// ADD LOGIC TO CHECK IF GAME IS WON/OVER.
	}

	// method that loops through each seed and checks it corresponding resitances vs the disastertype.
	// if no resitance it seets the seed to null/0.
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
			this.plantedSeeds.Remove (key);
		}

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
		if (this.naturalDisasters.ContainsKey (day)) {
			return true;
		}
		return false;
	}

	//------------------------FARM-STATE-----------------------------

	// plants seed at desired index if available slot
	public void plantSeed(Seed seed, int index) {
		if (emptyFarmSlot (index)) {
			this.plantedSeeds.Add (index, seed);
		}
	}

	public bool emptyFarmSlot(int index) {
		//int x = this.IMAGE_WIDTH % index;
		//int y = this.IMAGE_WIDTH / index;

		if (this.plantedSeeds.ContainsKey (index)) {
			return false;
		} else {
			return true;
		}
	}

	// method that increments number of days grown for each seed. 
	// to be called when next day is clicked
	public void incrementSeedDaysGrown() {
		foreach (Seed seed in this.plantedSeeds.Values) {
			seed.daysGrown++;
		}
	}

}

