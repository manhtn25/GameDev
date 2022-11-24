using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    Rigidbody2D rb;

    private GameObject player;

    public float Acceleration;
    public float RotationControl;

    float MovY, MovX = 1;



    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.right * speed;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

        MovY = Input.GetAxis("Vertical");

        
    }

    private void FixedUpdate()
    {
        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);


        Vector2 Direction = transform.position - player.transform.position;

        Direction.Normalize();

        float cross = Vector3.Cross(Direction, transform.right).z;

        rb.angularVelocity = cross * RotationControl;

        Vector2 Vel = transform.right * (MovX * Acceleration);
        rb.AddForce(Vel);

        float thrustForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.down)) * 2.0f;

        Vector2 relForce = Vector2.up * thrustForce;

        rb.AddForce(rb.GetRelativeVector(relForce));

        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
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
