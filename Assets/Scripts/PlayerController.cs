using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CombatEntity {  //Inherets from CombatEntity so we have health and bullet handling

    public float moveSpeed;

    private float sprintMult;
    private float timer;
    public float sprintTime;

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
        if (Globals.Inst.InputFocus == gameObject)
        {
            playerMoving = false;

            if (Globals.DidPlayerSwitchThisFrame)
                Globals.DidPlayerSwitchThisFrame = false;
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Globals.DidPlayerSwitchThisFrame = true;
                Globals.Inst.InputFocus = Globals.Inst.Cub.gameObject;
                Globals.Inst.MainCamera.GetComponent<CameraController>().followTarget = Globals.Inst.Cub.gameObject;
                return;
            }
            if (Input.GetKey(KeyCode.LeftShift) && timer <= sprintTime)
            {
                timer += Time.deltaTime;
                Debug.Log("sprinting " + timer);
                sprintMult = 1f + moveSpeed * .12f;

            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {

                timer = 0;

                Debug.Log("not sprinting " + timer);
                sprintMult = 1f;
            }
            else if (Input.GetMouseButton(0) && GetComponent<BaseWeapon>())
            {
                GetComponent<BaseWeapon>().TryToFire();
            }
            else
            {
                //Debug.Log("not sprinting outside loop " + timer);
                sprintMult = 1f;
            }

            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));   //(Devin) here is a cleaner way of doing player movement, I hope you don't mind...
            if (movement.magnitude > 0.5)
            {
                playerMoving = true;
                myRigidbody.velocity = movement.normalized * moveSpeed * sprintMult;
                LastMove = movement.normalized;
            }
            else myRigidbody.velocity = Vector2.zero;

            anim.SetFloat("MoveX", movement.x);
            anim.SetFloat("MoveY", movement.y);
            anim.SetBool("PlayerMoving", playerMoving);
            anim.SetFloat("LastMoveX", LastMove.x);
            anim.SetFloat("LastMoveY", LastMove.y);
        }
        else myRigidbody.velocity = Vector2.zero;
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
