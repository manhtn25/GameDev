using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    // Start is called before the first frame update

    // private float playVideo = 2.25f; just intro

    private float playVideo = 47.0f;

    void Start()
    {
        StartCoroutine(PlayVideo());
    }

    // Update is called once per frame
  
    IEnumerator PlayVideo()
    {
        yield return new WaitForSeconds(playVideo);

        SceneManager.LoadScene(1);
    }
}
