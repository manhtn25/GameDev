using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurret : MonoBehaviour
{
    private GameObject tempBullet;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject virtualBullet;
    float timebetween;
    public float startTimeBetweenBoss;
    private float bulletSpeed = 400f;

    public bool playerInTurretRangeBoss = false;

    private Animator anim;
    private CircleCollider2D coll;

    MainPlayerMovement death;
    PlayerLife respawnAfterDeath;

    private bool turretDead = false;
    [SerializeField] private PlayerLife deathCheck;

    private int Enemyhealth = 2;
    private bool isDamaged = false;
    private SpriteRenderer sprite;

    public GameObject[] itemDrop;
    private GameObject newInstance;
    private bool canDrop = true;

    private GameObject player;

    [SerializeField] private Interactables virtualCheck;

    [SerializeField] public AudioClip shoot;

    [SerializeField] private CerberusSwitches switchFirst;
    [SerializeField] private CerberusSwitches switchSecond;

    [SerializeField] private BossTurretRange switchChecks;

    void Start()
    {
        timebetween = startTimeBetweenBoss;
        anim = GetComponent<Animator>();
        coll = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        death = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerMovement>();
        respawnAfterDeath = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

 
        if (player == null)
        {
            return;
        }
        else
        {
            if (switchFirst.switchActive == true && switchSecond.switchActive == true)
            {
                switchChecks.turretRangeCol.enabled = false;
            }
            else if (switchFirst.switchActive == false || switchSecond.switchActive == false)
            {
                switchChecks.turretRangeCol.enabled = true;

                if (playerInTurretRangeBoss == true && turretDead == false)
                {
                    if (timebetween <= 0 && virtualCheck.inVirtual == false)
                    {
                        AudioSource.PlayClipAtPoint(shoot, transform.position);
                        tempBullet = Instantiate(bullet, firepoint.position, bullet.transform.rotation);
                        tempBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);

                        timebetween = startTimeBetweenBoss;
                    }
                   /* else if (timebetween <= 0 && virtualCheck.inVirtual == true)
                    {
                        AudioSource.PlayClipAtPoint(shoot, transform.position);
                        tempBullet = Instantiate(virtualBullet, firepoint.position, bullet.transform.rotation);
                        tempBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);

                        timebetween = startTimeBetweenBoss;
                    }*/
                    else
                    {
                        timebetween -= Time.deltaTime;
                    }
                }
            }
         
        }

        if (deathCheck.isDead == true)
        {
            EnemyDestroyed();
            canDrop = true;
        }

        if (turretDead == true)
        {

        }

    }

    public void TurretParticle()
    {
        coll.isTrigger = false;
        StartCoroutine(Destroy());


    }

    private IEnumerator Destroy()
    {

        //yield return new WaitForSeconds(2.0f);
        yield return new WaitForSeconds(0.1f);
        anim.Play("Turret_Explode");
        coll.enabled = false;
        // Destroy(this.gameObject, 0.30f);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
         {
             flyingAttack.Play();
             healthDmg.TakeDamage(1);
             *//* death.MainPlayerDeath();

              collision.GetComponent<MainPlayerMovement>().canMove = false;

              //call the playerlife function to respawn --> just invisible not runnign
              respawnAfterDeath.combackAlive();*//*

         }*/

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
                turretDead = true;
                ResetSprite();
                TurretParticle();
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
                turretDead = true;
                ResetSprite();
                TurretParticle();
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
        anim.Play("Turret_Idle");
        turretDead = false;
        Enemyhealth = 1;
        coll.enabled = true;
        coll.isTrigger = true;
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

            Destroy(newInstance, 3.0f);
        }

        canDrop = false;
    }
}
