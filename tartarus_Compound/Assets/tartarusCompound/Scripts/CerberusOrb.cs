using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CerberusOrb : MonoBehaviour
{
    [SerializeField] private Interactables virtualCheck;
    private SpriteRenderer spriteOrb;
    private Animator anim;
    private BoxCollider2D coll;

    [SerializeField] private CerberusSwitches switchFirst;
    [SerializeField] private CerberusSwitches switchSecond;

    private int EnemyHealth = 1;
    private bool isDamaged = false;

    private bool mainOrbIsDead = false;

    [SerializeField] private PlayerLife deathCheck;

    public GameObject[] itemDrop;
    private bool canDrop = true;
    private GameObject newInstance;

    [SerializeField]
    private GameObject HealthBarObject;
    [SerializeField] private Slider HealthBar;

    private float FillSpeed = 0.068f;
    private float targetProgress = 0f;

    private void Start()
    {
        spriteOrb = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = true;
    }

    private void Update()
    {
        
     

        //both swtiches for now
        if (switchFirst.switchActive == false && switchSecond.switchActive == false)
        {
            if (virtualCheck.inVirtual == false)
            {
                coll.enabled = true;
                HealthBarObject.SetActive(true);
                HealthBar.enabled = enabled;

            }

            //play the opening gates animation
        }
        else
        {
            coll.enabled = false;
            HealthBarObject.SetActive(false);
            HealthBar.enabled = false;
        }

        if (deathCheck.isDead == true)
        {
            EnemyDestroyed();
           

        }
    }

    private void EnemyDestroyed()
    {

        //exclamationPoint.SetActive(false);
        Invoke("Respawn", .10f);
    }

    private void Respawn()
    {
        anim.Play("Idle_Animation");
        mainOrbIsDead = false;
        EnemyHealth = 1;
        coll.enabled = true;
        coll.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name == "BulletRight(Clone)" || collision.gameObject.name == "BulletLeft(Clone)")
        {


            Destroy(collision.gameObject);

            EnemyHealth--;
            spriteOrb.color = new Color32(255, 127, 127, 255);

            

            if (HealthBar.value < targetProgress)
            {

                if (canDrop)
                {
                    ItemDrop();
                }
                mainOrbIsDead = true;
                ResetSprite();
               // FlyingEnemyGuardParticle(); , can possibly just be exploding
            }
            else
            {
                

                HealthBar.value -= FillSpeed * Time.deltaTime;
                
                isDamaged = true;
                Invoke("ResetSprite", .15f);
            }
        }
        else if (collision.gameObject.name == "punchRight" || collision.gameObject.name == "punchLeft")
        {

            EnemyHealth -= 2;
            spriteOrb.color = new Color32(255, 127, 127, 255);

            if (HealthBar.value < targetProgress)
            {
                if (canDrop)
                {
                    ItemDrop();
                }
                mainOrbIsDead = true;
                ResetSprite();
               // FlyingEnemyGuardParticle(); can possibly jsut be exploding
            }
            else
            {
                HealthBar.value -= FillSpeed * Time.deltaTime;
                isDamaged = true;
                Invoke("ResetSprite", .15f);
            }
        }

    }

    private void ResetSprite()
    {
        spriteOrb.color = new Color32(255, 255, 255, 255);
        isDamaged = false;
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
