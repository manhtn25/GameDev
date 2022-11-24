using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRange : MonoBehaviour
{
    public TurretMain turretObject;

    public CerberusHeads leftHead;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {
        
                turretObject.playerInTurretRange = true;
                leftHead.chase = true;
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {

            leftHead.chase = false;
            turretObject.playerInTurretRange = false;
            
        }
    }
}
