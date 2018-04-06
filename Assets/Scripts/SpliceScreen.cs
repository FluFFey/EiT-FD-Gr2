using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpliceScreen : MonoBehaviour {

	List<int[]> probabilityAttributeTable;

    //private Specie firstSpecie;
    //private Specie secondSpecie;

    // NEEDLE-CONSTANTS(Don't think they can be constant when accessed as public
    public /*const*/ int postivePopertyCost = 2;
    public /*const*/ int disasterPropertyCost = 3;
	public /*const*/ int workerValue = 4;
	// Probabilities for generating negative properties for each amount of stamina threshold
	// TODO: BETTER NAMING
	private const int firstLeftOverStaminaProb = 70; 
	private const int secondLeftOverStaminaProb = 50;
	private const int thirdLeftOverStaminaProb = 30;
	private const int lastLeftOverStaminaProb = 10;

    //Property buttons. Should probably not be here, but is faster to implement
    public Sprite propertyButtonUp;
    public Sprite propertyButtonDown;

    public TextMesh spliceStatsTextMesh;
    public TextMesh cloneStatsTextMesh;
    public GameObject leftPropertyDisplay;
    public GameObject rightPropertyDisplay;

    public GameObject clonePropertyDisplay;
    // Use this for initialization
    void Start () {
        //spliceStatsTextMesh = GameObject.Find("spliceStatsText").GetComponent<TextMesh>();
        //leftPropertyDisplay = GameObject.Find("propertiesLeft");
        //rightPropertyDisplay = GameObject.Find("propertiesRight");

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

		/* KEEPING THIS FOR LATER TESTS. List<DisasterProperty> disProp = new List<DisasterProperty> ();
		disProp.Add (DisasterProperty.EARTHQUAKE);

		List<Specie.PositiveProperty> posProp = new List<Specie.PositiveProperty> ();
		posProp.Add (Specie.PositiveProperty.EXTRA_DURABILITY);

		List<Specie.NegativeProperty> negProp = new List<Specie.NegativeProperty> ();
		negProp.Add (Specie.NegativeProperty.LESS_DURABILITY);

		firstSpecie = new Specie("Ørn",2,2,1, disProp,posProp, negProp,null, 2, 0,Specie.Rarity.COMMON, Specie.Type.FOREST);

		disProp = new List<DisasterProperty> ();
		disProp.Add (DisasterProperty.WATER);

		posProp = new List<Specie.PositiveProperty> ();
		posProp.Add (Specie.PositiveProperty.EXTRA_FOOD_POINT);

		negProp = new List<Specie.NegativeProperty> ();
		negProp.Add (Specie.NegativeProperty.LESS_FOOD_POINT);

		secondSpecie = new Specie("Fisk",2,2,1, disProp, posProp, negProp,null, 2, 0,Specie.Rarity.RARE, Specie.Type.WATER);
        //calcAttributes (null, null, 2);

		this.spliceSpecies (firstSpecie, secondSpecie, 2, firstSpecie.resistantProperties, secondSpecie.positiveProperties);
*/
    }
		
	public Specie spliceSpecies(Specie baseSpecie1, Specie baseSpecie2, int numWorkers, List<DisasterProperty> chosenDisProps, List<Specie.PositiveProperty> chosenPosPros) {
		//new splice with calculated attributes. 
		Specie splicedSpecie = calcAttributes(baseSpecie1, baseSpecie2, numWorkers);
        //splicedSpecie.name = GameState.instance.getSpecieName(baseSpecie1.name, baseSpecie2.name); //is needed when calculating for displaying on screen so redundant here?
        //splicedSpecie.name = baseSpecie1.name + baseSpecie2.name;
        splicedSpecie.image = GameState.instance.getSpecieSprite(baseSpecie1.name, baseSpecie2.name);
        //CREATE NEW SPECIES BASED ON ATTRIBUTES AND  SET PROPERTIES. 
        splicedSpecie.resistantProperties = chosenDisProps;

		// integer which evaluates how much of the needle you have used based on selected properties. 
		int propertyCost = 0;

		propertyCost += chosenDisProps.Count * disasterPropertyCost;
		propertyCost += chosenPosPros.Count * postivePopertyCost;
		print("PROPCOST: " + propertyCost);

		// SET SPLICED SPECIES.POS PROPS
		splicedSpecie.positiveProperties = chosenPosPros;

		// UPDATE SPECIE's ATTRIBUTES BASED ON THE PROPS IT GOT.
		print(splicedSpecie.foodPoint);
		splicedSpecie.positiveProperties.ForEach(prop => {
			
			switch (prop) 
			{
				//TODO: ADD CASE FOR EACH PROPERTY THAT IS ADDED. 
				case Specie.PositiveProperty.EXTRA_DURABILITY:
					splicedSpecie.durability++;
					break;
				case Specie.PositiveProperty.EXTRA_FOOD_POINT:
					splicedSpecie.foodPoint ++;
					break;
				case Specie.PositiveProperty.EXTRA_SEED_PER_WORKER:
					splicedSpecie.seedsPerWorker ++;
					break;
				case Specie.PositiveProperty.LESS_GROWTIME:
					splicedSpecie.growTime --;
					break;
				//TODO: HANDLE ANY MORE? 
				default:
					print("default case: NON-HANDLED POS-PROP");
					break;
			}
		});

		print(splicedSpecie.foodPoint);

		print("POSITIVE PROPS: ");
		splicedSpecie.positiveProperties.ForEach (prop => print (prop));

		// SET NEGATIVE PROPS BASED ON NUM_WORKERS AND COST 
		splicedSpecie.negativeProperties = calcNegativeProperties(baseSpecie1, baseSpecie2, numWorkers, propertyCost);
		print("NEGATIVE PROPS: ");
		splicedSpecie.negativeProperties.ForEach (prop => print (prop));
		//UPDATE ATTRIBUTES BASED ON NEGATIVE ATTRIBUtES.

		splicedSpecie.negativeProperties.ForEach(prop => {
			
			switch (prop) 
			{
				//TODO: ADD CASE FOR EACH PROPERTY THAT IS ADDED. 
				case Specie.NegativeProperty.LESS_DURABILITY:
					splicedSpecie.durability--;
					break;
				case Specie.NegativeProperty.LESS_FOOD_POINT:
					splicedSpecie.foodPoint--;
					break;
				case Specie.NegativeProperty.LESS_SEED_PER_WORKER:
					splicedSpecie.seedsPerWorker --;
					break;
				case Specie.NegativeProperty.EXTRA_GROWTIME:
					splicedSpecie.growTime ++;
					break;
				case Specie.NegativeProperty.REMOVES_RANDOM_RESISTANCE:
					splicedSpecie.removeRandomResitance();
					break;
				default:
					print("default case: NON-HANDLED POS-PROP");
					break;
			}
		});
		//TODO: IMPLEMENT RARITY? INCREMENT BY ONE OF HIGHEST?
		//TODO: ADD IMAGE? Another lookuptable?
		return splicedSpecie;

	}

	//CALC SCORE BASED ON WORKERS AND SET ACCORDINGDLY ATTRIBUTES ACCORDINGLY
	public Specie calcAttributes(Specie baseSpecie1, Specie baseSpecie2, int numWorkers, bool alwaysAverageValues = false) {

		Specie splicedSpecie = new Specie();
        splicedSpecie.name = GameState.instance.getSpecieName(baseSpecie1.name, baseSpecie2.name);
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
			splicedSpecie.foodPoint = getMax(baseSpecie1.foodPoint, baseSpecie2.foodPoint);
			splicedSpecie.durability = getMax(baseSpecie1.durability, baseSpecie2.durability);
			splicedSpecie.growTime = getMax(baseSpecie1.growTime, baseSpecie2.growTime);
			splicedSpecie.seedsPerWorker = getMax(baseSpecie1.seedsPerWorker, baseSpecie2.seedsPerWorker);
		} else if (threshold >= probabilities [0]) {
			// CALC AVERAGE VALUES
			splicedSpecie.foodPoint = getAverage(baseSpecie1.foodPoint, baseSpecie2.foodPoint);
			splicedSpecie.durability = getAverage(baseSpecie1.durability, baseSpecie2.durability);
			splicedSpecie.growTime = getAverage(baseSpecie1.growTime, baseSpecie2.growTime);
			splicedSpecie.seedsPerWorker = getAverage(baseSpecie1.seedsPerWorker, baseSpecie2.seedsPerWorker);
		} else {
			// CALC MIN VALUES
			splicedSpecie.foodPoint = getMin(baseSpecie1.foodPoint, baseSpecie2.foodPoint);
			splicedSpecie.durability = getMin(baseSpecie1.durability, baseSpecie2.durability);
			splicedSpecie.growTime = getMin(baseSpecie1.growTime, baseSpecie2.growTime);
			splicedSpecie.seedsPerWorker = getMin(baseSpecie1.seedsPerWorker, baseSpecie2.seedsPerWorker);

		}
		return splicedSpecie;
	}

	public List<Specie.NegativeProperty> calcNegativeProperties(Specie baseSpecie1, Specie baseSpecie2, int num_workers, int propertyCost) {
		// GET RANDOM NEGATIVE PROPERTIES BASED ON NUM WORKERS

		List<Specie.NegativeProperty> negativeProps = new List<Specie.NegativeProperty> ();
		int needleStamina = num_workers*workerValue;

		// Amount of points left after allocation of workers and choosing of props 
		//TODO: HÅNDTERE minusverdier her eller i scenen?
		int rest = needleStamina - propertyCost;
		print("REST: " + rest);
		// switch-case which calculates negative properties based on amount of stamina used.
		// more stamina used -> higher probability for negative props.
		switch (rest) {
		case 0: 
			//get random negative prop. 
			negativeProps.Add(getRandomNegativeProperty(baseSpecie1, baseSpecie2));
			break;
		case 1:
		case 2:
		case 3:
			if(Random.value*100 < firstLeftOverStaminaProb) {
				negativeProps.Add(getRandomNegativeProperty(baseSpecie1, baseSpecie2));
			}
			break;
		case 4:
		case 5:
		case 6:
			if(Random.value*100 < secondLeftOverStaminaProb) {
				negativeProps.Add(getRandomNegativeProperty(baseSpecie1, baseSpecie2));
			}
			break;
		case 7:
		case 8:
		case 9:
		case 10:
			if(Random.value*100 < thirdLeftOverStaminaProb) {
				negativeProps.Add(getRandomNegativeProperty(baseSpecie1, baseSpecie2));
			}
			break;
		default:
			if(Random.value*100 < lastLeftOverStaminaProb) {
				negativeProps.Add(getRandomNegativeProperty(baseSpecie1, baseSpecie2));
			}
			break;
		}
	
		return negativeProps;

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
            case 3:
                //TODO: some structural oversights here. Could (should?) copy/paste code, but feel this is still better
                monitor = clonePropertyDisplay;
                break;
            default:
                print("wrong input to setPropertyMonitorBasedOnSpecie(...)");
                break;
        }
        int numberOfPossibleProperties = 5;
        for (int i = 0; i < numberOfPossibleProperties; i++)
        {
            monitor.transform.GetChild(i).GetComponent<TextMesh>().text = "";
            if (monitorNr !=3)
            {
                monitor.transform.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
            }
        }

        int counter = 0;

        foreach (DisasterProperty property in specie.resistantProperties)
        {
            monitor.transform.GetChild(counter).GetComponent<TextMesh>().text = getPropertyName(property);
            if (monitorNr != 3)
            {
                monitor.transform.GetChild(counter).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonUp;
            }
          //  print("property");
            counter++;
        }
        //counter = 0;
        foreach (Specie.PositiveProperty property in specie.positiveProperties)
        {
            monitor.transform.GetChild(counter).GetComponent<TextMesh>().text = getPropertyName(property);
            if (monitorNr != 3)
            {
                monitor.transform.GetChild(counter).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = propertyButtonUp;
            }
            counter++;
        }
        if (monitorNr == 3 && counter <5)
        {
            foreach (Specie.NegativeProperty property in specie.negativeProperties)
            {
                monitor.transform.GetChild(counter).GetComponent<TextMesh>().text = getPropertyName(property);
            }
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
            case Specie.NegativeProperty.LESS_SEED_PER_WORKER:
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

    public void updateStatsDisplay(Specie firstSpecie, Specie secondSpecie, int numberOfWorkers, bool cloneDisplay = false)
    {
        TextMesh textMeshToUpdate = cloneDisplay ? cloneStatsTextMesh : spliceStatsTextMesh;
        textMeshToUpdate.text = "";
        if (firstSpecie != null && ( secondSpecie != null || cloneDisplay))
        {
            Specie unfinishedSpecie = cloneDisplay ? 
                firstSpecie : 
                calcAttributes(firstSpecie, secondSpecie, numberOfWorkers, true);
            //print(unfinishedSpecie.foodPoint);
            textMeshToUpdate.text = "Name:             " + unfinishedSpecie.name +
                                 "\nFood:             " + unfinishedSpecie.foodPoint +
                                 "\nDurability:       " + unfinishedSpecie.durability +
                                 "\nGrow time:        " + unfinishedSpecie.growTime +
                                 "\nSeeds Per Worker: " + unfinishedSpecie.seedsPerWorker;
            if (!cloneDisplay)
            {
                setPropertyMonitorBasedOnSpecie(firstSpecie, 1);
                setPropertyMonitorBasedOnSpecie(secondSpecie, 2);
            }
            else
            {
                setPropertyMonitorBasedOnSpecie(unfinishedSpecie, 3);
            }
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
    //public Specie getFirstSpecie(){
    //    return firstSpecie;
    //}

    //public Specie setFirstSpecie(){
    //    return secondSpecie;
    //}

    //public void setFirstSpecie(Specie specie) {
    //    firstSpecie = specie;
    //}

    //public void setSecondSpecie(Specie specie){
    //    secondSpecie = specie;
    //}

	public Specie.NegativeProperty getRandomNegativeProperty(Specie specie1, Specie specie2) {
		List<Specie.NegativeProperty> negProps = new List<Specie.NegativeProperty> ();
        //TODO: checks should not be needed. fix later
        if (specie1.negativeProperties != null)
        {
            negProps.AddRange(specie1.negativeProperties);
        }
        if (specie2.negativeProperties != null)
        {
            negProps.AddRange(specie2.negativeProperties);
        }
		return negProps[(UnityEngine.Random.Range(0,negProps.Count))];
	}

}
