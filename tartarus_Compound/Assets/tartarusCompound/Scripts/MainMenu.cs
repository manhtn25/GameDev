using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string startScene;

    [SerializeField] private GameObject leftCredits;
    [SerializeField] private GameObject rightCredits;


   // [SerializeField] private AudioClip prisonDoor;
   // public AudioSource prisonDoor;

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
        SceneManager.LoadScene(startScene);
    }

    public void CreditsPage()
    {
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
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}