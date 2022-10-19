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

    public Image healthOne;
    public Image healthTwo;
    public Image healthThree;

    /*    [SerializeField] private AudioSource deathSoundEffect;
    */

    public int maxHealth = 3;
    public int currentHealth;
    MainPlayerMovement death;

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

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //distinguish between traps and etc. by adding tags

        if (collision.gameObject.CompareTag("Trap"))
        {
            /*deathSoundEffect.Play();*/
            /*            Die();
            */            //RestartLevel();

           // transform.position = respawnPoint;
        }
        else if (collision.gameObject.CompareTag("WorldFall"))
        {
            //just disable the sprite renderer and then enable it, cause disabling object stops all the coroutine and play the explode anim
            anim.Play("Player_Explode");
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
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
        anim.Play("Player_Idle");
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
        transform.position = respawnPoint;
        GetComponent<MainPlayerMovement>().canMove = true;
        currentHealth = maxHealth;
        //mainPlayer.enabled = true;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        //subtract the hearts

        if (currentHealth <= 0)
        {
            death.MainPlayerDeath();
            combackAlive();
            GetComponent<MainPlayerMovement>().canMove = false;
            

            //enable all the hearts again
            //make sure to add where the player can't move

        }
    }

    private void GainHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}
