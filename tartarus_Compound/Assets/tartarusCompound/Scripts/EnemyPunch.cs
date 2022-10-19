using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPunch : MonoBehaviour
{
    MainPlayerMovement death;
    PlayerLife healthDmg;

    private void Start()
    {
        
        death = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerMovement>();
        healthDmg = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.CompareTag("Player"))
        {
            //needs to play player death anim // but subtract from now instead
            //death.MainPlayerDeath();

            healthDmg.TakeDamage(1);

            //collision.GetComponent<MainPlayerMovement>().canMove = false;
            //call the playerlife function to respawn --> just invisible not runnign
            //respawnAfterDeath.combackAlive();

        }
    }
}
