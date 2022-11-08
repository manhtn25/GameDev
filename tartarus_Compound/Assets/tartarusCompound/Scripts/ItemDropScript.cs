using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private int dropForce = 5;
    private BoxCollider2D col;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);
        col = GetComponent<BoxCollider2D>();
    }

    //need to freeze x and y and set the collider as a trigger

  /*  private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "TilemappedLevel" || collision.gameObject.tag == "Player")
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            

        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TilemappedLevel")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
