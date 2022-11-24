using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMainEnemyScript : MonoBehaviour
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

    [SerializeField] Transform castPos;
    const string LEFT = "left";
    const string RIGHT = "right";
    public string facingDirectionNew;
    Vector3 baseScale;

    RaycastHit2D hitInfo;

    private bool staticIsDead = false;
    [SerializeField] private PlayerLife deathCheck;

    private int Enemyhealth = 3;
    private bool isDamaged = false;

    [SerializeField]
    private Interactables virtualCheck;
    [SerializeField]
    private Interactables virtualCheckTwo;

    public GameObject[] itemDrop;
    private GameObject newInstance;
    private bool canDrop = true;

    public AudioSource audioSource;

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

        exclamationPoint.SetActive(false);

        facingDirectionNew = RIGHT;
        
        baseScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();

       // this.GetComponent<WalkingComboScript>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (virtualCheck.inVirtual == true || virtualCheckTwo.inVirtual == true)
        {
            sprite.enabled = false;
            exclamationPoint.SetActive(false);
        }
        else
        {
            sprite.enabled = true;
        }

        //transform.Rotate(Vector3.forward * Time.deltaTime * speed);, useful for rotating vision 
        if (facingDirectionNew == RIGHT)
        {
            hitInfo = Physics2D.Raycast(transform.position, transform.right, attackRaycast, rayCastMask);
            punchDirection = true;
        
        }
        else if (facingDirectionNew == LEFT)
        {
            hitInfo = Physics2D.Raycast(transform.position, -transform.right, attackRaycast, rayCastMask);
            punchDirection = false;
       
        }

        if (staticIsDead == true)
        {
            exclamationPoint.SetActive(false);
        }



        //detecting the player
        if (hitInfo.collider != null && staticIsDead == false)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);

            if (hitInfo.collider.tag == "Player")
            {

                /* EnemyLogic();

                 characterDetected = true;
                 exclamationPoint.SetActive(true);*/

                
                this.GetComponent<WalkingComboScript>().enabled = true;
            }
         

        }
        //not detecting the player but just patroling
        else if (hitInfo.collider == null && staticIsDead == false)
        {
            // Debug.DrawLine(transform.position, transform.position + transform.right * visionDistance, Color.green);

            Debug.DrawLine(transform.position, transform.position + transform.right * visionDistance, Color.green);

            anim.SetBool("canRun", false);
            anim.SetBool("canWalk", true);
            StopAttack();

           // Debug.Log("Gone");

            //GetComponent<EnemyPatrol>().enabled = true;

            characterDetected = false;
            exclamationPoint.SetActive(false);
        }

        if (deathCheck.isDead == true)
        {
            EnemyDestroyed();
            canDrop = true;
           // this.GetComponent<WalkingComboScript>().enabled = false;
        }

    }

    private void FixedUpdate()
    {


        float adjustDirectionSpeed = walkSpeed;

        if (facingDirectionNew == LEFT)
        {
            adjustDirectionSpeed *= -1;
        }
   

         //allow the enemy to constantly move at a fixed speed when walking
        rb.velocity = new Vector2(adjustDirectionSpeed, rb.velocity.y);

        //not checking for a ground but just increasing raycast length
        if (IsHittingWall())
        {
            if (facingDirectionNew == LEFT)
            {
                ChangeEnemyDirection(RIGHT);
            }
            else
            {
                ChangeEnemyDirection(LEFT);
            }
        }
    }

    private void EnemyLogic()
    {

        enemyPlayerDistance = Vector2.Distance(transform.position, target.transform.position);

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

        //Patrol script need to turn it off somehow, but turn it back on when the script is done and make sure it does not go out of bounds

        //when colliding with the player it flips the other way --> bool to whether player is detected, but still not flaweless though

        //hitting the waypoints flipping the enemy --> work around now is just setting up farther waypoints

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack") && isDamaged == false)
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
            audioSource.UnPause();
        }
    }

    private void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("canAttack", false);
        audioSource.Pause();
    }

    private void Attack()
    {
        audioSource.UnPause();
        timer = intTimer;
        attackMode = true;

        anim.SetBool("canRun", false);
        anim.SetBool("canAttack", true);

        if (canDamage)
        {
            punchRight.SetActive(true);
            StartCoroutine("EnemyPunchWait", .02f);

        }

        /*  else if (facingDirection == LEFT && canDamage)
          {

              punchRight.SetActive(true);
             // StartCoroutine("EnemyPunchWait", .05f);
          }*/

        /*if (GetComponent<EnemyPatrol>().gotFlipped && punchDirection && canDamage)
        {
            punchLeft.SetActive(true);
            StartCoroutine("EnemyPunchWait", .05f);

        }
        else if (GetComponent<EnemyPatrol>().gotFlipped && !punchDirection && canDamage)
        {
            punchRight.SetActive(true);
            StartCoroutine("EnemyPunchWait", .05f);

        }
        else if (punchDirection && canDamage && !GetComponent<EnemyPatrol>().gotFlipped)
        {
            punchRight.SetActive(true);
            StartCoroutine("EnemyPunchWait", .05f);

        }
        else if (!punchDirection && canDamage && !GetComponent<EnemyPatrol>().gotFlipped)
        {
            punchLeft.SetActive(true);
            StartCoroutine("EnemyPunchWait", .05f);

        }*/
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
        
        audioSource.Pause();
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

    void ChangeEnemyDirection(string newDirection)
    {

        Vector3 newScale = baseScale;

        if (newDirection == LEFT)
        {
            newScale.x = baseScale.x * -1;
        }else if (newDirection == RIGHT)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;

        facingDirectionNew = newDirection;

    }

    private bool IsHittingWall()
    {
        bool val = false;

        float castDist = distance;
        //defining the raycast Distance for left and right
        if (facingDirectionNew == LEFT)
        {
            castDist = distance * -1;
        }
        else 
        {
            castDist = distance;
        }

        //determine the target destination based on the cast distance
        Vector3 mainTargetPos = castPos.position;
        mainTargetPos.x += castDist;

       Debug.DrawLine(castPos.position, mainTargetPos, Color.blue);

        if (Physics2D.Linecast(castPos.position, mainTargetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = true; //found a wall
  
        }
        else
        {
            val = false;
        }

        return val;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BulletRight(Clone)" || collision.gameObject.name == "BulletLeft(Clone)")
        {

            Destroy(collision.gameObject);

            Enemyhealth--;
            sprite.color = new Color32(255, 127, 127, 255);

            if (Enemyhealth <= 0)
            {
                if (canDrop)
                {
                    MovingItemDrop();
                }
               
      
                staticIsDead = true;
                ResetSprite();
                EnemyGuardParticle();
               
            }
            else
            {
                isDamaged = true;
                Invoke("ResetSprite", .15f);
            }
        }
        else if (collision.gameObject.name == "punchRight" || collision.gameObject.name == "punchLeft")
        {
            Enemyhealth -= 2;
            sprite.color = new Color32(255, 127, 127, 255);


            if (Enemyhealth <= -1)
            {
                if (canDrop)
                {
                    MovingItemDrop();
                }

                staticIsDead = true;
                ResetSprite();
                EnemyGuardParticle();
            }
            else
            {
                isDamaged = true;
                Invoke("ResetSprite", .15f);
            }
        }
    }

    private void ResetSprite()
    {
        sprite.color = new Color32(255, 255, 255, 255);
        isDamaged = false;
    }


    private void EnemyDestroyed()
    {

        //exclamationPoint.SetActive(false);
        Invoke("Respawn", .10f);
    }

    private void Respawn()
    {
        anim.Play("Enemy_Idle");
        staticIsDead = false;
        Enemyhealth = 3;
        

    }



    private void MovingItemDrop()
    {
        for (int i = 0; i < itemDrop.Length; i++)
        {
            if (i % 2 == 0)
            {
                newInstance = Instantiate(itemDrop[i], transform.position + new Vector3(-1, 1, 0), Quaternion.identity);
            }
            else
            {
                newInstance = Instantiate(itemDrop[i], transform.position + new Vector3(1, 1, 0), Quaternion.identity);
          
            }
           
            Destroy(newInstance, 5.0f);
        }

        canDrop = false;

    }


}
