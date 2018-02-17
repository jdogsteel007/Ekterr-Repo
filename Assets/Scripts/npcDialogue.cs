using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcDialogue : MonoBehaviour {


    public GameObject target;

    public int maxRange;
    public int minRange;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if ((Vector3.Distance(transform.position, target.transform.position) < maxRange)
            && (Vector3.Distance(transform.position, target.transform.position) > minRange))
        {

            if (Input.GetKeyDown(KeyCode.E)) {

                dialogueTrigger triggerDialouge = GetComponent<dialogueTrigger>();
                triggerDialouge.TriggerDialogue();

                Debug.Log("dialogue attempted");
            }

        }



    }
}
