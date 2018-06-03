using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubController : MonoBehaviour {

    public GameObject DashDirectionArrow;
    public int AttackDmg = 3;
    public float AttackCooldownTime = 5f, AttackRange = 5f, AttackTime = 0.2f, DashCollisionBuffer = 1f;

    private float attackCooldown;
    private bool dashing = false, attackHeld = false;
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

            if(Input.GetMouseButton(1) && attackCooldown == AttackCooldownTime && !dashing)
            {
                if (DashDirectionArrow)
                {
                    //pre attack
                    attackHeld = true;
                    DashDirectionArrow.SetActive(true);
                    DashDirectionArrow.transform.rotation = StaticHelper.LookAt2D(DashDirectionArrow.transform.position, Globals.Inst.MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition));
                }
                else
                    Debug.Log("Cub doesn't have a dash arrow sprite...");
            }
            else if (attackCooldown < AttackCooldownTime)
            {
                attackCooldown += Time.deltaTime;
                if (attackCooldown > AttackCooldownTime) attackCooldown = AttackCooldownTime;
            }
            else if (attackHeld)
            {
                attackHeld = false;
                DashDirectionArrow.SetActive(false);
                //attack
                StartCoroutine(DashInDirection());
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
            transform.position = Vector3.Lerp(transform.position, Globals.Inst.Player.transform.position - buffer, .03f);
        }
    }

   public IEnumerator DashInDirection()
    {
        Vector3 target = transform.position + DashDirectionArrow.transform.up * AttackRange;
        Vector3 originalPos = transform.position;
        float amt = 0f;
        while(transform.position != target)
        {
            amt += 1f / AttackTime * Time.deltaTime;

            Vector3 frameTarget = Vector3.Lerp(originalPos, target, amt);

            RaycastHit2D rh = Physics2D.Linecast(transform.position, frameTarget);
            if(rh)
            {
                transform.position = rh.point - (Vector2)DashDirectionArrow.transform.up * DashCollisionBuffer;
                break;
            }

            transform.position = frameTarget;
            yield return null;
        }
    }
}
