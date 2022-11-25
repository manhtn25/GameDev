using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D coll;

    //tried making a list but it was buggy
    [SerializeField] private Interactables virtualCheck;
    [SerializeField] private Interactables virtualCheckTwo;

    [SerializeField] private Switches switchFirst;
    [SerializeField] private Switches switchSecond;

    public AudioSource laserClip;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); //animator component
                                         //inVirtual = GetComponent<Interactables>().inVirtual;

        laserClip = GetComponent<AudioSource>();

        //terminalListSize = virtualCheck.Length;

    }

    // Update is called once per frame
    void Update()
    {

        if (virtualCheck.inVirtual == false && virtualCheckTwo.inVirtual == false)
        {
            anim.SetBool("isOn", true);
            laserClip.UnPause();
            coll.enabled = true;

            /*else
            {
                coll.enabled = false;
            }*/
        }
        else if (virtualCheck.inVirtual == true || virtualCheckTwo.inVirtual == true)
        {
            laserClip.Pause();
            if (tag == "LaserTrap")
            {
                anim.SetBool("isOn", false);
                coll.enabled = false;
            }
        }

        if (switchFirst.switchActive == false && switchSecond.switchActive == false)
        {
            laserClip.Pause();
            if (tag == "ElectricTrap")
            {
                anim.SetBool("isOn", false);
                coll.enabled = false;
            }

        }



        /* else if (virtualCheck.inVirtual == true || virtualCheckTwo.inVirtual == true)
         {
             anim.SetBool("isOn", false);

             if (tag == "LaserTrap")
             {
                 coll.enabled = false;
             }
             else
             {
                 coll.enabled = true; //this is referring to electricTrap
             }

         //just add different animation state later when wanting off and on electric + laser
         }*/


        /* if (virtualCheckTwo.inVirtual == false)
         {
             anim.SetBool("isOn", true);
             if (tag == "LaserTrap")
             {
                 coll.enabled = true;
             }
             else
             {
                 coll.enabled = false;
             }
         }
         else if (virtualCheckTwo.inVirtual == true)
         {
             anim.SetBool("isOn", false);

             if (tag == "LaserTrap")
             {
                 coll.enabled = false;
             }
             else
             {
                 coll.enabled = true; //this is referring to electricTrap
             }

         }*/







    }

}

