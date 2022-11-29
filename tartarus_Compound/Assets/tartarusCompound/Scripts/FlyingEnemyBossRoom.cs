using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyBossRoom : MonoBehaviour
{
    public BossFlyingGuards[] enemyArray;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {
            foreach (BossFlyingGuards enemy in enemyArray)
            {
                enemy.chaseBoss = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {
            foreach (BossFlyingGuards enemy in enemyArray)
            {
                enemy.chaseBoss = false;
            }
        }
    }

   /* [SerializeField] private Transform flyingbot;

    private void Update()
    {
        transform.position = new Vector3(flyingbot.position.x, flyingbot.position.y, flyingbot.position.z);
    }*/
}
