using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    NewMainEnemyScript explode;
    FlyingEnemyPatrol explodeTwo;
    StaticEnemyScript explodeStatic;

    [SerializeField] private GameObject[] enemyPrefabs;

    private Vector3 respawnEnemyPos;

    private void Start()
    {

   
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

        else if (collision.CompareTag("StaticEnemy"))
        {
            explodeStatic = collision.transform.gameObject.GetComponent<StaticEnemyScript>();
          
            respawnEnemyPos = collision.gameObject.transform.position;

            explodeStatic.EnemyGuardParticle();

            //Destroy(collision.gameObject, .10f);

            explodeStatic.Sprite.enabled = false;

            Destroy(gameObject); //destroys bullet when hitting enemy

            StartCoroutine(RespawnEnemy(collision.gameObject,  respawnEnemyPos));



        }


    }



    private IEnumerator RespawnEnemy(GameObject orgEnemy, Vector3 respawnPos)
    {
        yield return new WaitForSeconds(5);

        GameObject enemyClone = (GameObject)Instantiate(enemyPrefabs[0]);
        enemyClone.transform.position = respawnPos;
        //Instantiate(newEnemy, respawnPos, Quaternion.identity);
        Debug.Log("Success");

        Destroy(orgEnemy);
    }



}
