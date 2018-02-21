using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;

    public void TriggerDialogue() {


        FindObjectOfType<dialogueManager>().StartDialogue(dialogue);

    }

}
