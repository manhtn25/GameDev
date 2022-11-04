using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemDrops : MonoBehaviour
{
    private Rigidbody2D rb;
    public float dropForce = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);
        
    }

    // Update is called once per frame
    
}
