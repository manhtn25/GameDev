using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegainHealthPlatform : MonoBehaviour
{

    private BoxCollider2D coll;

    [SerializeField] PlayerLife mainHealthGain;


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
            mainHealthGain.GainHealth(1);
            coll.enabled = false;
            Invoke("healthReplenish", .50f);
            //coll.enabled = true;

        }
    }

    private void healthReplenish()
    {
        coll.enabled = true;
    }


}
