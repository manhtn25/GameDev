using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlyingGuards : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] private GameObject exclamationPoint;
    private GameObject player;
    public bool chaseBoss = false;
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

    private int Enemyhealth = 1;
    private bool isDamaged = false;

    /*  [SerializeField]
      private Interactables virtualCheck;
      [SerializeField]
      private Interactables virtualCheckTwo;*/

    public AudioSource audioSourceFlyingBoss;

    public GameObject[] itemDrop;
    private GameObject newInstance;
    private bool canDrop = true;

    private Vector3 respawnPointFlying;




    //the idea is that once an flying enemy is killed it gets transformed at the starting position and turning of their range finder until some given time

    //this is the command to bring it back to starting pos   transform.position = respawnPoint;

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

        respawnPointFlying = transform.position;

    }

    private void Update()
    {

        /*if (virtualCheck.inVirtual == true || virtualCheckTwo.inVirtual == true)
        {
            sprite.enabled = false;
            exclamationPoint.SetActive(false);
        }
        else
        {
            sprite.enabled = true;
        }*/

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

            if (chaseBoss == true && flyingIsDead == false && isDamaged == false)
            {
                TargetPlayer();
                exclamationPoint.SetActive(true);
                audioSourceFlyingBoss.UnPause();
                //Debug.Log("Working");
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
            exclamationPoint.SetActive(false);
            EnemyDestroyed();
            coll.enabled = false;
            canDrop = true;

        }

        if (flyingIsDead == true)
        {
            /*exclamationPoint.SetActive(false);
            audioSourceFlying.Pause();*/
            exclamationPoint.SetActive(false);
            EnemyDestroyed();
            coll.enabled = false;
            canDrop = true;

        }


        anim.SetBool("IsFlying", true);

    }

    private void TargetPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void BackPatrolling()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPoint.position, 2 * speed * Time.deltaTime);
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

    public void FlyingEnemyGuardParticleBoss()
    {
        coll.isTrigger = false;
        exclamationPoint.SetActive(false);
        StartCoroutine(Destroy());


    }

    private IEnumerator Destroy()
    {

        //yield return new WaitForSeconds(2.0f);
        yield return new WaitForSeconds(0.1f);
        anim.Play("Explode_Animation");
        coll.enabled = false;
        exclamationPoint.SetActive(false);
        // Destroy(this.gameObject, 0.30f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            flyingAttack.Play();
            healthDmg.TakeDamage(1);
            coll.enabled = false;
            Invoke("ResetCollider", .40f);
            /* death.MainPlayerDeath();

             collision.GetComponent<MainPlayerMovement>().canMove = false;

             //call the playerlife function to respawn --> just invisible not runnign
             respawnAfterDeath.combackAlive();*/

        }
        else if (collision.CompareTag("VirtualPlayer"))
        {
            flyingAttack.Play();
            healthDmg.TakeDamage(1);
            coll.enabled = false;
            Invoke("ResetCollider", 1.5f);
        }

        if (collision.gameObject.name == "BulletRight(Clone)" || collision.gameObject.name == "BulletLeft(Clone)")
        {


            Destroy(collision.gameObject);

            Enemyhealth--;
            sprite.color = new Color32(255, 127, 127, 255);

            if (Enemyhealth <= 0)
            {

                if (canDrop)
                {
                    ItemDrop();
                }
                flyingIsDead = true;
                ResetSprite();
                FlyingEnemyGuardParticleBoss();
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
                    ItemDrop();
                }
                flyingIsDead = true;
                ResetSprite();
                FlyingEnemyGuardParticleBoss();
            }
            else
            {
                isDamaged = true;
                Invoke("ResetSprite", .15f);
            }
        }

    }

    private void ResetCollider()
    {
        coll.enabled = true;
    }
    private void ResetSprite()
    {
        sprite.color = new Color32(255, 255, 255, 255);
        isDamaged = false;
    }

    private void EnemyDestroyed()
    {

        //exclamationPoint.SetActive(false);
        FlyingEnemyGuardParticleBoss();
        Invoke("Respawn", 5.0f);
    }

    private void Respawn()
    {
        anim.Play("Idle_Animation");
        exclamationPoint.SetActive(false);
        flyingIsDead = false;
        Enemyhealth = 1;
        coll.enabled = true;
        coll.isTrigger = true;
        transform.position = respawnPointFlying;
    }

    private void ItemDrop()
    {
        // Instantiate(itemType, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

        for (int i = 0; i < itemDrop.Length; i++)
        {
            if (i % 2 == 0)
            {
                newInstance = Instantiate(itemDrop[i], transform.position + new Vector3(-2, 1, 0), Quaternion.identity);
            }
            else
            {
                newInstance = Instantiate(itemDrop[i], transform.position + new Vector3(-2, 1, 0), Quaternion.identity);

            }

            Destroy(newInstance, 5.0f);
        }

        canDrop = false;
    }
}
