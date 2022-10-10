using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEnemyGuard : MonoBehaviour
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

    public float distance = 17f; //raycast length
    public float attackDistance;
    public bool characterDetected = true;

    private float enemyPlayerDistance; //distance between enemy and player
    private bool cooling;
    private bool attackMode;
    private float intTimer;

    [SerializeField] private GameObject punchRight;
    [SerializeField] private GameObject punchLeft;

    [SerializeField] private GameObject exclamationPoint;

    private bool punchDirection = true;
    private bool canDamage;

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

        exclamationPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        //transform.Rotate(Vector3.forward * Time.deltaTime * speed);, useful for rotating vision 
        if (GetComponent<EnemyPatrol>().enemyFacingRight)
        {
            hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, rayCastMask);
            punchDirection = true;
        }
        else if (!GetComponent<EnemyPatrol>().enemyFacingRight)
        {
            hitInfo = Physics2D.Raycast(transform.position, transform.right, -distance, rayCastMask);
            punchDirection = false;
           
        }
        
        
       
        //detecting the player
        if (hitInfo.collider != null)
        {
         Debug.DrawLine(transform.position, hitInfo.point, Color.red);

            if (hitInfo.collider.tag == "Player")
            {

               EnemyLogic();
               //GetComponent<EnemyPatrol>().enabled = false;
                //characterDetected = true;
                characterDetected = true;
                exclamationPoint.SetActive(true);

            }

        }
        //not detecting the player but just patroling
        else if (hitInfo.collider == null)
        {
         Debug.DrawLine(transform.position, transform.position + transform.right * visionDistance, Color.green);
            anim.SetBool("canRun", false);
            anim.SetBool("canWalk", true);
            StopAttack();
            //GetComponent<EnemyPatrol>().enabled = true;
            characterDetected = false;
            exclamationPoint.SetActive(false);
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

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
        }
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

        if (GetComponent<EnemyPatrol>().gotFlipped && punchDirection && canDamage)
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
        punchRight.SetActive(false);
        punchLeft.SetActive(false);
        canDamage = false;


    }
}
