using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb; //caching the components; make sure to make it private and not expose it to use in other scripts
    private SpriteRenderer sprite;
    private Animator anim;

    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f; //serializedfield allows the edits of value in the editor
    [SerializeField] private float jumpForce = 20f;

    private int horizontalVal = 0; //this one sets up an integer whether player is walking or trying to run

    private int maxJumps = 1; //using int values lets you adjust later on whenever an item can give infinite jumps

    ShootingScript bulletAnim;
    public bool facingRight = true;

    PunchingDmg punchAnim;

    private enum MovementState { idle, running, jumping, falling, sneaking, shooting, punching } //this is basically an array, instead of having to remember the correct name, just refer to the its index position

    [SerializeField] private AudioSource jumpSoundEffect;

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
    }

    // Update is called once per frame
    private void Update()
    {
        //This actually has joystick support
        dirX = Input.GetAxisRaw("Horizontal"); //getAxis has slight decelaration, getAxisRaw tries to minimize it

        /* rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
         horizontalVal = 1;*/


        if (Input.GetKey(KeyCode.LeftShift))  //this one still needs to be setup for console for sneaking
        {

            rb.velocity = new Vector2(dirX * 3f, rb.velocity.y);
            horizontalVal = 0;
        }
        else if (Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
        {
            horizontalVal = 4;              //this is for fighting
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
        else
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
*/          rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0); //vector3(x, y, z), optional but can also use Vector2
                       
 
        }

        //note that transition can be paused momentarily

        UpdateAnimationState(horizontalVal);


        
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

        }
        else if (dirX < 0f && horizontalVal == 1) //note that i Know you can change which direction the character is facing via sprite flipX
        {
            //anim.SetBool("running", true);//running left
            state = MovementState.running;
            sprite.flipX = true;
            facingRight = false;
        }
        else if (dirX > 0f && horizontalVal == 0)
        {
            state = MovementState.sneaking;
            sprite.flipX = false;
        }
        else if (dirX < 0f && horizontalVal == 0)
        {
            state = MovementState.sneaking;
            sprite.flipX = true;
        }
        else
        {
            /* anim.SetBool("running", false);*/
            state = MovementState.idle;
        }


        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y > .1f && maxJumps > 0)
        {
            state = MovementState.running;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }



        if (horizontalVal == 3)
        {
            state = MovementState.shooting;
            bulletAnim.Fire();
        }

        if (horizontalVal == 4)
        {
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



    //with regards to shooting, are we planning to add more sprites when mc is constant shooting or just have single-fire



}
