using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{

    //MainEnemyGuard explode;


    //[SerializeField] NewMainEnemyScript enemyOne;
   // [SerializeField] MainEnemyGuard enemyTwo;

   /* NewMainEnemyScript enemy;
    FlyingEnemyPatrol enemyTwo;
    StaticEnemyScript enemyStatic;*/

   

    private void Start()
    {
        //explode = GameObject.FindGameObjectWithTag("Enemy").GetComponent<MainEnemyGuard>();
        //enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NewMainEnemyScript>();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        /* if (collision.gameObject.CompareTag("Enemy"))
         {
             if (collision.gameObject.name == "enemyBotTwo")
             {
                 enemyOne.EnemyGuardParticle();
                 //explode.EnemyGuardParticle();


                 Destroy(collision.gameObject, .10f);

             }
             else if (collision.gameObject.name == "enemyBotOne")
             {
                 //explode.EnemyGuardParticle();
                 //enemyTwo.EnemyGuardParticle();
                 Destroy(collision.gameObject, .10f);
             }


         }*/
/*
        if (collision.CompareTag("Enemy"))
        {

            enemy = collision.transform.gameObject.GetComponent<NewMainEnemyScript>();
            enemy.EnemyGuardParticle();
            Destroy(collision.gameObject, .10f);
           
        }

        else if (collision.CompareTag("FlyingEnemy"))
        {
            enemyTwo = collision.transform.gameObject.GetComponent<FlyingEnemyPatrol>();
            enemyTwo.FlyingEnemyGuardParticle();
            //Destroy(collision.gameObject, .10f);
        }

        else if (collision.CompareTag("StaticEnemy"))
        {
            enemyStatic = collision.transform.gameObject.GetComponent<StaticEnemyScript>();
            enemyStatic.EnemyGuardParticle();
            Destroy(collision.gameObject, .10f);
        }*/

      
    }

   
}
