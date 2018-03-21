using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpliceScreen : MonoBehaviour {

	List<int[]> probabilityAttributeTable;
    private Specie firstSpecie;
    private Specie secondSpecie;
	// Use this for initialization
	void Start () {
		
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
        firstSpecie = null;
        secondSpecie = null;
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
	public Specie calcAttributes(Specie baseSpecie1, Specie baseSpeice2, int numWorkers) {

		Specie splicedSpecie = new Specie();

		//GET THRESHOLD-VALUE BETWEEN 0-100
		double threshold = Random.value*100;

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

    public Specie getFirstSpecie()
    {
        return firstSpecie;
    }

    public Specie setFirstSpecie()
    {
        return secondSpecie;
    }

    public void setFirstSpecie(Specie specie)
    {
        firstSpecie = specie;
    }

    public void setSecondSpecie(Specie specie)
    {
        secondSpecie = specie;
    }

}
