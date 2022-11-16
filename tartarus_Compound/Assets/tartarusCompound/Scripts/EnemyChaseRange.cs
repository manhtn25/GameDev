using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseRange : MonoBehaviour
{
    public FlyingEnemyPatrol[] enemyArray;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {
            foreach(FlyingEnemyPatrol enemy in enemyArray)
            {
                enemy.chase = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {
            foreach (FlyingEnemyPatrol enemy in enemyArray)
            {
                enemy.chase = false;
            }
        }
    }

    [SerializeField] private Transform flyingbot;

    private void Update()
    {
        transform.position = new Vector3(flyingbot.position.x, flyingbot.position.y, flyingbot.position.z);
    }
}
