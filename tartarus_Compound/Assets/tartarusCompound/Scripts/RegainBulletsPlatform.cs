using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegainBulletsPlatform : MonoBehaviour
{
    // Start is called before the first frame update

    private BoxCollider2D coll;

    [SerializeField] ShootingScript mainBulletGain;


    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("VirtualPlayer"))
        {
            
            mainBulletGain.GainAmmo(1);
            coll.enabled = false;
            Invoke("ammoReplenish", .50f);
            //coll.enabled = true;

        }
    }

    private void ammoReplenish()
    {
        coll.enabled = true;
    }


}
