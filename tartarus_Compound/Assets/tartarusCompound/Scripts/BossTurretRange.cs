using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretRange : MonoBehaviour
{
    public BossTurret turretObject;

    public BoxCollider2D turretRangeCol;



    private void Start()
    {
        turretRangeCol = GetComponent<BoxCollider2D>();
        turretRangeCol.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            turretObject.playerInTurretRangeBoss = true;
            Debug.Log("true");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            turretObject.playerInTurretRangeBoss = false;

        }
    }
}
