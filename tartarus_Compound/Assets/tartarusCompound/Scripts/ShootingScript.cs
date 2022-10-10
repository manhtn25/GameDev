using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject BulletRight;
    [SerializeField] private GameObject BulletLeft;
    [SerializeField] private Transform GunNozzleRight;
    [SerializeField] private Transform GunNozzleLeft;

    public AudioClip gunShot;
    //[SerializeField] private AudioSource gunMainSound;

    private float bulletSpeed = 400f;

    private GameObject tempBullet;

    private float fireRate = 0.25f;
    private float nextFire = 0.0f;


    public void Fire()
    {
        if (Time.time > nextFire)
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

        }else if (Time.time < nextFire)
        {
          
        }

        

        Destroy(tempBullet, 2.0f); //destroys when bullet does hit anything
    }
}
