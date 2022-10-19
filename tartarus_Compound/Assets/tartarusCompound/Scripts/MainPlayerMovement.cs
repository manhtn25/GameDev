using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb; //caching the components; make sure to make it private and not expose it to use in other scripts
    private SpriteRenderer sprite;
    private Animator anim;


    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 10f; //serializedfield allows the edits of value in the editor
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private ForceMode ourForceMode;

    private int horizontalVal = 0; //this one sets up an integer whether player is walking or trying to run

    private int maxJumps = 1; //using int values lets you adjust later on whenever an item can give infinite jumps

    ShootingScript bulletAnim;
    public bool facingRight = true;

    PunchingDmg punchAnim;

    private bool canPunch = true;
    private bool isPunching;
    private float punchForwardPos = 10f;
    private float punchTime = .2f;
    private float punchCoolDown = 1f;
    //private int punchCount = 1;
    private float punchBetweenTime = 0f;
    public Image punchImage;
    [SerializeField] private GameObject coinUI;

    public bool canMove = true;


    [SerializeField] TrailRenderer dashTrail;

    [SerializeField] ParticleSystem punchTrailRight;
    [SerializeField] ParticleSystem punchTrailLeft;


    

    //public static PauseMenu instance;

    private enum MovementState { idle, running, jumping, falling, sneaking, shooting, punching } //this is basically an array, instead of having to remember the correct name, just refer to the its index position
    public AudioClip running, jump, land, prisonDoors;
    [SerializeField] private AudioSource runningSound;
    //[SerializeField] private AudioSource sneakingSound;
    [SerializeField] private AudioSource backgroundMusic;

    //data types int = 16, float = 4.45f, string = "bla", bool = true/false

    // Start is called before the first frame update
    private void Start() //methods should be private as well
    {
        rb = GetComponent<Rigidbody2D>(); //do this you can use the component
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); //animator component
   

        bulletAnim = GetComponent<ShootingScript>();
        punchAnim = GetComponent<PunchingDmg>();

        backgroundMusic.UnPause();
        AudioSource.PlayClipAtPoint(prisonDoors, transform.position);

        //runningSound.Stop();
        //numPunches = extraPunchVal;

    }


 

    // Update is called once per frame
    private void Update()
    {

        if (!PauseMenu.instance.isPaused)
        {
            coinUI.SetActive(true);

            Cursor.visible = false;

            //This actually has joystick support
            dirX = Input.GetAxisRaw("Horizontal"); //getAxis has slight decelaration, getAxisRaw tries to minimize it
            Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            /* rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
             horizontalVal = 1;*/

            if (isPunching)
            {
                return;
            }


            if (Input.GetKey(KeyCode.LeftShift))  //this one still needs to be setup for console for sneaking
            {

                rb.velocity = new Vector2(dirX * 3f, rb.velocity.y);
                horizontalVal = 0;
            }
            else if (Input.GetButtonUp("Fire1") && !Input.GetButton("Fire2") && movementVector != Vector2.zero)
            {
                if (Time.time > punchBetweenTime)
                {
                    punchImage.enabled = !punchImage.enabled;
                    StartCoroutine(PunchWait()); //set a dash punch
                    
                    punchBetweenTime = Time.time + 3.0f;
                    
                }

               
         
            }
            else if (Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
            {

                horizontalVal = 4;

                //this is for fighting
            }
            else if (Input.GetButton("Fire2") && IsGrounded() && Input.GetButton("Fire1"))
            {
                horizontalVal = 3;              //shooting in ground
                bulletAnim.Fire();
                rb.velocity = new Vector2(0, 0);


            }
            else if (Input.GetButton("Fire2") && !IsGrounded() && Input.GetButton("Fire1")) //mainly aiming right now and combine with fire one to shoot/ cause fire1 by itself is punch
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
                AudioSource.PlayClipAtPoint(jump, transform.position);
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0); //vector3(x, y, z), optional but can also use Vector2


            }

            //note that transition can be paused momentarily

            UpdateAnimationState(horizontalVal);

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
            sprite.flipX = false;
            facingRight = true;


            runningSound.GetComponent<AudioSource>().UnPause();

            //sneakingSound.Pause();


        }
        else if (dirX < 0f && horizontalVal == 1) //note that i Know you can change which direction the character is facing via sprite flipX
        {
            //anim.SetBool("running", true);//running left
            state = MovementState.running;
            sprite.flipX = true;
            facingRight = false;
            //Debug.Log(transform.right);


            runningSound.GetComponent<AudioSource>().UnPause();

            //sneakingSound.Pause();

        }
        else if (dirX > 0f && horizontalVal == 0)
        {
            state = MovementState.sneaking;
            sprite.flipX = false;
            runningSound.GetComponent<AudioSource>().Pause();

            //sneakingSound.UnPause();
           
        }
        else if (dirX < 0f && horizontalVal == 0)
        {
            state = MovementState.sneaking;
            sprite.flipX = true;
            runningSound.GetComponent<AudioSource>().Pause();
            //sneakingSound.UnPause();
          
        }
        else
        {
            /* anim.SetBool("running", false);*/
            state = MovementState.idle;
            runningSound.GetComponent<AudioSource>().Pause();
            //sneakingSound.Pause();
          
        }


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



        if (horizontalVal == 3)
        {
            state = MovementState.shooting;
            bulletAnim.Fire();
        }


        if (horizontalVal == 5)
        {
           // rb.velocity = new Vector3(50, 0, 0);

            rb.velocity = transform.right * punchForwardPos * Time.deltaTime;

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

           

        }


        anim.SetInteger("state", (int)state); //state value casts the integer representation

    }

    private bool IsGrounded()
    {
   
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        //creates another box similar to the size of the actual boxcollider, 0f is the rotation value, vector2.down + .1f moves the box a tiny bit down/ offsets it (overlaps it)
        
    }


    private IEnumerator PunchWait()
    {
        canPunch = false;
        isPunching = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (facingRight)
        {
            rb.velocity = new Vector2(transform.localScale.x * punchForwardPos, 0f);
            punchTrailRight.Play();
     
        }
        else
        {
            rb.velocity = new Vector2(-transform.localScale.x * punchForwardPos, 0f);
            punchTrailLeft.Play();
  
        }

       
        dashTrail.emitting = true;
        yield return new WaitForSeconds(punchTime);
        dashTrail.emitting = false;
        rb.gravityScale = originalGravity;
        isPunching = false;
        
        yield return new WaitForSeconds(punchCoolDown);
        canPunch = true;

        punchImage.enabled = !punchImage.enabled;

    }
   

    public void MainPlayerDeath()
    {
        anim.Play("Player_Death");
       

    }


    //with regards to shooting, are we planning to add more sprites when mc is constant shooting or just have single-fire


}
