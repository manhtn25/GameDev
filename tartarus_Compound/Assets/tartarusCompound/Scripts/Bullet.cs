using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    NewMainEnemyScript explode;
    FlyingEnemyPatrol explodeTwo;
    //StaticEnemyScript explodeStatic;

    //[SerializeField] private GameObject[] enemyPrefabs;

    private Vector3 respawnEnemyPos;

    private void Start()
    {
        //make sure the turret only has a range when it starts firing
   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("TilemappedLevel"))
        {
            Destroy(gameObject);
        }

      /*  if (collision.CompareTag("Enemy"))
        {
            explode = collision.transform.gameObject.GetComponent<NewMainEnemyScript>();
            explode.EnemyGuardParticle();
            Destroy(collision.gameObject, .10f);

            Destroy(gameObject); //destroys bullet when hitting enemy

        }*/

        /* else if (collision.CompareTag("FlyingEnemy"))
         {
             explodeTwo = collision.transform.gameObject.GetComponent<FlyingEnemyPatrol>();
             explodeTwo.FlyingEnemyGuardParticle();
             //Destroy(collision.gameObject, .10f);

             Destroy(gameObject); //destroys bullet when hitting enemy

         }*/

        /*  else if (collision.CompareTag("StaticEnemy"))
          {
              explodeStatic = collision.transform.gameObject.GetComponent<StaticEnemyScript>();
              explodeStatic.EnemyGuardParticle();

              Destroy(collision.gameObject, .10f);

              Destroy(gameObject); //destroys bullet when hitting enemy

          }*/


    }





}
