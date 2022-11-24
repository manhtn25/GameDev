using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusSwitches : MonoBehaviour
{
    public bool inSwitchRange;
    public KeyCode activeSwitchKey;
    [SerializeField] private GameObject switchText;
    private Animator anim;
    private BoxCollider2D coll;
    public bool switchActive = true;

    [SerializeField] private PlayerLife deathCheck;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("isOn", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (inSwitchRange)
        {
            switchText.SetActive(true);
        }
        else
        {
            switchText.SetActive(false);
        }


        if (inSwitchRange && Input.GetKeyDown(activeSwitchKey))
        {
            anim.SetBool("isOn", false);
            switchActive = false;
            switchText.SetActive(false);
            coll.enabled = false;


        }

        if (switchActive == false)
        {
            Invoke("ResetSwitches", 2.0f);
        }

        /* if (deathCheck.isDead == true)
         {
             anim.SetBool("isOn", true);
             switchActive = true;
             coll.enabled = true;
         }*/

    }

    private void ResetSwitches()
    {
        anim.SetBool("isOn", true);
        switchActive = true;
        coll.enabled = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("VirtualPlayer"))
        {
            inSwitchRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("VirtualPlayer"))
        {
            inSwitchRange = false;

        }
    }
}
