using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyPatrol : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject[] points;

    private Rigidbody2D myRigidbody;
    private BoxCollider2D myBoxCollider;


    //best to utilize platforms with edges to make instead of one continous patrol route

    //there should be friendly fire when laser are shot

    //utilize istrigger points

    //trigger all enemies to attack within area to attack

    //patrol is still buggy in that its switching y, instead of x how its facing

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (IsFacingRight())
        {
            myRigidbody.velocity = new Vector2(0f, moveSpeed);
            //myRigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            myRigidbody.velocity = new Vector2(0f, -moveSpeed);
            //myRigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }

    }

    private bool IsFacingRight()
    {
        return transform.localScale.y > Mathf.Epsilon;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), transform.localScale.y);

        transform.localScale = new Vector2( transform.localScale.x, -(Mathf.Sign(myRigidbody.velocity.y)));
    }




}
