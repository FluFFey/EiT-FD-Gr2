using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpliceScreen : MonoBehaviour {

	List<int[]> probabilityAttributeTable;
    TextMesh statsTextMesh;
    GameObject leftPropertyDisplay;
    GameObject rightPropertyDisplay;
    // Use this for initialization
    void Start () {
        statsTextMesh = GameObject.Find("randomStatsText").GetComponent<TextMesh>();
        leftPropertyDisplay = GameObject.Find("propertiesLeft");
        rightPropertyDisplay = GameObject.Find("propertiesRight");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake() {
		this.probabilityAttributeTable = new List<int[]>();
		//PROBABILITIES FOR EACH NUMBER OF ASSIGNED WORKERS. Indexed from 0. 0 => 1 worker.
		this.probabilityAttributeTable.Add(new int[]{50,40,10});
		this.probabilityAttributeTable.Add(new int[]{20,50,30});
		this.probabilityAttributeTable.Add(new int[]{10,50,40});
		this.probabilityAttributeTable.Add(new int[]{0,0,100});
        //calcAttributes (null, null, 2);
    }
		
	public Specie spliceSpecies(Specie baseSpecie1, Specie baseSpeice2, int numWorkers, List<DisasterProperty> chosenProps) {
		//new splice with calculated attributes. 
		Specie splicedSpecie = calcAttributes(baseSpecie1, baseSpeice2, numWorkers);
		splicedSpecie.name = baseSpecie1.name + baseSpeice2.name;

		//CREATE NEW SPECIES BASED ON ATTRIBUTES AND  SET PROPERTIES. 
		splicedSpecie.resistantProperties = chosenProps;

		//TODO: IMPLEMENT RARITY? INCREMENT BY ONE OF HIGHEST?
		//TODO: ADD IMAGE? Another lookuptable?
		return splicedSpecie;

	}

	//CALC SCORE BASED ON WORKERS AND SET ACCORDINGDLY ATTRIBUTES ACCORDINGLY
	public Specie calcAttributes(Specie baseSpecie1, Specie baseSpeice2, int numWorkers, bool alwaysAverageValues = false) {

		Specie splicedSpecie = new Specie();

        double threshold;
		
        if (alwaysAverageValues)
        {
            //GET THRESHOLD WHICH ALWAYS EQUALS AVERAGE
            threshold = 1 + (double)probabilityAttributeTable[numWorkers - 1][1];
        }
        else
        {
            //GET THRESHOLD-VALUE BETWEEN 0-100
            threshold = Random.value * 100;
        }
		

		//GET VALUES FROM TABLE. 0-indexed.
		int[] probabilities = this.probabilityAttributeTable [numWorkers - 1];

		// CALC VALUES BASED ON THRESHOLD.
		if (threshold > probabilities [0] + probabilities[1]) {
			//CALC MAXVALUES
			splicedSpecie.foodPoint = getMax(baseSpecie1.foodPoint, baseSpeice2.foodPoint);
			splicedSpecie.durability = getMax(baseSpecie1.durability, baseSpeice2.durability);
			splicedSpecie.growTime = getMax(baseSpecie1.growTime, baseSpeice2.growTime);
			splicedSpecie.seedsPerWorker = getMax(baseSpecie1.seedsPerWorker, baseSpeice2.seedsPerWorker);
		} else if (threshold >= probabilities [0]) {
			// CALC AVERAGE VALUES
			splicedSpecie.foodPoint = getAverage(baseSpecie1.foodPoint, baseSpeice2.foodPoint);
			splicedSpecie.durability = getAverage(baseSpecie1.durability, baseSpeice2.durability);
			splicedSpecie.growTime = getAverage(baseSpecie1.growTime, baseSpeice2.growTime);
			splicedSpecie.seedsPerWorker = getAverage(baseSpecie1.seedsPerWorker, baseSpeice2.seedsPerWorker);
		} else {
			// CALC MIN VALUES
			splicedSpecie.foodPoint = getMin(baseSpecie1.foodPoint, baseSpeice2.foodPoint);
			splicedSpecie.durability = getMin(baseSpecie1.durability, baseSpeice2.durability);
			splicedSpecie.growTime = getMin(baseSpecie1.growTime, baseSpeice2.growTime);
			splicedSpecie.seedsPerWorker = getMin(baseSpecie1.seedsPerWorker, baseSpeice2.seedsPerWorker);

		}
		return splicedSpecie;
	}


	private int getAverage(int attr1, int attr2) {
		return Mathf.CeilToInt ((attr1 + attr2) / 2);
	}

	private int getMax(int attr1, int attr2) {
		return Mathf.Max (attr1, attr2);
	}

	private int getMin(int attr1, int attr2) {
		return Mathf.Min (attr1, attr2);
	}

    public void setPropertyMonitorBasedOnSpecie(Specie specie, int monitorNr)
    {
        GameObject monitor = null;
        switch (monitorNr)
        {
            case 1:
                monitor = leftPropertyDisplay;
                break;
            case 2:
                monitor = rightPropertyDisplay;
                break;
            default:
                print("wrong input to setPropertyMonitorBasedOnSpecie(...)");
                break;
        }
        int numberOfPossibleProperties = 5;
        for (int i = 0; i < numberOfPossibleProperties; i++)
        {
            monitor.transform.GetChild(i).GetComponent<TextMesh>().text = "";
        }

        int counter = 0;
        //TODO: which positive properties can you select? Add exceptions to this loop
        //foreach (Specie.PositiveProperty property in specie.positiveProperties)
        //{
        //    monitor.transform.GetChild(i).GetComponent<TextMesh>().text = getPropertyName(property);
        //    i++;
        //}
        
        foreach (DisasterProperty property in specie.resistantProperties)
        {
            monitor.transform.GetChild(counter).GetComponent<TextMesh>().text = getPropertyName(property);
            counter++;
        }
    }

    string getPropertyName(Specie.PositiveProperty property)
    {
        switch(property)
        {
            case Specie.PositiveProperty.EXTRA_DURABILITY:
                return "Extra durable";
            case Specie.PositiveProperty.EXTRA_FOOD_POINT:
                return "Nutritious";
            case Specie.PositiveProperty.EXTRA_SEED_PER_WORKER:
                return "More seeds";
            case Specie.PositiveProperty.LESS_GROWTIME:
                return "Faster growth";
            default:
                return "Wrong parameter to getPropertyName";
        }
        
    }

    string getPropertyName(DisasterProperty property)
    {
        switch (property)
        {
            case DisasterProperty.EARTHQUAKE:
                return "Resist Tremor";
            case DisasterProperty.WATER:
                return "Resist Water";
            case DisasterProperty.WIND:
                return "Resist Wind";
            default:
                return "Wrong parameter to getPropertyName";
        }
    }

    string getPropertyName(Specie.NegativeProperty property)
    {
        //I think this is needed. depends on how we want to display negative properties, if at all
        switch (property)
        {
            case Specie.NegativeProperty.EXTRA_GROWTIME:
                return "Increased growtime";
            case Specie.NegativeProperty.EXTRA_SEED_PER_WORKER:
                return "Less seeds";
            case Specie.NegativeProperty.LESS_DURABILITY:
                return "Decreased durability";
            case Specie.NegativeProperty.LESS_FOOD_POINT:
                return "Less food";
            case Specie.NegativeProperty.REMOVES_RANDOM_RESISTANCE:
                return "Lost resistance";
            default:
                return "Wrong parameter to getPropertyName";
        }
    }

    public void updateStatsDisplay(Specie firstSpecie, Specie secondSpecie,int numberOfWorkers)
    {
        statsTextMesh.text = "";
        if (firstSpecie != null && secondSpecie != null)
        {
            Specie unfinishedSpecie = calcAttributes(firstSpecie, secondSpecie, numberOfWorkers, true);
            statsTextMesh.text = "Name:             " + unfinishedSpecie.name +
                                 "\nFood:             " + unfinishedSpecie.foodPoint +
                                 "\nDurability:       " + unfinishedSpecie.durability +
                                 "\nGrow time:        " + unfinishedSpecie.growTime +
                                 "\nSeeds Per Worker: " + unfinishedSpecie.seedsPerWorker;
            setPropertyMonitorBasedOnSpecie(firstSpecie, 1);
            setPropertyMonitorBasedOnSpecie(secondSpecie, 2);
        }
        //uncomment if we want properties to  be displayed without selecting two species
        //if (firstSpecie != null)
        //{
        //    setPropertyMonitorBasedOnSpecie(firstSpecie, 1);
        //}
        //if (secondSpecie != null)
        //{
        //    setPropertyMonitorBasedOnSpecie(secondSpecie, 2);
        //}
        
    }

}
