using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hurts the player over time as long as the decay is above the threshold
/// </summary>
public class DamagingGoo : MonoBehaviour {

    public float Lifetime = 2f, DamageThreshold = 0.25f, DamageRatePerSecond = 1;
    public int Damage = 1;

    private bool contactWithPlayer = false;
    private float timeSinceLastDamage = 0, currentLifetime;

	// Use this for initialization
	void Start () {
        currentLifetime = Lifetime;
	}
	
	// Update is called once per frame
	void Update () {
        if (Lifetime > 0)
        {
            currentLifetime -= (1f / Lifetime) * Time.deltaTime;
            Color col = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(col.r, col.g, col.b, currentLifetime / Lifetime);

            if (currentLifetime > DamageThreshold && contactWithPlayer)
            {
                //damage player
                if (timeSinceLastDamage < DamageRatePerSecond)
                    timeSinceLastDamage += Time.deltaTime;
                else
                {
                    timeSinceLastDamage = 0;
                    Globals.Inst.Player.Health -= Damage;
                    Debug.Log("Playerhealth: " + Globals.Inst.Player.Health);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Globals.Inst.Player.gameObject)
            contactWithPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Globals.Inst.Player.gameObject)
            contactWithPlayer = false;
    }
}
