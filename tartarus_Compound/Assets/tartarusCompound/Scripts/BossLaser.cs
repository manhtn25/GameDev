using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D coll;

    //tried making a list but it was buggy
   /* [SerializeField] private Interactables virtualCheck;
    [SerializeField] private Interactables virtualCheckTwo;

    [SerializeField] private Switches switchFirst;
    [SerializeField] private Switches switchSecond;*/

    [SerializeField] private PlayerLife deathCheck;

    public AudioSource laserClip;

    private bool bossRoomStart = false;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); //animator component
                                         //inVirtual = GetComponent<Interactables>().inVirtual;

        //anim.SetBool("isOn", false);

        laserClip = GetComponent<AudioSource>();

        //terminalListSize = virtualCheck.Length;

    }

    // Update is called once per frame
    void Update()
    {

       if (bossRoomStart == false)
        {
            laserClip.Pause();
            anim.SetBool("isOn", false);
        }

        if (deathCheck.isDead == true)
        {
            //EnemyDestroyed();
            laserClip.Pause();
            coll.isTrigger = true;
            bossRoomStart = false;

        }


       
      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("VirtualPlayer"))
        {
            bossRoomStart = true;
            laserClip.UnPause();
            anim.SetBool("isOn", true);
            coll.isTrigger = false;
        }
    }
}
