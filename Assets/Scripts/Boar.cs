using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : CombatEntity
{

    public Sprite DashWarningSprite;
    public int DashUnits = 5;
    public float DashCooldown = 3, DashSpeed = 1, PreDashTime = 1;
    public bool IsDashingOrAboutToDash = false;

    // Use this for initialization
    private float _timeSinceLastDash = 0;
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsDashingOrAboutToDash)
        {
            transform.rotation = StaticHelper.LookAt2D(transform.position, Globals.Inst.Player.transform.position);
            _timeSinceLastDash += Time.deltaTime;

            if (_timeSinceLastDash > DashCooldown)
            {
                IsDashingOrAboutToDash = true;
                StartCoroutine(Dash());
                _timeSinceLastDash = 0;
            }
        }
    }

    public IEnumerator Dash()
    {
        //warning
        List<GameObject> AlertIcons = new List<GameObject>();
        for (int i = 0; i < DashUnits; i++)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            go.GetComponent<SpriteRenderer>().sprite = DashWarningSprite;
            go.transform.position = transform.position + transform.up * (i + 1);
            AlertIcons.Add(go);
        }
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject go in AlertIcons)
            Destroy(go);
        //dashing
        Vector3 target = transform.position + transform.up * DashUnits;
        target.z = 0;
        float timeSinceBeginDash = 0;
        timeSinceBeginDash += Time.deltaTime;
        while ((transform.position - target).magnitude > 0.01)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, target, 0.5f);
            RaycastHit2D rhh = Physics2D.Raycast(transform.position + transform.up, transform.forward, DashUnits); //will break if boar is scaled, for now
            if (!rhh)
            {
                transform.position = newPos;
            }
            else
            {
                transform.position = new Vector3(rhh.point.x, rhh.point.y, 0) - transform.up;
                Debug.Log("Boar Hit: ");
                break;
            }
            yield return null;
        }
        IsDashingOrAboutToDash = false;
        yield return null;
    }
}