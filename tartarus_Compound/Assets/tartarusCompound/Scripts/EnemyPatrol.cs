using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject[] points;
    private int curPoints;
    PlayerLife healthDmg;

    private Rigidbody2D myRigidbody;
    private BoxCollider2D myBoxCollider;
    private Animator anim;


    public bool enemyFacingRight = true;
    public bool gotFlipped = false;

    //stop collider is what is making turn the character, but I need to make sure that if the character gets detected turn of the patrol boxes so enemies attack him

    // there should be friendly fire when laser are shot

    //utilize istrigger points

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsFacingRight())
        {
            myRigidbody.velocity = new Vector2(moveSpeed, 0f);
            enemyFacingRight = true;
        }
        else
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, 0f);
            enemyFacingRight = false;
        }

    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {

        if (GetComponent<MainEnemyGuard>().characterDetected)
        {
            //doesn't flip the sprite so he keeps on attacking in same direction
            transform.localScale = new Vector2((Mathf.Sign(myRigidbody.velocity.x)), transform.localScale.y);
            gotFlipped = false;

            if (collision.CompareTag("Player"))
            {
                //GetComponent<MainEnemyGuard>().characterDetected = false;

                StartCoroutine(FlipWait());
                gotFlipped = true;

            }

            //flip the sprite when the player passes or jumps over enemy

        }
        else if (!GetComponent<MainEnemyGuard>().characterDetected)
        {

            //this flips the sprite to face left
            transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), transform.localScale.y);

        }

    }

    private IEnumerator FlipWait()
    {
        yield return new WaitForSeconds(0.40f);
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), transform.localScale.y);
    }
    /*
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //GetComponent<MainEnemyGuard>().characterDetected = false;


            }
        }*/




}
