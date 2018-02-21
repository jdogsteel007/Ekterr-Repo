using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class healthBarScript : MonoBehaviour {

    Image healthBar;
    float maxHealth = 100f;
    public static float health;

    public GameObject player;
    

	// Use this for initialization
	void Start () {

        healthBar = GetComponent<Image>();
        health = maxHealth;

	}
	
	// Update is called once per frame
	void Update () {
        healthBar.fillAmount = health / maxHealth;

        if (health == 0) {
            Destroy(player);
            SceneManager.LoadScene(1);
        }
	}
}
