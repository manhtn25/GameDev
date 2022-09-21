using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
   /* private Animator anim;*/
    private Rigidbody2D rb;

/*    [SerializeField] private AudioSource deathSoundEffect;
*/
    // Start is called before the first frame update
    private void Start()
    {
        /*anim = GetComponent<Animator>();*/
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //distinguish between traps and etc. by adding tags

        if (collision.gameObject.CompareTag("Trap"))
        {
            /*deathSoundEffect.Play();*/
/*            Die();
*/            RestartLevel();
        }
    }

    private void Die()
    {
/*        anim.SetTrigger("death");
*/        rb.bodyType = RigidbodyType2D.Static;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
