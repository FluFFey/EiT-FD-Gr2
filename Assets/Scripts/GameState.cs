using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


using System;


public class GameState : MonoBehaviour {

	public static GameState instance;

	//HUD-ATTRIBUTES
	int survivors; 
	List<Job> currentJobs;
	Dictionary<Seed, int> seeds;
	List<Specie> knownSpecies;
	List<Specie> unknownSpecies;
	List<Specie> mySplices;

	//CALENDAR-ATTRIBUTES
	int totalDays;
	int daysPassed;
	Dictionary<int,  NaturalDisaster> naturalDisasters;

	//BASE-SPECIES
	//TODO: ADD VALUES TO EACH SPECIE. 
	Specie[] baseSpecies = { new Specie() , new Specie(), new Specie(), new Specie(), new Specie()};
	//Unknown species
	Specie[] undiscoveredSpecies = { new Specie() , new Specie(), new Specie(), new Specie(), new Specie()};

	// Use this for initialization
	void Start () {
		totalDays = 100;
		setDisasters (10);
		pressNextDay();
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

		//init HUD-State
		this.survivors = 10;
		this.currentJobs = new List<Job> ();
		this.seeds = new Dictionary<Seed, int>();

		//init Species-State
		this.knownSpecies = new List<Specie>(baseSpecies); 
		this.unknownSpecies = new List<Specie>(undiscoveredSpecies); 
		this.mySplices = new List <Specie>();

		//init Calendar-State
		this.naturalDisasters = new Dictionary<int, NaturalDisaster>();

	}

	// -------------------------JOB-METHODS --------------------------------------
	bool addJob(JobType jobType, int numWorkers, int id) {
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
	void resetJobs() {
		//new day, new jobs.
		this.currentJobs = new List<Job>();
	}
		

	//------------------------CALENDAR-STATE-----------------------------

	public void pressNextDay() {
		// ADD DEATHLOGIC

		// ADD ACTIVATION OF A NATURAL DISASTER IF PRESENT
		bool val = checkDisaster(daysPassed);

		// INCEREMENT DAY
		daysPassed++;

		// ADD LOGIC TO CHECK IF GAME IS WON/OVER.
	}



	//SETS PREDEFINED DISASTERS AND ADDS A RANDOM DISASTER TO EACH EVENT.
	public void setDisasters(int interval) {
		// FOR EACH X DAY, SET TO A RANDOM DISASTER. 
		for (int i = interval; i <= totalDays; i += interval) {
			Property type = EnumUtil.RandomEnumValue<Property> ();
			switch (type) 
			{
			//TODO: ADD CASE FOR EACH PROPERTY THAT IS ADDED. 
			case Property.WIND:
				this.naturalDisasters.Add (i, new NaturalDisaster ("Hurricane", type));
				break;
			case Property.EARTHQUAKE:
				this.naturalDisasters.Add (i, new NaturalDisaster ("Earthquake", type));
				break;
			case Property.WATER:
				this.naturalDisasters.Add (i, new NaturalDisaster ("Tsunami", type));
				break;
			default:
				print("Default case! NOTHING ADDED :((");
				break;
			}
		}
	}

	//TODO: CONSIDER VOID.
	public bool checkDisaster(int day) {
		if (this.naturalDisasters.ContainsKey (day)) {
			//TODO EXECUTE DISASTER LOGIC
			return true;
		}
		return false;
	}
}

