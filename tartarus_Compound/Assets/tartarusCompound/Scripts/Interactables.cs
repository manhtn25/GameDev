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

    [SerializeField] private PlayerLife deathCheck;
    [SerializeField] private MainPlayerMovement mainPlayerRef;


    [SerializeField] private TilemapRenderer virtualMapBackground;

    [SerializeField] private TilemapCollider2D virtualMapTerrainCollider;
    [SerializeField] private TilemapRenderer virtualMapTerrain;

    [SerializeField]
    private GameObject VirtualTimerBarObject;
    [SerializeField] private Slider VirtualTimerBar;
    private float FillSpeed = 0.068f;
    private float targetProgress = 0f;

    private bool terminalIsCooldown = false;



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

        VirtualTimerBar.enabled = false;


        /*DecrementProgress(0f);*/

        //platforms.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {

        if (VirtualTimerBar.value > targetProgress && inVirtual == true)
        {
            VirtualTimerBarObject.SetActive(true);
            VirtualTimerBar.enabled = enabled;
            VirtualTimerBar.value -= FillSpeed * Time.deltaTime;
        }
        else
        {
            VirtualTimerBarObject.SetActive(false);
            VirtualTimerBar.enabled = false;
        }


        if (isInRange && inVirtual == false && terminalIsCooldown == false)
        {
            terminalText.SetActive(true);
        }
        else
        {
            terminalText.SetActive(false);
        }



        if (isInRange && Input.GetKeyDown(interactKey) && inVirtual == false && terminalIsCooldown == false)
        {
            AudioSource.PlayClipAtPoint(terminal, transform.position);


            //just reveals the platforms && enable a new character and change background, but platforms are static and no timer, pressing E again resets world

            //physicalMapBackground.enabled = !physicalMapBackground.enabled; //false

            virtualMapBackground.enabled = !virtualMapBackground.enabled; //true

            virtualMapTerrainCollider.enabled = !virtualMapTerrainCollider.enabled;

            virtualMapTerrain.enabled = !virtualMapTerrain.enabled;
            mainPlayerRef.spriteMainPlayer.color = new Color32(67, 237, 255, 255);
            mainPlayerRef.tag = "VirtualPlayer";
            inVirtual = true;
            mainPlayerRef.canPunch = false;
            Invoke("VirtualTimer", 15);

        }
        /* else if(isInRange && Input.GetKeyDown(interactKey) && inVirtual)
         {
             AudioSource.PlayClipAtPoint(terminal, transform.position);
             VirtualTimer();

         }*/




        if (terminalIsCooldown == true)
        {
          
            Invoke("TerminalCooldown", 5);
        }

        if (deathCheck.isDead == true)
        {
            CancelInvoke();
            VirtualTimer();
            terminalIsCooldown = false;
        }


    }

    private void TerminalCooldown()
    {
        terminalIsCooldown = false;
    }

    private void VirtualTimer()
    {
        mainPlayerRef.spriteMainPlayer.color = new Color32(248, 248, 248, 255);
        physicalMapBackground.enabled = true; //false
        virtualMapBackground.enabled = false; //true
        virtualMapTerrainCollider.enabled = false;
        virtualMapTerrain.enabled = false;
        inVirtual = false;
        VirtualTimerBar.value = 1;
        mainPlayerRef.tag = "Player";
        mainPlayerRef.canPunch = true;
        terminalIsCooldown = true;

    }


    /*private void DecrementProgress(float newProgress)
    {
        targetProgress = VirtualTimerBar.value - newProgress;
    }
*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("VirtualPlayer"))
        {
            isInRange = false;

        }
    }

}
