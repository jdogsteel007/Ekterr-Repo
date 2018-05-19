using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeGun : BaseWeapon {

    // Use this for initialization
    public float ChargeAmt, ChargeTime = 3f, BulletScale = 5f, MinCharge = 0.1f, ImageSpinSpeed = 30f;
    public int MaxDamage = 5, MinDamage = 1;

    private GameObject chargeImage;

	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0))
        {
            ChargeAmt += Time.deltaTime * (1.0f / ChargeTime);
            if (ChargeAmt > 1.0f) ChargeAmt = 1.0f;

            if(chargeImage == null)
            {
                chargeImage = new GameObject();
                chargeImage.AddComponent<SpriteRenderer>();
                chargeImage.GetComponent<SpriteRenderer>().sprite = Globals.Inst.DefaultBulletPrefab.GetComponent<SpriteRenderer>().sprite;
                chargeImage.transform.parent = gameObject.transform;
                chargeImage.transform.localPosition = Vector3.zero;
            }
            chargeImage.transform.localScale = Vector3.one * ChargeAmt * BulletScale;
            chargeImage.transform.Rotate(0, 0, Time.deltaTime * ChargeAmt * ImageSpinSpeed);
        }
        else if (ChargeAmt > 0)
        {
            GameObject.Destroy(chargeImage);
            if (ChargeAmt < MinCharge) ChargeAmt = MinCharge;
            GameObject bullet = CreateBullet();
            bullet.transform.localScale = Vector3.one * ChargeAmt * BulletScale;
            int bulletDamage = (int)(ChargeAmt * MaxDamage);
            if (bulletDamage < MinDamage) bulletDamage = MinDamage;
            bullet.GetComponent<Bullet>().Damage = bulletDamage;
            ChargeAmt = 0;
        }
	}

    public override void TryToFire()
    {
    }
}
