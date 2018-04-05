using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteFade : MonoBehaviour {

    public float fadeTime = 1f;

	// Use this for initialization
	void Start () {
        StartCoroutine(FadeOut(GetComponent<SpriteRenderer>()));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator FadeOut(SpriteRenderer sprite) {
        Color tmpColor = sprite.color;

        while (tmpColor.a > 0) {
            tmpColor.a = Time.deltaTime / fadeTime;
            sprite.color = tmpColor;
            if (tmpColor.a <= 0)
                {
                    tmpColor.a = 0.0f;
                }
                    yield return null;
        }
        sprite.color = tmpColor;





    }

    

}
