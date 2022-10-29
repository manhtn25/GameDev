using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D coll;


    [SerializeField] private Interactables virtualCheck;

    // Start is called before the first frame update
    void Start()
    {
       coll = GetComponent<BoxCollider2D>();
       anim = GetComponent<Animator>(); //animator component
       //inVirtual = GetComponent<Interactables>().inVirtual;

        

    }

    // Update is called once per frame
    void Update()
    {
     
        if (virtualCheck.inVirtual == false)
        {
            anim.SetBool("isOn", true);
            coll.enabled = true;
        }

        else if (virtualCheck.inVirtual == true)
        {
            anim.SetBool("isOn", false);
            coll.enabled = false;
        }





    }
    
}
