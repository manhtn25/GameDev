using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMain : MonoBehaviour
{
    // Start is called before the first frame update

    //its working now but jsut need to transform to the actual player target


    private GameObject tempBullet;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject bullet;
    float timebetween;
    public float startTimeBetween;
    private float bulletSpeed = 400f;

    void Start()
    {
        timebetween = startTimeBetween;
    }

    // Update is called once per frame
    void Update()
    {
        if (timebetween <= 0)
      
        {
            tempBullet = Instantiate(bullet, firepoint.position, bullet.transform.rotation);
            tempBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
            timebetween = startTimeBetween;
        }
        else
        {
            timebetween -= Time.deltaTime;
        }
        
    }
}
