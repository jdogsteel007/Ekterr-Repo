using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;

    public GameObject player;
    private Rigidbody2D playerRigid;

    public Animator animator;

    //like a list but all the sentences go in first, and then as the user reads we will load in new sentences
    private Queue<string> sentences;

	// Use this for initialization
	void Start () {

        sentences = new Queue<string>();

        playerRigid = player.GetComponent<Rigidbody2D>();

	}

    public void StartDialogue(Dialogue dialogue) {

        playerRigid.constraints = RigidbodyConstraints2D.FreezeAll;

        player.GetComponent<playerProjectile>().enabled = false;

        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue() {
        Debug.Log("End of convo");
        animator.SetBool("isOpen", false);
        playerRigid.constraints = RigidbodyConstraints2D.None;
        playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        player.GetComponent<playerProjectile>().enabled = true;
    }
	

}
