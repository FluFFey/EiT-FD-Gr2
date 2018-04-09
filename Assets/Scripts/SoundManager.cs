using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;
    private SoundCaller sc;

    public AudioClip cloneSound;
    public AudioClip spliceSound;
    public AudioClip farmSound;
    public AudioClip harvestSound;
    public AudioClip harvestRottenSound;
    public AudioClip placeBaseSpecieInSpliceMachineSound;
    public AudioClip selectShelfObjectSound;
    public AudioClip feastSound;
    public AudioClip enterLabSound;
    public AudioClip leaveLabSound;
    public AudioClip labErrorSound;
    public AudioClip normalNightSound;
    public AudioClip earthquakeNightSound;
    public AudioClip windNightSound;
    public AudioClip waterNightSound;
    public AudioClip toggleSettings;
    public AudioClip defaultUISound;
    public AudioClip readBottleMessage;
    public enum SOUNDS
    {
        CLONE,
        SPLICE,
        FARM,
        HARVEST,
        HARVEST_ROTTEN,
        PLACE_IN_SPLICE_MACHINE,
        SELECT_SHELF_OBJ,
        FEAST,
        ERROR_LAB,
        ENTER_LAB,
        LEAVE_LAB,
        NIGHT_DEFAULT,
        NIGHT_EARTHQUAKE,
        NIGHT_WIND,
        NIGHT_WATER,
        DEFAULT_UI,
        READ_MESSAGE
    }

    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            DestroyObject(gameObject);
            //Destroy(this);
        }
        DontDestroyOnLoad(this);
        sc = GetComponent<SoundCaller>();
    }

    public void playSound(SOUNDS sound)
    {
        AudioClip clipToPlay = null;
        switch (sound)
        {
            case SOUNDS.CLONE:
                clipToPlay = cloneSound;
                break;
            case SOUNDS.SPLICE:
                clipToPlay = spliceSound;
                break;
            case SOUNDS.FARM:
                clipToPlay = farmSound;
                break;
            case SOUNDS.HARVEST:
                clipToPlay = harvestSound;
                break;
            case SOUNDS.HARVEST_ROTTEN:
                clipToPlay = harvestRottenSound;
                break;
            case SOUNDS.PLACE_IN_SPLICE_MACHINE:
                clipToPlay = placeBaseSpecieInSpliceMachineSound;
                break;
            case SOUNDS.SELECT_SHELF_OBJ:
                clipToPlay = selectShelfObjectSound;
                break;
            case SOUNDS.FEAST:
                clipToPlay = feastSound;
                break;
            case SOUNDS.ERROR_LAB:
                clipToPlay = labErrorSound;
                break;
            case SOUNDS.ENTER_LAB:
                clipToPlay = enterLabSound;
                break;
            case SOUNDS.LEAVE_LAB:
                clipToPlay = enterLabSound;
                break;
            case SOUNDS.NIGHT_DEFAULT:
                clipToPlay = normalNightSound;
                break;
            case SOUNDS.NIGHT_EARTHQUAKE:
                clipToPlay = earthquakeNightSound;
                break;
            case SOUNDS.NIGHT_WIND:
                clipToPlay = windNightSound;
                break;
            case SOUNDS.NIGHT_WATER:
                clipToPlay = waterNightSound;
                break;
            case SOUNDS.DEFAULT_UI:
                clipToPlay = defaultUISound;
                break;
            case SOUNDS.READ_MESSAGE:
                clipToPlay = readBottleMessage;
                break;
            default:
                print("invalid sound requested");
                break;
        }
        sc.attemptSound(clipToPlay,0.1f);
    }

    public SoundCaller getSoundCaller()
    {
        return sc;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
