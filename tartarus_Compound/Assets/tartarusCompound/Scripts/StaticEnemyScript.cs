using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb; //caching the components; make sure to make it private and not expose it to use in other scripts
    private SpriteRenderer sprite;
    private Animator anim;
    private BoxCollider2D coll;

    //create isgrounded logic for this bot because he is falling out of the tilemap and also disable the patrol script
    //attach and enable follow script ai when detected by the flying orb object and enable an invisible wall that blocks the escape

    public float visionDistance;
    public LayerMask rayCastMask;
    public float timer;
    public Transform target;

    public float distance = .5f; //raycast length
    private float attackRaycast = 10f;
    public float attackDistance;
    public bool characterDetected = false;

    private float enemyPlayerDistance; //distance between enemy and player
    private bool cooling;
    private bool attackMode;
    private float intTimer;

    [SerializeField] private GameObject punchRight;
    [SerializeField] private GameObject punchLeft;

    [SerializeField] private GameObject exclamationPoint;

    private float walkSpeed = 3f;

    private bool punchDirection = true;
    private bool canDamage;

    //[SerializeField] Transform castPos;
    const string LEFT = "left";
    const string RIGHT = "right";
    private string facingDirection;


    private GameObject player;

    RaycastHit2D hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        intTimer = timer;

        rb = GetComponent<Rigidbody2D>(); //do this you can use the component
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); //animator component
        punchRight.SetActive(false);
        punchLeft.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        exclamationPoint.SetActive(false);

        facingDirection = RIGHT;
        
    }

    // Update is called once per frame
    void Update()
    {
        Flip();

        enemyPlayerDistance = Vector2.Distance(transform.position, target.transform.position);

        //transform.Rotate(Vector3.forward * Time.deltaTime * speed);, useful for rotating vision 
        if (facingDirection == RIGHT)
        {
 
            hitInfo = Physics2D.Raycast(transform.position, transform.right, attackRaycast, rayCastMask);
            punchDirection = true;
        }

        else if (facingDirection == LEFT)
        {

            //hitInfo = Physics2D.Raycast(transform.position, -transform.right, attackRaycast, rayCastMask);
            hitInfo = Physics2D.Raycast(transform.position, transform.right, attackRaycast, rayCastMask);
            punchDirection = false;


        }

        //whats wrong with this one is that once it flips the raycast and transform position does not work



        //detecting the player
        if (hitInfo.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);

            if (hitInfo.collider.tag == "Player")
            {

                EnemyLogic();
                characterDetected = true;
                exclamationPoint.SetActive(true);
                //Debug.Log("See");


            }


        }
        //not detecting the player but just patroling
        else if (hitInfo.collider == null)
        {
            // Debug.DrawLine(transform.position, transform.position + transform.right * visionDistance, Color.green);

            Debug.DrawLine(transform.position, transform.position + transform.right * visionDistance, Color.green);

            anim.SetBool("canRun", false);
            anim.SetBool("canWalk", false);
            StopAttack();

            characterDetected = false;
            exclamationPoint.SetActive(false);
        }

        

    }

    private void EnemyLogic()
    {

        

        if (enemyPlayerDistance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if (attackDistance >= enemyPlayerDistance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("canAttack", false); //basically a momentary idle animation
        }


    }

    private void Move()
    {
        anim.SetBool("canWalk", false);
        anim.SetBool("canRun", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
        {
            Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);

        }
        /*
                if (transform.position.x < target.position.x)
                {
                    rb.velocity = new Vector2(walkSpeed, 0);
                }
                else
                {
                    rb.velocity = new Vector2(-walkSpeed, 0);
                }
        */
    }

    private void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("canAttack", false);
    }

    private void Attack()
    {
        timer = intTimer;
        attackMode = true;

        anim.SetBool("canRun", false);
        anim.SetBool("canAttack", true);

        if (canDamage)
        {
            punchRight.SetActive(true);
            StartCoroutine("EnemyPunchWait", .02f);

        }

    }

    private void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    public void TriggerCooling() //set instance in the actual animation 
    {
        cooling = true;
    }

    public void EnemyGuardParticle()
    {
        anim.Play("Enemy_Vaporate");

    }

    public void TriggerAttackBox()
    {
        canDamage = true;
    }

    private IEnumerator EnemyPunchWait(float punchDelay)
    {
        yield return new WaitForSeconds(punchDelay);
        //punchLeft.SetActive(false);
        punchRight.SetActive(false);
        canDamage = false;


    }


    private void Flip() //this is just flipping the sprite
    {
        if (transform.position.x < target.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingDirection = RIGHT;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingDirection = LEFT;
        }
    }









}
