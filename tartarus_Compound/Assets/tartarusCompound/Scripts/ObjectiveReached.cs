using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveReached : MonoBehaviour
{
    public AudioClip flagWave;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); //animator component
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckPointAnim()
    {
        anim.Play("WayPoint_Animation");
        AudioSource.PlayClipAtPoint(flagWave, transform.position);
    }
}
