using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticleDestroyer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitBeforeDestroy());
    }

    private IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}

public class Bullet : MonoBehaviour {   //Base class for all bullets, uses physics2d functions instead of collider for optimization reasons

    public float MovementSpeed = 0.1f,      //How fast the bullet moves (in units per second)
        Lifespan = 1f,                      //How long the bullet can exist before it automatically despawns
        KillDistance = 20,                  //How far away from the player is the bullet is before it automatically despawns
        CirlceColliderRadius = 0.225f,      //Radius of circle overlap (if we're using it)
        BoxColliderWidth = 1f,              //Width of box overlap (if we're using it)
        BoxColliderHeight = 1f;             //Height of box overlap (if we're using it)
    public Vector2 velocity = Vector2.zero;
    public int Damage = 1;                  //Amount of damage the bullet does
    public bool FriendlyBullet = true,      //Friendly bullets can't hurt the player, unfriendly bullets can't hurt the enemies
        UseCircleCollider = false;          //Whether we're using a box or circle overlap
    public ParticleSystem Particles;

    private float _lifetime = 0f;           //How long the bullet has existed

    public GameObject Creator; //TEMP or maybe not so temp, the gameobject that created this bullet (so we know to ignore its collider)

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.Inst.Player && Vector3.Distance(transform.position, Globals.Inst.Player.transform.position) > KillDistance) //check if the bullet is still within range of the player to stay alive
            DestroyBullet();
        else
            transform.position += transform.up.normalized * MovementSpeed * Time.deltaTime; //Move the bullet forward. Since we're not using a rigidbody we have to compensate for frame time ourself.

        _lifetime += Time.deltaTime;
        if (_lifetime > Lifespan)
            DestroyBullet();

        //check if the bullet hit anything
        Collider2D otherCollider;
        if (UseCircleCollider)
        {
            otherCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), CirlceColliderRadius);
        }
        else
        {
            otherCollider = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), new Vector2(BoxColliderWidth, BoxColliderHeight), transform.rotation.eulerAngles.z);
        }
        if (Creator && otherCollider && otherCollider != Creator.GetComponent<Collider2D>())
        {
            if(otherCollider.gameObject.GetComponent<CombatEntity>() != null)
                otherCollider.gameObject.GetComponent<CombatEntity>().HandleBullet(this);
            //temp? below (destroy gameobject if it hits something, anything)
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        StopAllCoroutines();
        if(Particles)
        {
            Particles.gameObject.transform.parent = null;
            Particles.loop = false;
            Particles.Stop();
            Particles.gameObject.AddComponent<BulletParticleDestroyer>();
        }
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UseCircleCollider)
        {
            Gizmos.DrawWireSphere(transform.position, CirlceColliderRadius);
        }

        else
        {
            StaticHelper.Draw2DBoxWidget(transform.position, new Vector2(BoxColliderWidth, BoxColliderHeight), transform.rotation.eulerAngles.z);
        }
    }
#endif
}
