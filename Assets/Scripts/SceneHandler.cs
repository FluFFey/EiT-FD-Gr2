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
        LAB,
        VICTORY,
        LOSS,
        MAIN_MENU
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
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }

    void OnSceneLoad(Scene scene, LoadSceneMode sceneMode)
    {
        if (instance != this)
        {
            return; //this is really fucking stupid. Unity calls onsceneload before awake for some reason, and the other object (which hasn't had time to be deleted in awake, finishes this function)
        }
        switch (scene.buildIndex)
        {
            case 0: //main menu
                print("back to main menu");
                Destroy(this); //TODO: does this even work as expected? should delete this object because main menu has own camera
                Destroy(MusicManager.instance.gameObject);
                Destroy(GameState.instance.gameObject);
                Destroy(CameraScript.instance.gameObject);
                break;
            case 1: //overworld
                Camera.main.transform.position = new Vector3(0.42f, 0, -4);
                Camera.main.backgroundColor = new Color(60.0f / 255.0f, 90.0f / 255.0f, 190.0f / 255.0f);
                break;
            case 2: //farm
                Camera.main.transform.position = new Vector3(0, 1, -10);
                Camera.main.backgroundColor = new Color(26.0f / 255.0f, 160.0f / 255.0f, 98.0f / 255.0f);
                FindObjectOfType<Canvas>().worldCamera = Camera.main;
                break;
            case 3: //laboratory
                Camera.main.transform.position = new Vector3(0, -2, -10);
                break;
            case 4: //Hud
                break;
            case 5: //Victory
                Camera.main.transform.position = new Vector3(0, 1, -10);
                break;
            case 6: //Game Over
                Camera.main.transform.position = new Vector3(0, 1, -10);
                break;
            default:
                print("Scene not properly added to build or something I don't know");
                break;
        }
        if (scene.buildIndex == 1 || 
            scene.buildIndex == 2 || 
            scene.buildIndex == 3 ||
            scene.buildIndex == 5 ||
            scene.buildIndex == 6) 
        {
            StartCoroutine(CameraScript.instance.fade(true));
            MusicManager.instance.transform.position = Camera.main.transform.position;
            SoundManager.instance.transform.position = Camera.main.transform.position;
        }
        
        
    }

    public void changeScene(SCENES sceneToLoad)
    {
        //string oldScene = currentScene;
        switch (sceneToLoad)
        {
            case SCENES.FARM:
                StartCoroutine(fadeOutToNewScene("Farm"));
                break;
            case SCENES.OVERWORLD:
                StartCoroutine(fadeOutToNewScene("Overworld"));
                break;
            case SCENES.LAB:
                StartCoroutine(fadeOutToNewScene("Laboratory"));
                break;
            case SCENES.VICTORY:
                StartCoroutine(fadeOutToNewScene("Victory"));
                break;
            case SCENES.LOSS:
                StartCoroutine(fadeOutToNewScene("GameOver"));
                break;
            case SCENES.MAIN_MENU:
                StartCoroutine(fadeOutToNewScene("MainMenu"));
                break;
            default:
                print("Invalid scene name");
                break;
        }
        
    }

    IEnumerator fadeOutToNewScene(string sceneName)
    {
        if (sceneName != "MainMenu")
        {
            StartCoroutine(CameraScript.instance.fade(false));
            yield return new WaitForSeconds(0.26f);
            print("møll0?");
            
        }
        SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
        if (sceneName != "MainMenu" && sceneName != "Victory" && sceneName != "Loss")
        {
            print("mølla?");
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
