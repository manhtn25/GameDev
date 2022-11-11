using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRange : MonoBehaviour
{
    public TurretMain turretObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {
        
                turretObject.playerInTurretRange = true;
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {


            turretObject.playerInTurretRange = false;
            
        }
    }
}
