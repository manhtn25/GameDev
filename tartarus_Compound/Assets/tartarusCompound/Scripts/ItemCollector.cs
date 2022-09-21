using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{

    public int realCoins = 0;

    [SerializeField] private Text coinsText; //make sure to import libarary

    /*    [SerializeField] private AudioSource collectionSoundEffect;
    */



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins")) //this is basically checking to see if the Tag is matching 
        {
            /*            collectionSoundEffect.Play();
            */
            Destroy(collision.gameObject); 
            realCoins++;
            coinsText.text = "Coins: " + realCoins;

            //keep an eye on the hierarchy of objects to see if it actually got destroyed
        }
        
    }

}
