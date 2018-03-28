using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CombatEntity {  //Inherets from CombatEntity so we have health and bullet handling

    public float moveSpeed;

    private Animator anim;

    private bool playerMoving;
    //basically two different variables: lastMoveX and lastMoveY vv
    public Vector2 LastMove;

    private Rigidbody2D myRigidbody;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        Health = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {

        playerMoving = false;

        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));   //(Devin) here is a cleaner way of doing player movement, I hope you don't mind...
        if (movement.magnitude > 0.5)
        {
            playerMoving = true;
            myRigidbody.velocity = movement.normalized * moveSpeed;
            LastMove = movement.normalized;
        }
        else myRigidbody.velocity = Vector2.zero;

        /*

        if (Input.GetAxisRaw("Horizontal") > 0.5)
        {
            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidbody.velocity.y);
            playerMoving = true;
            //lastMove gets the value of the horizontal axis and keeps it instead of it returning to zero
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5)
        {
            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidbody.velocity.y);
            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }

        if (Input.GetAxisRaw("Vertical") > 0.5)
        {
            //transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
            playerMoving = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }
        else if (Input.GetAxisRaw("Vertical") < -0.5)
        {
            //transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
            playerMoving = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetAxisRaw("Horizontal") < .5f && Input.GetAxisRaw("Horizontal") > -.5f) {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        }

        if (Input.GetAxisRaw("Vertical") < .5f && Input.GetAxisRaw("Vertical") > -.5f)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
        }

        */

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", LastMove.x);
        anim.SetFloat("LastMoveY", LastMove.y);

    }

    public override void HandleBullet(Bullet bullet)
    {
        if (bullet.FriendlyBullet != IsFriendly)
            Health -= bullet.Damage;
        if (Health == 0)
        {
            OnKill.Invoke();
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject); //RIP!
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
