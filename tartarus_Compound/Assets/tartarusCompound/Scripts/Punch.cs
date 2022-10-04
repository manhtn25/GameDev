using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Punch : MonoBehaviour
{
    public AudioClip punching;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            AudioSource.PlayClipAtPoint(punching, transform.position);
            Destroy(collision.gameObject);
           
        }
    }
}
