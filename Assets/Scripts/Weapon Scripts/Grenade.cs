using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public float WaitDuration = 3f, MaxBlinkRate = 10f, DamageRadiusUnits = 5;
    public int Damage = 5;
    public GameObject ExplosionPrefab;

    private float CurrentTime = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CurrentTime += Time.deltaTime;
        GetComponent<SpriteRenderer>().color = new Color(1.0f, Mathf.Sin(Time.fixedTime * MaxBlinkRate * (CurrentTime / WaitDuration)), Mathf.Sin(Time.fixedTime * MaxBlinkRate * (CurrentTime / WaitDuration)));
        if(CurrentTime > WaitDuration)
        {
            Explode();
        }
	}

    void Explode()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, DamageRadiusUnits);
        foreach(Collider2D c in colls)
        {
            if (c.gameObject.GetComponent<CombatEntity>())
                c.gameObject.GetComponent<CombatEntity>().Health -= Damage;
        }
        Instantiate(ExplosionPrefab);
        Destroy(gameObject);
    }
}
