using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
   /* private Animator anim;*/
    private Rigidbody2D rb;

    private Vector3 respawnPoint; //records player position at the start of the game

    private bool hasFallen = false;
    private float deathTime = 0.30f;

    private SpriteRenderer mainPlayer;
    private Animator anim;


    [SerializeField] ObjectiveReached objectiveFlag; //different way to access another object from a script

    [SerializeField] private AudioClip deathSound;

    [SerializeField] ObjectiveReached endingFlag;

    [SerializeField] private Interactables virtualCheck;

    public Image healthOne;
    public Image healthTwo;
    public Image healthThree;

    public bool isDead = false;

    /*    [SerializeField] private AudioSource deathSoundEffect;
    */

    public int maxHealth = 3;
    public int currentHealth;
    MainPlayerMovement death;

    private bool isInvincible = false;
    private float InvincibleDuration = 2.0f;
    private float InvincibleTimeAdd = 0.15f;

    // Start is called before the first frame update
    private void Start()
    {
        /*anim = GetComponent<Animator>();*/
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        mainPlayer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        death = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerMovement>();


    }

    private void Update()
    {
        
     if (Input.GetKeyDown(KeyCode.H)) //for geting stuck
        {
            StartCoroutine(Respawn());
        }
      
     if (currentHealth == 3)
        {
            healthOne.enabled = true;
            healthTwo.enabled = true;
            healthThree.enabled = true;
        }
     else if(currentHealth == 2)
        {
            healthThree.enabled = false;
        }
     else if(currentHealth == 1)
        {
            healthTwo.enabled = false;
        }
     else if (currentHealth == 0)
        {
            healthOne.enabled = false;
        }

        if (isDead == true)
        {
            virtualCheck.inVirtual = false;
        }

        if (virtualCheck.inVirtual == true)
        {
            mainPlayer.color = new Color32(67, 237, 255, 255);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //distinguish between traps and etc. by adding tags

        if (collision.gameObject.CompareTag("LaserTrap") || collision.gameObject.CompareTag("ElectricTrap"))
        {
            anim.Play("Player_Explode");
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            isDead = true;
            StartCoroutine(Respawn());
        }
        else if (collision.gameObject.CompareTag("WorldFall"))
        {
            //just disable the sprite renderer and then enable it, cause disabling object stops all the coroutine and play the explode anim
            anim.Play("Player_Explode");
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            isDead = true;
            StartCoroutine(Respawn());

        }

        else if (collision.gameObject.CompareTag("OutofBounds"))
        {
            //StartCoroutine("Respawn", 5f);
           
        }
    }

    private void Die()
    {
/*        anim.SetTrigger("death");
*/        rb.bodyType = RigidbodyType2D.Static;
    }

   /* private void RestartLevel()
    {
       *//* SceneManager.LoadScene(SceneManager.GetActiveScene().name);*//* //this is a complete reset of the level to the beginning
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            if (collision.gameObject.name == "checkpointOne")
            {
                respawnPoint = transform.position;
                objectiveFlag.CheckPointAnim();

            }else if (collision.gameObject.name == "checkpointOneFinal")
            {
                respawnPoint = transform.position;
                endingFlag.CheckPointAnim();
            }else 
            {
                respawnPoint = transform.position;
            }
            
           


        }
        /*else if (collision.tag == "Hole")
        {
            transform.position = respawnPoint;
        }*/

        //consider ienumerating the timer in between respawns
    }

    private IEnumerator Respawn()
    {

        //mainPlayer.enabled = false;
        yield return new WaitForSeconds(deathTime);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        anim.Play("Player_Idle");
        isDead = false;
        transform.position = respawnPoint;
        //mainPlayer.enabled = true;
        //Debug.Log("Success");
        currentHealth = maxHealth;
        
    }

    public void combackAlive()
    {
        StartCoroutine(EnemyPhysicalDeath());
    }

    private IEnumerator EnemyPhysicalDeath()
    {

        //mainPlayer.enabled = false;
        yield return new WaitForSeconds(1.25f);
        anim.Play("Player_Idle");
        isDead = false;
        transform.position = respawnPoint;
        GetComponent<MainPlayerMovement>().canMove = true;
        currentHealth = maxHealth;
       
        //mainPlayer.enabled = true;
    }

    public void TakeDamage(int amount)
    {

        if (virtualCheck.inVirtual == false)
        {
            if (isInvincible) return;

            currentHealth -= amount;
            mainPlayer.color = new Color32(255, 127, 127, 255);
            //subtract the hearts

            if (currentHealth <= 0)
            {
                death.MainPlayerDeath();
                isDead = true;
                combackAlive();
                GetComponent<MainPlayerMovement>().canMove = false;

                //enable all the hearts again
                //make sure to add where the player can't move

            }
            else
            {
                Invoke("ResetSprite", .10f);
                StartCoroutine(ActivateInvincibility());
            }
        }

       
    }

    private void ResetSprite()
    {
        mainPlayer.color = new Color32(255, 255, 255, 255);
 
  
    }

    private void GainHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    

    private IEnumerator ActivateInvincibility()
    {
        //yield return new WaitForSeconds(1.65f);
       
        isInvincible = true;
        int count = 0;

        for (float i = 0; i < InvincibleDuration; i += InvincibleTimeAdd)
        {
            if (count % 2 == 0)
            {
                mainPlayer.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                mainPlayer.color = new Color32(198, 198, 198, 255);
            }
            count++;
            yield return new WaitForSeconds(InvincibleTimeAdd);
        }

        isInvincible = false;
        mainPlayer.color = new Color32(255, 255, 255, 255);

    }

}
