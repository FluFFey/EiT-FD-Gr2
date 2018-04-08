using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript instance;
    public SpriteRenderer fadeSpriteRenderer;
    // Use this for initialization

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
        DontDestroyOnLoad(this);
        fadeSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start ()
    {
        
        //fade(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator fade(bool fadeToClear,float fadeTime=0.25f, float fadeFactor = 1)
    {
        Color newColor = fadeSpriteRenderer.color;
        float startAlpha = fadeSpriteRenderer.color.a;
        for (float f = 0; f < fadeTime; f+=Time.deltaTime)
        {
            float pd = f / fadeTime;
            pd *= pd;
            newColor = fadeSpriteRenderer.color;
            newColor.a = fadeToClear ?
                startAlpha - pd * fadeFactor * startAlpha :
                startAlpha + pd * fadeFactor * (1-startAlpha);
            fadeSpriteRenderer.color = newColor;
            yield return null;
        }
        newColor.a = fadeToClear ? 
            1-fadeFactor : 
            fadeFactor;
        fadeSpriteRenderer.color = newColor;
    }
}
