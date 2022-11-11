using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardRange : MonoBehaviour
{
    public StaticEnemyScript staticEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {

            staticEnemy.staticCanChase = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {


            staticEnemy.staticCanChase = false;

        }
    }
}
