using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class healthBarScript : MonoBehaviour {  //(Devin) I changed this a little bit so it uses player's CombatEntity health instead of the one in here

    public Image healthBar;
    //float maxHealth = 100f;
    //public static float health;

    //public GameObject player;
    

	// Use this for initialization
	void Start () {
        //health = maxHealth;

	}

    // Update is called once per frame
    void Update () {
        healthBar.fillAmount = (float)Globals.Inst.Player.Health / (float)Globals.Inst.Player.MaxHealth;

        //if (Globals.Inst.Player.Health <= 0) {
        //Globals.Inst.Player.Kill();
        //}
    }
}
