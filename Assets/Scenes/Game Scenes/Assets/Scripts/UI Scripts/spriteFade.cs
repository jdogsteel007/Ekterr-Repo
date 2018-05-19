using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteFade : MonoBehaviour {

    public float fadeTime = 0.1f;
    public int timeTillFadeIn;

	// Use this for initialization
	void Start () {
        StartCoroutine(FadeOut(GetComponent<SpriteRenderer>()));
        StartCoroutine(FadeIn(GetComponent<SpriteRenderer>()));

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator FadeOut(SpriteRenderer sprite) {
        Color tmpColor = sprite.color;

        while (tmpColor.a > 0) {
            //tmpColor.a = Time.deltaTime / fadeTime;
            tmpColor.a -= fadeTime;
            sprite.color = tmpColor;
            if (tmpColor.a <= 0)
                {
                    tmpColor.a = 0.0f;
                }
                    yield return null;
        }
        sprite.color = tmpColor;





    }

    IEnumerator FadeIn(SpriteRenderer sprite)
    {
        yield return new WaitForSeconds(timeTillFadeIn);

        Color tmpColor = sprite.color;

        while (tmpColor.a <= 1f)
        {
            //tmpColor.a = Time.deltaTime / fadeTime;
            tmpColor.a += fadeTime;
            sprite.color = tmpColor;
            if (tmpColor.a >= 1)
            {
                tmpColor.a = 1f;
            }
            yield return null;
        }
        sprite.color = tmpColor;





    }



}
