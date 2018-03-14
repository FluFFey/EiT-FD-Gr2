using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {


    Text survivorsText;
    Text workersText;
    Text daysText;

    // Use this for initialization
    void Start () {
        survivorsText = transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        workersText = transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
        daysText = transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();

    }

	// Update is called once per frame
	void Update () {
		if (GameState.instance != null)
        {
            int numberOfSurvivors = 0;
            int numberOfWorkers = 0;
            int numberOfDaysPassed = 0;
            GameState.instance.getHUDData(out numberOfSurvivors, out numberOfWorkers, out numberOfDaysPassed);
            survivorsText.text = numberOfSurvivors.ToString();
            workersText.text = numberOfWorkers.ToString();
            daysText.text = numberOfWorkers.ToString();
        }
	}
}
