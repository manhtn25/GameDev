using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyPatrol : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] private GameObject exclamationPoint;
    private GameObject player;
    public bool chase = false;
    private bool dead = false;
    public Transform initialPoint;
    private Animator anim;

    private Rigidbody2D rb; //caching the components; make sure to make it private and not expose it to use in other scripts
    private SpriteRenderer sprite;
    private CircleCollider2D coll;

    [SerializeField] ParticleSystem flyingAttack;
    MainPlayerMovement death;
    PlayerLife respawnAfterDeath;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); //do this you can use the component
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<CircleCollider2D>();
        exclamationPoint.SetActive(false);

        flyingAttack.Stop();

        death = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerMovement>();
        respawnAfterDeath = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();

    }

    private void Update()
    {
        if(player == null)
        {
            return;
        }

        if (dead)
        {
          
           rb.velocity = Vector2.zero;
           
        }
        else
        {
            if (chase == true)
            {
                TargetPlayer();
                exclamationPoint.SetActive(true);
            }
            else
            {
                BackPatrolling();
                exclamationPoint.SetActive(false);
            }
            TargetPlayer();
            Flip();
        }


        anim.SetBool("IsFlying", true);

    }

    private void TargetPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void BackPatrolling()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPoint.position, speed * Time.deltaTime);
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void FlyingEnemyGuardParticle()
    {
        coll.isTrigger = false;
        dead = true;
        exclamationPoint.SetActive(false);
        StartCoroutine(Destroy());
        

    }

    private IEnumerator Destroy()
    {
       
        yield return new WaitForSeconds(2.0f);
        anim.Play("Explode_Animation");
        Destroy(this.gameObject, 0.30f);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            flyingAttack.Play();

            death.MainPlayerDeath();

            collision.GetComponent<MainPlayerMovement>().canMove = false;

            //call the playerlife function to respawn --> just invisible not runnign
            respawnAfterDeath.combackAlive();
        }

     
    }
}
