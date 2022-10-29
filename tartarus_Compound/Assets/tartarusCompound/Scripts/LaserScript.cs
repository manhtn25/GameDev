using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D coll;

    //tried making a list but it was buggy
    private Interactables virtualCheck;

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

        /*for (int i = 0; i < terminalListSize; i++)
        {
            if (virtualCheck[i].inVirtual == false)
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

            else if (virtualCheck[i].inVirtual == true)
            {
                anim.SetBool("isOn", false);
         
                if (tag == "LaserTrap")
                {
                    coll.enabled = false;
                }
                else {
                    coll.enabled = true; //this is referring to electricTrap
                }
            }
        }*/
       

        if (virtualCheck.inVirtual == false)
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
        else if (virtualCheck.inVirtual == true)
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
     
        





    }
    
}
