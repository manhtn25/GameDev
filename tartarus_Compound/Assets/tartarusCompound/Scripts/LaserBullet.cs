using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    Rigidbody2D rb;


    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.right * speed;
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("TilemappedLevel"))
        {
            Destroy(gameObject);
        }

      //collision with player and player enemy tag while in virtual
      //this is the part the movetowards transform should be when referencing the player


    }
}
