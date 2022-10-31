using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyPatrol : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] private GameObject exclamationPoint;
    private GameObject player;
    public bool chase = false;
    public Transform initialPoint;
    private Animator anim;

    private Rigidbody2D rb; //caching the components; make sure to make it private and not expose it to use in other scripts
    private SpriteRenderer sprite;
    private CircleCollider2D coll;

    [SerializeField] ParticleSystem flyingAttack;
    MainPlayerMovement death;
    PlayerLife respawnAfterDeath;

    PlayerLife healthDmg;

    private bool flyingIsDead = false;
    [SerializeField] private PlayerLife deathCheck;

    private int Enemyhealth = 3;
    private bool isDamaged = false;

    [SerializeField]
    private Interactables virtualCheck;
    [SerializeField]
    private Interactables virtualCheckTwo;

    public AudioSource audioSource; 

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
        healthDmg = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
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

        if (player == null)
        {
            return;
        }

        if (flyingIsDead)
        {
          
           rb.velocity = Vector2.zero;
           
        }
        else
        {
            if (chase == true && flyingIsDead == false && isDamaged == false)
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

        if (deathCheck.isDead == true)
        {
            EnemyDestroyed();
        }

        if (flyingIsDead == true)
        {
            exclamationPoint.SetActive(false);
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
        exclamationPoint.SetActive(false);
        StartCoroutine(Destroy());
        

    }

    private IEnumerator Destroy()
    {
       
        yield return new WaitForSeconds(2.0f);
        anim.Play("Explode_Animation");
       // Destroy(this.gameObject, 0.30f);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            flyingAttack.Play();
            healthDmg.TakeDamage(1);
            /* death.MainPlayerDeath();

             collision.GetComponent<MainPlayerMovement>().canMove = false;

             //call the playerlife function to respawn --> just invisible not runnign
             respawnAfterDeath.combackAlive();*/

        }

        if (collision.gameObject.name == "BulletRight(Clone)" || collision.gameObject.name == "BulletLeft(Clone)" )
        {
          

            Destroy(collision.gameObject);

            Enemyhealth--;
            sprite.color = new Color32(255, 127, 127, 255);

            if (Enemyhealth <= 0)
            {
                flyingIsDead = true;
                ResetSprite();
                FlyingEnemyGuardParticle();
            }
            else
            {
                isDamaged = true;
                Invoke("ResetSprite", .15f);
            }
        }
        else if (collision.gameObject.name == "punchRight" || collision.gameObject.name == "punchLeft")
        {
        
            Enemyhealth--;
            sprite.color = new Color32(255, 127, 127, 255);

            if (Enemyhealth <= 0)
            {
                flyingIsDead = true;
                ResetSprite();
                FlyingEnemyGuardParticle();
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
        anim.Play("Idle_Animation");
        flyingIsDead = false;
        Enemyhealth = 3;
    }
}
