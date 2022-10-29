using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Interactables : MonoBehaviour
{
    // Start is called before the first frame update

   //www.youtube.com/watch?v=hPsB6MiJPQY&ab_channel=ShackMan, cool idea + tutorial

    //still working on making it private

    public bool isInRange;
    public KeyCode interactKey;
    public bool inVirtual = false;
    [SerializeField] private GameObject terminalText;
    public AudioClip terminal;

    [SerializeField] private TilemapRenderer physicalMapBackground;
    [SerializeField] private GameObject physicalPlayer;
    [SerializeField] private GameObject realCamera;


    [SerializeField] private TilemapRenderer virtualMapBackground;

    [SerializeField] private TilemapCollider2D virtualMapTerrainCollider;
    [SerializeField] private TilemapRenderer virtualMapTerrain;
    //[SerializeField] private GameObject virtualPlayer;
   
    //[SerializeField] private GameObject virtualCamera; 


    //no timer yet + they way the virtualPlayer is facing is different but can be changed


    /*    public UnityEventQueueSystem interactAction; //will be using in the future to activate traps and timer
    */
    private void Start()
    {
        virtualMapBackground.enabled = false; //two ways to do it with bool values or !

        virtualMapTerrainCollider.enabled = false;
        virtualMapTerrain.enabled = false;

        //platforms.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {

        if (isInRange)
        {
            terminalText.SetActive(true);
        }
        else
        {
            terminalText.SetActive(false);
        }


        if (isInRange && Input.GetKeyDown(interactKey) && !inVirtual)
        {
            AudioSource.PlayClipAtPoint(terminal, transform.position);

            //just reveals the platforms && enable a new character and change background, but platforms are static and no timer, pressing E again resets world

            physicalMapBackground.enabled = !physicalMapBackground.enabled; //false

            virtualMapBackground.enabled = !virtualMapBackground.enabled; //true

            virtualMapTerrainCollider.enabled = !virtualMapTerrainCollider.enabled;

            virtualMapTerrain.enabled = !virtualMapTerrain.enabled;

            //physicalPlayer.SetActive(false);
            //virtualPlayer.SetActive(true);

            //realCamera.SetActive(false);
            //virtualCamera.SetActive(true);

            //platforms.SetActive(true);

            inVirtual = true;

        }
        else if (isInRange && Input.GetKeyDown(interactKey) && inVirtual)
        {
            AudioSource.PlayClipAtPoint(terminal, transform.position);
            physicalMapBackground.enabled = !physicalMapBackground.enabled; //false

            virtualMapBackground.enabled = !virtualMapBackground.enabled; //true

            virtualMapTerrainCollider.enabled = !virtualMapTerrainCollider.enabled;

            virtualMapTerrain.enabled = !virtualMapTerrain.enabled;

            //physicalPlayer.SetActive(true);
            //Destroy(virtualPlayer);
            //virtualPlayer.SetActive(false);

            //realCamera.SetActive(true);
            // virtualCamera.SetActive(false);

            inVirtual = false;

        }


    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
    
        }
    }

}
