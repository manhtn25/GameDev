using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingComboScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb; //caching the components; make sure to make it private and not expose it to use in other scripts
    private SpriteRenderer Sprite;
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

    public GameObject righTempWall;
    public GameObject leftTempWall;
    //[SerializeField] private NewMainEnemyScript directionCheck;

    [SerializeField] private GameObject punchRight;
    [SerializeField] private GameObject punchLeft;

   // [SerializeField] private GameObject secondDamageCheck;

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

   

    // Start is called before the first frame update
    void Start()
    {
        intTimer = timer;

        rb = GetComponent<Rigidbody2D>(); //do this you can use the component
        Sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); //animator component
        punchRight.SetActive(false);
        punchLeft.SetActive(false);
        //secondDamageCheck.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        exclamationPoint.SetActive(false);

        facingDirection = RIGHT;


    }

    // Update is called once per frame
    private void Update()
    {

        Vector3 Temp = transform.localScale;
        Temp.x = 1;
        transform.localScale = Temp;

        if (virtualCheck.inVirtual == true || virtualCheckTwo.inVirtual == true)
        {
            Sprite.enabled = false;
            exclamationPoint.SetActive(false);
        }
        else
        {
            Sprite.enabled = true;
        }


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

                    EnemyLogic();
                    characterDetected = true;
                    exclamationPoint.SetActive(true);
                //Debug.Log("See");
                this.GetComponent<NewMainEnemyScript>().enabled = false;
                rb.velocity = Vector3.zero;
            }


            }
            //not detecting the player but just patroling
            else if (hitInfo.collider == null && staticIsDead == false)
            {
            // Debug.DrawLine(transform.position, transform.position + transform.right * visionDistance, Color.green);

            Debug.DrawLine(transform.position, transform.position + transform.right * visionDistance, Color.green);

            anim.SetBool("canRun", false);
            anim.SetBool("canWalk", false);
            StopAttack();

            characterDetected = false;
            exclamationPoint.SetActive(false);


            //this.GetComponent<WalkingComboScript>().enabled = false;
        }
        
       


        if (deathCheck.isDead == true)
        {
           // this.GetComponent<NewMainEnemyScript>().enabled = true;
            EnemyDestroyed();
            canDrop = true;
           

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

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack") && isDamaged == false)
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

          /*  if (facingDirection == RIGHT)
            {
                secondDamageCheck.SetActive(true);
            }*/

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
        //secondDamageCheck.SetActive(false);
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BulletRight(Clone)" || collision.gameObject.name == "BulletLeft(Clone)")
        {
            Destroy(collision.gameObject);

            Enemyhealth--;
            Sprite.color = new Color32(255, 127, 127, 255);

            if (Enemyhealth <= 0)
            {
                if (canDrop)
                {
                    ItemDrop();
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
            Sprite.color = new Color32(255, 127, 127, 255);


            if (Enemyhealth <= -1)
            {
                if (canDrop)
                {
                    ItemDrop();
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
        Sprite.color = new Color32(255, 255, 255, 255);
        isDamaged = false;
    }


    private void EnemyDestroyed()
    {

/*
        if (facingDirection == LEFT)
        {

            this.GetComponent<NewMainEnemyScript>().facingDirectionNew = LEFT;

        }
        else
        {
            this.GetComponent<NewMainEnemyScript>().facingDirectionNew = RIGHT;

        }*/
        //exclamationPoint.SetActive(false);

     
        //righTempWall.SetActive(true);

        Invoke("Respawn", .02f);
    }

    private void Respawn()
    {
      
        anim.Play("Enemy_Idle");
        staticIsDead = false;
        Enemyhealth = 3;
       // righTempWall.SetActive(false);
       // leftTempWall.SetActive(false);

       // this.GetComponent<WalkingComboScript>().enabled = false;
    }

    private void ItemDrop()
    {
        // Instantiate(itemType, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
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
