using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingPlatform : MonoBehaviour
{
    private float fallDelay = .75f;
    private float destroyDelay = 2f;

    private Vector3 platformRespawn;

    private Rigidbody2D rb;

    private bool falling;
    private Coroutine fallingCoroutine;

    //couple improvements, not being able to jump from underneath platform?, instead of just respawning have it move back up??


    private void Start()
    {
        falling = false;
        rb = GetComponent<Rigidbody2D>();
        platformRespawn = transform.position;
    }

    private void Update()
    {
        if (falling == true)
        {
            StopCoroutine(fallingCoroutine); //Note coroutine did stop, but there was still forces acting on the object thats why it kept falling
            rb.velocity = Vector3.zero;
            falling = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && falling == false)
        {
            fallingCoroutine = StartCoroutine(Fall());
            
        }else if (collision.gameObject.CompareTag("VirtualPlayer") && falling == false)
        {
            fallingCoroutine = StartCoroutine(Fall());
        }
  
       
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic; //applies gravity when falling

        //Destroy(gameObject, destroyDelay); //comment this out to have platform respawn back
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.gameObject.CompareTag("RespawnPlatforms")) //assign a game object with this properyty
        {
            Invoke("FallingPlatformTimer", 3);
            
        }
        
    }

    private void FallingPlatformTimer()
    {
        transform.position = platformRespawn;
        rb.bodyType = RigidbodyType2D.Kinematic;
        falling = true;
    }
}
