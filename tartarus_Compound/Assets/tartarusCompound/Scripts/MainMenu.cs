using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string startScene;

    [SerializeField] private GameObject leftCredits;
    [SerializeField] private GameObject rightCredits;


    //[SerializeField] public AudioClip click;

    // Start is called before the first frame update
    void Start()
    {
        leftCredits.SetActive(false);
        rightCredits.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {       
        //AudioSource.PlayClipAtPoint(click, transform.position);
        SceneManager.LoadScene(startScene);

    }

    public void CreditsPage()
    {
        //AudioSource.PlayClipAtPoint(click, transform.position);
        leftCredits.SetActive(true);
        rightCredits.SetActive(true);
        Invoke("DisableCredits", 5.0f);
    }

    private void DisableCredits()
    {
        leftCredits.SetActive(false);
        rightCredits.SetActive(false);
    }

    public void QuitGame()
    {
        //AudioSource.PlayClipAtPoint(click, transform.position);        
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}