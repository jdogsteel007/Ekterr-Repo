using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {
	public float FireRate = 0.5f, BulletSpeed = 5f;
    public int BulletDamage = 1;
    public Color BulletTint = Color.white;
	// Use this for initialization
	void Start () {
        _timeSinceLastFire = FireRate;
	}
	
	// Update is called once per frame
	void Update () {
	}

	protected float _timeSinceLastFire = 0;
	public virtual void TryToFire()
	{
		_timeSinceLastFire += Time.deltaTime;
		if ( _timeSinceLastFire > FireRate) {
			_timeSinceLastFire = 0;
			Fire ();
		}
	}

    public virtual GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(Globals.Inst.DefaultBulletPrefab);
        bullet.GetComponent<Bullet>().Creator = gameObject;
        bullet.GetComponent<Bullet>().Damage = BulletDamage;
        bullet.GetComponent<Bullet>().MovementSpeed = BulletSpeed;
        bullet.GetComponent<Bullet>().FriendlyBullet = GetComponent<CombatEntity>().IsFriendly;
        bullet.GetComponent<SpriteRenderer>().color = BulletTint;
        bullet.transform.position = transform.position;
        if (GetComponent<playerProjectile>())
            bullet.transform.rotation = StaticHelper.LookAt2D(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        else
            bullet.transform.rotation = transform.rotation;
        return bullet;
    }

	public virtual void Fire()
	{
		Debug.Log ("Firing!");
	}
}
