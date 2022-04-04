using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



enum ammoState { EMPTY, NORMAL, INCENDIARY}
// This determines if the shotgun is loaded and what type of ammo it has.
/// EMPTY: No ammo in the gun
/// NORMAL: Regular bullets. Nothing special
/// ...Anything else we think of
/// 
enum animState { RUN, FIRE, MELEE, HURT, GRAP }
/// This will be sent to setAnimation() as a parameter
/// In there, it'll flip the sprites and figure out which direction to face
/// 
public enum directionState { LEFT, RIGHT, UP, DOWN } //Tracking for melee code


public class player : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxShield;
    float curShield;
    float shieldRecharge;
    public AudioSource gunAudioSource;
    public AudioSource ammoAudioSource;
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

    //Animation stuff, don't worry about it
    public Animator myAnimator;
    public directionState direction = directionState.DOWN;
    public bool directionLock = false;

    //UI
    public GameObject hpBar;
    public GameObject sBar;

    //Melee Hitboxes
    public GameObject stabBoxUp;  //This is the hitbox for le stab
    public GameObject stabBoxDown;  //This is the hitbox for le down stab
    public GameObject stabBoxRight;  //Please I swear i know what im doing
    public GameObject stabBoxLeft;  //I promise this is the legitamate solution TRUST me



    SpriteRenderer mSpriteRenderer; // for testing only
    void Start()
    {
        shieldRecharge = 0.0f;
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        attacking = false;
        grappled = false;
        leftBarrel = ammoState.NORMAL;
        rightBarrel = ammoState.NORMAL;
        curHealth = maxHealth;
        curShield = maxShield / 2;
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        print(curHealth);
        if (curShield < maxShield)
        {
            if (shieldRecharge > 0)
            {
                shieldRecharge -= Time.deltaTime;
            }
            else
                curShield += 10 * Time.deltaTime;
        }
        else if (curShield > maxShield)
            curShield = maxShield;

        updateUI();
        do_input(hAxis, vAxis);

    }
    void updateUI()
    {
        //hpBar.GetComponent<Slider>().value = curHealth / maxHealth;  //Apparently this Doesn't work.
        //sBar.GetComponent<Slider>().value = curShield / maxShield;
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
            for(int i=0; i <= 10; i++)
            {
                
                float angleMult = 5 * i;
                GameObject c = Instantiate(bullet, transform.position, Quaternion.identity);
                c.transform.LookAt(aim, Vector3.forward);
                c.transform.Rotate(new Vector3(0, 0, -60-angleMult));
            }

            setAnimation(animState.FIRE);


            // Play shotgun sound
            gunAudioSource.Play();
            
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
                {
                    leftBarrel = ammoState.EMPTY;
                    
                    attacking = true;
                    attackCD = 0.4f;
                }
            }
            else
            {
                if (fire(rightBarrel))
                {
                    rightBarrel = ammoState.EMPTY;
                    attacking = true;
                    attackCD = 0.4f;
                }
            }
        }
        // melee input (Space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attacking = true;
            attackCD = 0.4f;
            //mSpriteRenderer.color = Color.blue; // color change is for testing
            setAnimation(animState.MELEE);

        }


        if (attacking || grappled)
        {
            speedMod = 0;
            if (attacking)
            {
                
                attackCD -= Time.deltaTime;
                if (attackCD <= 0)
                {
                    //mSpriteRenderer.color = Color.green; // color change is for testing
                    attacking = false;
                }
            }
        }
        else
        {
            speedMod = 1;
        }
        float actualSpeedx = horiz * moveSpeed * speedMod;
        float actualSpeedy = vert * moveSpeed * speedMod;
        body.velocity = new Vector3(actualSpeedx,actualSpeedy, 0);

        //There can only be ONE hurtbox active.  If we don't move, just use previous state...
        if (!attacking && !grappled && !directionLock)
        {
            if (body.velocity.y < -0.1)
                direction = directionState.DOWN;
            else if (body.velocity.y > 0.1)
                direction = directionState.UP;
            else if (body.velocity.x < -0.1)
                direction = directionState.LEFT;
            else if (body.velocity.x > 0.1)
                direction = directionState.RIGHT;
        }


        //Now that we know our velocity, the animator knows what direction we're headed
        if (speedMod == 1)
        {
            setAnimation(animState.RUN);
        }

    }
    //This just lets the animation change it to and from being stab time.
    public void stabEnable() //Apparently animation events cant do true false values...but they can do int
    {
        //Ugly, I know.
        print("detroit simulator...");
        switch(direction)
        {
            case directionState.DOWN:
                stabBoxDown.SetActive(!stabBoxDown.activeSelf);
                break;
            case directionState.UP:
                stabBoxUp.SetActive(!stabBoxUp.activeSelf);
                break;
            case directionState.RIGHT:
                stabBoxRight.SetActive(!stabBoxRight.activeSelf);
                break;
            case directionState.LEFT:
                stabBoxLeft.SetActive(!stabBoxLeft.activeSelf);
                break;
        }
        directionLock = !directionLock;
    }
    private void setAnimation(animState stat)
    {
        //Where are we going?  Feed the animator this info.

        myAnimator.SetFloat("yVel", body.velocity.y);
        myAnimator.SetFloat("xVel", body.velocity.x);



        if (body.velocity.x > 0)
        {
            mSpriteRenderer.flipX = false;
        }
        else if (body.velocity.x < 0)
        {
            mSpriteRenderer.flipX = true;
        }

        //Actually tell the animator what animation to play.  It'll use which way we're going + the trigger
        //To determine the final animation.
        switch(stat)
        {
            case animState.RUN:
                myAnimator.SetTrigger("Run");
                break;

            case animState.FIRE:
                myAnimator.SetTrigger("Fire");
                break;

            case animState.MELEE:
                myAnimator.SetTrigger("Melee");
                break;

            case animState.HURT:
                myAnimator.SetTrigger("Hurt");
                break;

            case animState.GRAP:
                myAnimator.SetTrigger("Grap");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("collision");
        switch (collision.tag)
        {
            case "ammo":
                print("Got to switch");
                if (leftBarrel != ammoState.EMPTY && rightBarrel != ammoState.EMPTY)
                    return;
                if (leftBarrel == ammoState.EMPTY)
                    leftBarrel = ammoState.NORMAL;
                if (rightBarrel == ammoState.EMPTY)
                    rightBarrel = ammoState.NORMAL;
                ammoAudioSource.Play();
                Destroy(collision.gameObject);
                break;
            case "R A T":
                curHealth -= 10;
                break;
            case "health":
                if (curHealth < maxHealth) 
                { 
                    curHealth = maxHealth;
                    // Play health pickup sound
                    Destroy(collision.gameObject);
                }
                break;
        }
    }
}
