using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] MainPlayerMovement anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {

            anim.isJumping = false;

        }
    }

  /*  private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {


            turretObject.playerInTurretRange = false;

        }
    }*/
}
