using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingDmg : MonoBehaviour
{
    [SerializeField] private GameObject punchRight;
    [SerializeField] private GameObject punchLeft;


    public AudioClip airPunch;

    private Animator anim;

    private float punchRate = 0.2f;
    private float nextPunch = 0.0f;

    private void Start()
    {
        punchRight.SetActive(false);
        punchLeft.SetActive(false);

        anim = GetComponent<Animator>(); //animator component
    }

    public void Punch()
    {

        if (Time.time > nextPunch)
        {
            nextPunch = Time.time + punchRate;


            if (GetComponent<MainPlayerMovement>().facingRight)
            {
                StartCoroutine("PunchWait", .15f);
                punchRight.SetActive(true);
                AudioSource.PlayClipAtPoint(airPunch, transform.position);


            }
            else
            {
                StartCoroutine("PunchWait", .15f);
                punchLeft.SetActive(true);
                AudioSource.PlayClipAtPoint(airPunch, transform.position);



            }

        }      

    }

    private IEnumerator PunchWait(float punchDelay)
    {
        yield return new WaitForSeconds(punchDelay);
        punchRight.SetActive(false);
        punchLeft.SetActive(false);
       // anim.SetBool("canPunch", false);
        


    }
}
