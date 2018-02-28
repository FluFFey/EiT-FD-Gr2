using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//TODO: REPLACE WITH GAMESTATE?
public class GameState : MonoBehaviour {

	public static GameState instance;

	//HUD-ATTRIBUTES
	int survivors; 
	List<Job> currentJobs;
	Dictionary<Seed, int> seeds;
	List<Specie> knownSpecies;
	List<Specie> unknownSpecies;
	List<Specie> mySplices;

	//BASE-SPECIES
	//TODO: ADD VALUES TO EACH SPECIE. 
	Specie[] baseSpecies = { new Specie() , new Specie(), new Specie(), new Specie(), new Specie()};
	//Unknown species
	Specie[] undiscoveredSpecies = { new Specie() , new Specie(), new Specie(), new Specie(), new Specie()};

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

		//init HUD-State
		this.survivors = 10;
		this.currentJobs = new List<Job> ();
		this.seeds = new Dictionary<Seed, int>();

		//init Species-State
		//TODO ARRAYLIST DEPRECATED
		this.knownSpecies = new List<Specie>(baseSpecies); 
		this.unknownSpecies = new List<Specie>(undiscoveredSpecies); 
		this.mySplices = new List <Specie>();
	}

	// JOB-METHODS -------------------------------------------------------------

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
		

}

