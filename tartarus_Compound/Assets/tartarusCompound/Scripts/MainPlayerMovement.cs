using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb; //caching the components; make sure to make it private and not expose it to use in other scripts
    public SpriteRenderer spriteMainPlayer;
    private Animator anim;

    //set to public so interactables script can change


    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Interactables virtualCheck;
    [SerializeField] private Interactables virtualCheckTwo;
    [SerializeField] private Interactables virtualCheckThree;
    [SerializeField] private Interactables virtualCheckBoss;
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 10f; //serializedfield allows the edits of value in the editor
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private ForceMode ForceModeValue;
    [SerializeField] private PlayerLife deathCheck;

    private int horizontalVal = 0; //this one sets up an integer whether player is walking or trying to run

    private int maxJumps = 1; //using int values lets you adjust later on whenever an item can give infinite jumps

    ShootingScript bulletAnim;
    public bool facingRight = true;

    PunchingDmg punchAnim;

    public bool canPunch = true;
    private bool isPunching;
    private float DashForce = 15f;
    private float punchTime = .2f;
    private float punchCoolDown = 3f;
    //private int punchCount = 1;
    private float punchBetweenTime = 0f;
    private float dashBetweenTime = 0f;
    public Image dashImage;
    [SerializeField] private GameObject coinUI;
    private bool canDash = true;

    public bool canMove = true;


    [SerializeField] TrailRenderer dashTrail;

    [SerializeField] ParticleSystem punchTrailRight;
    [SerializeField] ParticleSystem punchTrailLeft;

    [SerializeField] ItemCollector gunCheck;

    [SerializeField] private Text bulletText;
    [SerializeField] private GameObject bulletUI;

    public bool isJumping = false;

    //public static PauseMenu instance;

    private enum MovementState { idle, running, jumping, falling, sneaking, shooting, punching } //this is basically an array, instead of having to remember the correct name, just refer to the its index position
    public AudioClip running, jump, land, prisonDoors, death;
    [SerializeField] private AudioSource runningSound;
    //[SerializeField] private AudioSource sneakingSound;
    [SerializeField] private AudioSource backgroundMusic;

    //data types int = 16, float = 4.45f, string = "bla", bool = true/false

    // Start is called before the first frame update
    private void Start() //methods should be private as well
    {
        rb = GetComponent<Rigidbody2D>(); //do this you can use the component
        spriteMainPlayer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); //animator component


        bulletAnim = GetComponent<ShootingScript>();
        punchAnim = GetComponent<PunchingDmg>();

        backgroundMusic.UnPause();
        AudioSource.PlayClipAtPoint(prisonDoors, transform.position);

        //runningSound.Stop();
        //numPunches = extraPunchVal;

        bulletUI.SetActive(false);
        bulletText.enabled = false;
    }




    // Update is called once per frame
    private void Update()
    {

        if (!PauseMenu.instance.isPaused)
        {


            /*    if (virtualCheck.inVirtual == false)
                {
                    sprite.color = new Color32(248, 248, 248, 255);
                }

                else if (virtualCheck.inVirtual == true)
                {
                    sprite.color = new Color32(67, 237, 255, 255);
                }*/


            coinUI.SetActive(true);

            Cursor.visible = false;

            //This actually has joystick support
            dirX = Input.GetAxisRaw("Horizontal"); //getAxis has slight decelaration, getAxisRaw tries to minimize it
            Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            /* rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
             horizontalVal = 1;*/

            /* if (isPunching)
             {
                 return;
             }*/

            if (gunCheck.hasGun == true)
            {
                bulletText.text = ": " + bulletAnim.currentBullets;
                bulletUI.SetActive(true);
                bulletText.enabled = true;
            }

            if (Input.GetKey(KeyCode.LeftShift))  //dashing move even if idle and moving
            {

                /*rb.velocity = new Vector2(dirX * 3f, rb.velocity.y);
                horizontalVal = 0;*/
                if (Time.time > dashBetweenTime)
                {
                    dashImage.enabled = false;
                    punchAnim.Punch();
                    StartCoroutine(DashWait()); //set a dash punch
                    dashBetweenTime = Time.time + 3.0f;

                }
            }
            /* else if (Input.GetButtonUp("Fire1") && !Input.GetButton("Fire2") && movementVector != Vector2.zero)
             {
             //dash punching only if moving
                 if (Time.time > punchBetweenTime)
                 {
                     punchImage.enabled = !punchImage.enabled;
                     //StartCoroutine(PunchWait()); //set a dash punch

                     punchBetweenTime = Time.time + 3.0f;
                 }
             }*/
            /*else if (Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
            {
                horizontalVal = 4;

                //this is for fighting/punching
            }
            else if (Input.GetButton("Fire2") && IsGrounded() && Input.GetButton("Fire1"))
            {
                if (virtualCheck.inVirtual == false && virtualCheckTwo.inVirtual == false)
                {
                    horizontalVal = 3;              //shooting in ground
                    bulletAnim.Fire();
                    rb.velocity = new Vector2(0, 0);
                }

            }
            else if (Input.GetButton("Fire2") && !IsGrounded() && Input.GetButton("Fire1")) //mainly aiming right now and combine with fire one to shoot/ cause fire1 by itself is punch
            {
                horizontalVal = 3;
                rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

            }*/

            else if (Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.K))
            {

                if (Time.time > punchBetweenTime)
                {
                    horizontalVal = 4;

                    punchBetweenTime = Time.time + .2f;

                }

            }
            else if (Input.GetKey(KeyCode.K) && IsGrounded())
            {
                if (virtualCheck.inVirtual == false && virtualCheckTwo.inVirtual == false && virtualCheckThree.inVirtual == false && gunCheck.hasGun == true && virtualCheckBoss.inVirtual == false)
                {
                    horizontalVal = 3;              //shooting in ground
                    bulletAnim.Fire();
                    rb.velocity = new Vector2(0, 0);

                }
            }
            else if (Input.GetKey(KeyCode.K) && !IsGrounded() && gunCheck.hasGun == true)
            {
                horizontalVal = 3;
                rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
            }
            else if (canMove)
            {

                rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
                horizontalVal = 1;

            }

            //when creating variables make sure to keep in the smallest scope, notice how dirX was not initialize similar to rb up top


            if (Input.GetButtonDown("Jump") && IsGrounded()) //using GetButtonDown is referring to the values in the input Manager

            //getkey has the effect of constantly adding velocity is a key is pressed 
            //getkeyDown only applies for a brief time --> note that both these types do not refer to the input manager in Unity but hard coded
            {
                /*            jumpSoundEffect.Play();
                */
                isJumping = true;
                AudioSource.PlayClipAtPoint(jump, transform.position, 0.3F);
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0); //vector3(x, y, z), optional but can also use Vector2


            }

            //note that transition can be paused momentarily

            if (canDash == true)
            {
                dashImage.enabled = true;
            }

            UpdateAnimationState(horizontalVal);

         /*   if (deathCheck.isDead == true)
            {
                //StopCoroutine(DashWait());
                canDash = true;
                dashBetweenTime = 0f;
                
            }*/

        }
        else
        {
            coinUI.SetActive(false);
            Cursor.visible = true;

        }



    }

    private void FixedUpdate()
    {
        if (isPunching)
        {
            return;
        }
    }



    private void UpdateAnimationState(int horizontalVal)
    {
        MovementState state;

        if (dirX > 0f && horizontalVal == 1)
        {
            //make sure spelling within animator is exact
            //anim.SetBool("running", true); //running right
            state = MovementState.running;
            spriteMainPlayer.flipX = false;
            facingRight = true;


            runningSound.GetComponent<AudioSource>().UnPause();

            //sneakingSound.Pause();


        }
        else if (dirX < 0f && horizontalVal == 1) //note that i Know you can change which direction the character is facing via sprite flipX
        {
            //anim.SetBool("running", true);//running left
            state = MovementState.running;
            spriteMainPlayer.flipX = true;
            facingRight = false;
            //Debug.Log(transform.right);


            runningSound.GetComponent<AudioSource>().UnPause();

            //sneakingSound.Pause();

        }
        /* else if (dirX > 0f && horizontalVal == 0)
         {
             state = MovementState.sneaking;
             spriteMainPlayer.flipX = false;
             runningSound.GetComponent<AudioSource>().Pause();

             //sneakingSound.UnPause();

         }
         else if (dirX < 0f && horizontalVal == 0)
         {
             state = MovementState.sneaking;
             spriteMainPlayer.flipX = true;
             runningSound.GetComponent<AudioSource>().Pause();
             //sneakingSound.UnPause();

         }*/
        else
        {
            /* anim.SetBool("running", false);*/
            state = MovementState.idle;
            runningSound.GetComponent<AudioSource>().Pause();
            //sneakingSound.Pause();

        }

        if (isJumping)
        {
            if (rb.velocity.y > .1f)
            {
                state = MovementState.jumping;
                runningSound.GetComponent<AudioSource>().Pause();
                //sneakingSound.Pause();

            }
            else if (rb.velocity.y > .1f && maxJumps > 0)
            {
                state = MovementState.running;
            }
            else if (rb.velocity.y < -.1f)
            {
                state = MovementState.falling;
                runningSound.GetComponent<AudioSource>().Pause();
                //sneakingSound.Pause();

            }
        }
        

        if (virtualCheck.inVirtual == false && virtualCheckTwo.inVirtual == false && virtualCheckThree.inVirtual == false && virtualCheck.inVirtual == false && virtualCheckBoss.inVirtual == false)
        {
            if (horizontalVal == 3)
            {
                state = MovementState.shooting;
                bulletAnim.Fire();
            }


            if (horizontalVal == 5)
            {
                // rb.velocity = new Vector3(50, 0, 0);

                rb.velocity = transform.right * DashForce * Time.deltaTime;

                // rb.AddForce(rb.velocity * punchForwardPos);     // this is actually teleports you;

                //rb.velocity = new Vector3(50, 0, 0);

            }

            else if (dirX > 0f && horizontalVal == 4 && facingRight && canPunch)
            {




                state = MovementState.punching;
                punchAnim.Punch();
                //rb.AddForce(transform.right * punchForwardPos);



                /* if (curPunch == numPunches)
               {
                   anim.SetBool("test", false);
                   StartCoroutine(PunchCooldown());
                   //Debug.Log("Success");
               }
               curPunch++;*/

            }
            else if (dirX < 0f && horizontalVal == 4 && !facingRight && canPunch)
            {


                state = MovementState.punching;
                punchAnim.Punch();
                //rb.AddForce(-(transform.right * punchForwardPos)); //this is for impulse mechanic
                //rb.velocity = new Vector2(0, 0);



            }
            else if (horizontalVal == 4)
            {

                //rb.velocity = new Vector2(0, 0);
                state = MovementState.punching;
                punchAnim.Punch();
                /*anim.SetTrigger("IdlePunch");
                punchAnim.Punch();
                Invoke("ResetPunch", 0.25f);*/

            }




        }

        anim.SetInteger("state", (int)state); //state value casts the integer representation

    }


    private void ResetPunch()
    {

        anim.Play("Player_Idle");
    }

    private bool IsGrounded()
    {

        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);

        //creates another box similar to the size of the actual boxcollider, 0f is the rotation value, vector2.down + .1f moves the box a tiny bit down/ offsets it (overlaps it)

    }


    private IEnumerator DashWait()
    {
        canDash = false;
        deathCheck.isInvincible = true;
        //isPunching = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (facingRight)
        {
            rb.velocity = new Vector2((transform.localScale.x) * DashForce, 0f);
           // punchTrailRight.Play();

        }
        else
        {
            rb.velocity = new Vector2(-(transform.localScale.x) * DashForce, 0f);
           // punchTrailLeft.Play();

        }


        dashTrail.emitting = true;
        yield return new WaitForSeconds(punchTime);
        dashTrail.emitting = false;
        rb.gravityScale = originalGravity;
        //isPunching = false;
        
        yield return new WaitForSeconds(punchCoolDown);
        deathCheck.isInvincible = false;
        canDash = true;



    }


    public void MainPlayerDeath()
    {
        anim.Play("Player_Death");
        AudioSource.PlayClipAtPoint(death, transform.position);

    }


    //with regards to shooting, are we planning to add more sprites when mc is constant shooting or just have single-fire


}
