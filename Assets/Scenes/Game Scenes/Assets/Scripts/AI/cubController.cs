using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubController : MonoBehaviour {

    public GameObject player;
    public int AttackDmg = 3;
    public float AttackCooldownTime = 5f, AttackRange = 5f;

    private float attackCooldown;
    private bool dashing = false;
    private Vector3 buffer = new Vector3(-.1f, 2f, 0f);

	// Use this for initialization
	void Start () {
        attackCooldown = AttackCooldownTime;
	}

    public float moveSpeed;
    // Update is called once per frame
    void Update () {

        if (Globals.Inst.InputFocus == gameObject && !dashing)
        {
            if (Globals.DidPlayerSwitchThisFrame)
                Globals.DidPlayerSwitchThisFrame = false;
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Globals.DidPlayerSwitchThisFrame = true;
                Globals.Inst.InputFocus = Globals.Inst.Player.gameObject;
                Globals.Inst.MainCamera.GetComponent<CameraController>().followTarget = Globals.Inst.Player.gameObject;
                return;
            }

            if(Input.GetMouseButtonDown(1) && attackCooldown == AttackCooldownTime)
            {
                StartCoroutine(Dash());
            }
            else if (attackCooldown < AttackCooldownTime)
            {
                attackCooldown += Time.deltaTime;
                if (attackCooldown > AttackCooldownTime) attackCooldown = AttackCooldownTime;
            }

            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));   //(Devin) here is a cleaner way of doing player movement, I hope you don't mind...
            if (movement.magnitude > 0.1)
            {
                GetComponent<Rigidbody2D>().velocity = movement.normalized * moveSpeed;
            }
            else GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else //player is not controlling cub
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = Vector3.Lerp(transform.position, player.transform.position - buffer, .03f);
        }
    }

    public IEnumerator Dash()
    {
        dashing = true;
        Vector3 dir = (Globals.Inst.MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Vector3 target = transform.position + dir * AttackRange;
        target.z = 0;
        float timeSinceBeginDash = 0;
        timeSinceBeginDash += Time.deltaTime;
        while ((transform.position - target).magnitude > 0.2)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, target, 0.5f);
            RaycastHit2D rhh = Physics2D.Raycast(transform.position + dir, dir, AttackRange); //will break if boar is scaled, for now
            if (!rhh)
            {
                transform.position = newPos;
                float mag = (newPos - target).magnitude;
                Debug.Log("Mag: " + mag);
                if ( mag < 0.2)
                    break;
            }
            else
            {
                transform.position = new Vector3(rhh.point.x, rhh.point.y, 0) - transform.up;
                break;
            }
            yield return null;
        }
        dashing = false;
    }
}
