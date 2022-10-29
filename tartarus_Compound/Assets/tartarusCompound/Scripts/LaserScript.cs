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

    // Start is called before the first frame update
    void Start()
    {
       coll = GetComponent<BoxCollider2D>();
       anim = GetComponent<Animator>(); //animator component
                                        //inVirtual = GetComponent<Interactables>().inVirtual;

      //terminalListSize = virtualCheck.Length;

    }

    // Update is called once per frame
    void Update()
    {

        if (virtualCheck.inVirtual == false && virtualCheckTwo.inVirtual == false)
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
        else if (virtualCheck.inVirtual == true || virtualCheckTwo.inVirtual == true)
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

        }


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

