using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{

    private int cherries = 0;

    [SerializeField] private Text cherriesText; //make sure to import libarary

    [SerializeField] private AudioSource collectionSoundEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry")) //this is basically checking to see if the Tag is matching 
        {
            collectionSoundEffect.Play();
            Destroy(collision.gameObject); //destroys game object when colliding
            cherries++;
            cherriesText.text = "Cherries: " + cherries;

            //keep an eye on the hierarchy of objects to see if it actually got destroyed
        }
        
    }

}
