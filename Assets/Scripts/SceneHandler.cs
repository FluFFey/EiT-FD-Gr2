using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    public static SceneHandler instance;

    public enum SCENES
    {
        OVERWORLD,
        FARM,
        LAB
    }
    string currentScene;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            DestroyObject(gameObject);
        }
        currentScene = "Overworld";
        DontDestroyOnLoad(this);
    }

    public void changeScene(SCENES sceneToLoad)
    {
        string oldScene = currentScene;
        switch (sceneToLoad)
        {
            case SCENES.FARM:
                SceneManager.LoadScene("Farm");
                SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
                currentScene = "Farm";
                break;
            case SCENES.OVERWORLD:
                SceneManager.LoadScene("Overworld");
                SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
                currentScene = "Overworld";
                break;
            case SCENES.LAB:
                SceneManager.LoadScene("Laboratory");
                SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
                currentScene = "Laboratory";
                break;
            default:
                print("Invalid scene name");
                break;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
