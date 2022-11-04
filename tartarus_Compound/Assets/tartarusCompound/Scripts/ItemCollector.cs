using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{

    public int realCoins = 0;
    public AudioClip coinSound;

    public bool hasGun = false;

    [SerializeField] PlayerLife mainPlayerHealth;
    [SerializeField] ShootingScript mainBulletGain;

    //[SerializeField] private Text coinsText; //make sure to import libarary

    //[SerializeField] private AudioSource collectionSoundEffect;
    



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins")) //this is basically checking to see if the Tag is matching 
        {
            /*            collectionSoundEffect.Play();
            */
            AudioSource.PlayClipAtPoint(coinSound, transform.position);

            //Destroy(collision.gameObject); 
            //collision.gameObject.SetActive(false);
            realCoins++;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //coinsText.text = ": " + realCoins;

            //keep an eye on the hierarchy of objects to see if it actually got destroyed
        }
        else if (collision.gameObject.CompareTag("Health") && mainPlayerHealth.currentHealth < 3)
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            mainPlayerHealth.GainHealth(1);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }else if (collision.gameObject.CompareTag("Bullets") && mainBulletGain.currentBullets < 7)
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            mainBulletGain.GainAmmo(1);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (collision.gameObject.name == "GunFind")
        {
            hasGun = true;
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

    }


}
