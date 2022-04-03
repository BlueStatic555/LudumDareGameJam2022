using System.Collections;
using System.Collections.Generic;
using UnityEngine;



enum ammoState { EMPTY, NORMAL, INCENDIARY}
// This determines if the shotgun is loaded and what type of ammo it has.
/// EMPTY: No ammo in the gun
/// NORMAL: Regular bullets. Nothing special
/// ...Anything else we think of


public class player : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public float maxHealth;
    public GameObject bullet;
    public GameObject UI1;
    float curHealth;
    public float moveSpeed;
    public Rigidbody2D body;    
    bool attacking;             // if the player is attacking or not
    bool grappled;
    float attackCD;             // duration that the player is in the attacking state
    

    
    ammoState leftBarrel;
    ammoState rightBarrel;
    
    



    SpriteRenderer mSpriteRenderer; // for testing only
    void Start()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        attacking = false;
        grappled = false;
        leftBarrel = ammoState.NORMAL;
        rightBarrel = ammoState.NORMAL;
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        do_input(hAxis, vAxis);

    }

    bool fire(ammoState barrel)
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 aim = cam.ScreenToWorldPoint(mousePos);
        if (barrel == ammoState.EMPTY)
        {
            // play empty clicking noise

            // return false since no bullet was fired
            return false;
        }
        else if (barrel == ammoState.NORMAL)
        {
            // shoot normal projectiles
            for(int i=0; i <= 5; i++)
            {
                int angleMult = 5 * i;
                GameObject c = Instantiate(bullet, transform.position, Quaternion.identity);
                c.transform.LookAt(aim, Vector3.forward);
                c.transform.Rotate(new Vector3(0, 0, -75-angleMult));
            }
            /*
            GameObject c = Instantiate(bullet, transform.position, Quaternion.identity);
            c.transform.LookAt(aim, Vector3.forward);
            c.transform.Rotate(new Vector3(0, 0, -90));
            */

            // Play shotgun sound

            // return true since a bullet was fired
            return true;
            
        }
        return false;
    }

    void do_input(float horiz, float vert)
    {
        int speedMod = 1;
        // gun input
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (fire(leftBarrel))
                    leftBarrel = ammoState.EMPTY;
            }
            else
            {
                if (fire(rightBarrel))
                    rightBarrel = ammoState.EMPTY;
            }
        }
        // melee input (Space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attacking = true;
            attackCD = 0.4f;
            mSpriteRenderer.color = Color.blue; // color change is for testing
        }


        if (attacking || grappled)
        {
            speedMod = 0;
            if (attacking)
            {
                
                attackCD -= Time.deltaTime;
                if (attackCD <= 0)
                {
                    mSpriteRenderer.color = Color.green; // color change is for testing
                    attacking = false;
                }
            }
        }
        else
        {
            speedMod = 1;
        }
        body.velocity = new Vector3(horiz * moveSpeed * speedMod, vert * moveSpeed * speedMod, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("collision");
        switch(collision.tag)
        {
            case "ammo":
                print("Got to switch");
                if (leftBarrel != ammoState.EMPTY && rightBarrel != ammoState.EMPTY)
                    return;
                if (leftBarrel == ammoState.EMPTY)
                    leftBarrel = ammoState.NORMAL;
                if (rightBarrel == ammoState.EMPTY)
                    rightBarrel = ammoState.NORMAL;
                Destroy(collision.gameObject);
                break;
            case "R A T":
                curHealth -= 10;
                break;
        }
    }
}
