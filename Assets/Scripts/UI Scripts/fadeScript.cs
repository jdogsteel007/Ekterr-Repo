using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeScript : MonoBehaviour {

    public void FadeMe() {
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade() {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha <= 1) {
            canvasGroup.alpha += Time.deltaTime * .35f;
            yield return null;
        }
        canvasGroup.interactable = false;
        yield return null;
    }

}
