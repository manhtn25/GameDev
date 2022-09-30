using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingDmg : MonoBehaviour
{
    [SerializeField] private GameObject punchRight;
    [SerializeField] private GameObject punchLeft;

    private float punchRate = 0.25f;
    private float nextPunch = 0.0f;

    private void Start()
    {
        punchRight.SetActive(false);
        punchLeft.SetActive(false);
    }

    public void Punch()
    {

        if (Time.time > nextPunch)
        {
            nextPunch = Time.time + punchRate;

            if (GetComponent<MainPlayerMovement>().facingRight)
            {
                StartCoroutine("PunchWait", .25f);
                punchRight.SetActive(true);


            }
            else
            {
                StartCoroutine("PunchWait", .25f);
                punchLeft.SetActive(true);

            }

        }      

    }

    IEnumerator PunchWait(float punchDelay)
    {
        yield return new WaitForSeconds(punchDelay);
        punchRight.SetActive(false);
        punchLeft.SetActive(false);
       
        
    }
}
