using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    NewMainEnemyScript explode;
    FlyingEnemyPatrol explodeTwo;

    private void Start()
    {
        //explode = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NewMainEnemyScript>();
   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            explode = collision.transform.gameObject.GetComponent<NewMainEnemyScript>();
            explode.EnemyGuardParticle();
            Destroy(collision.gameObject, .10f);
            
            Destroy(gameObject); //destroys bullet when hitting enemy
        }

        else if (collision.CompareTag("FlyingEnemy"))
        {
            explodeTwo = collision.transform.gameObject.GetComponent<FlyingEnemyPatrol>();
            explodeTwo.FlyingEnemyGuardParticle();
            //Destroy(collision.gameObject, .10f);

            Destroy(gameObject); //destroys bullet when hitting enemy

        }

        else if (collision.CompareTag("TilemappedLevel"))
        {
            Destroy(gameObject);
        }
    }
}
