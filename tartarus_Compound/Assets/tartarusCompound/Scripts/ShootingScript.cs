using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject BulletRight;
    [SerializeField] private GameObject BulletLeft;
    [SerializeField] private Transform GunNozzleRight;
    [SerializeField] private Transform GunNozzleLeft;

    [SerializeField] private Text bulletText;
    [SerializeField] private GameObject bulletUI;

    public AudioClip gunShot;
    //[SerializeField] private AudioSource gunMainSound;

    private float bulletSpeed = 400f;

    private GameObject tempBullet;

    private float fireRate = 0.25f;
    private float nextFire = 0.0f;

    private int maxBullets = 7;
    public int currentBullets;


    private void Start()
    {
        currentBullets = maxBullets;
        //bulletUI.SetActive(true);
    }

    private void Update()
    {
        //bulletText.text = ": " + currentBullets;
    }

    public void Fire()
    {
        if (Time.time > nextFire && currentBullets > 0)
        {

            nextFire = Time.time + fireRate;

            if (GetComponent<MainPlayerMovement>().facingRight)
            {
                tempBullet = Instantiate(BulletRight, GunNozzleRight.position, BulletRight.transform.rotation);
                tempBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
                AudioSource.PlayClipAtPoint(gunShot, transform.position);
            }
            else
            {
                tempBullet = Instantiate(BulletLeft, GunNozzleLeft.position, BulletLeft.transform.rotation);
                tempBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
                AudioSource.PlayClipAtPoint(gunShot, transform.position);
            }

            currentBullets--;
        }

        

        Destroy(tempBullet, 2.0f); //destroys when bullet does hit anything
    }

    public void GainAmmo(int amount)
    {
        currentBullets += amount;

        if (currentBullets > maxBullets)
        {
            currentBullets = maxBullets;
        }
    }
}
