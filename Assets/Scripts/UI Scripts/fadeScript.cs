using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeScript : MonoBehaviour {

    public float alpha;

    public void FadeMe() {
        StartCoroutine(DoFade());
    }

    public void undoFadeMe()
    {
        StartCoroutine(UndoFade());
    }

    IEnumerator DoFade() {


        Canvas canvas = GetComponent<Canvas>();

        if (canvas.tag == "secondText") {
            canvas.GetComponent<CanvasGroup>().alpha = .99f;
        }

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha <= 1) {
            canvasGroup.alpha += Time.deltaTime * .35f;
            yield return null;
        }
        canvasGroup.interactable = false;
        yield return null;
    }

    IEnumerator UndoFade()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * .012f;
            yield return null;
        }
        canvasGroup.interactable = false;
        yield return null;
    }

    private void Update()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        alpha = canvasGroup.alpha;
    }

}
