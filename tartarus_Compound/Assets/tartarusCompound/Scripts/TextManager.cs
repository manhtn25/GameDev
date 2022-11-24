using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    public GameObject textBox;

    public Text theText;

    [SerializeField] public TextAsset textFile;
    public string[] textLines;

    [SerializeField] public int currentLine;
    [SerializeField] public int endAtLine;

    public MainPlayerMovement player;
    public bool stopPlayerMovement;

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<MainPlayerMovement>();

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        // default setting for when endAtLine is set to 0, go all the way to end of file
        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        if (isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        // get the current line of text to display
        if (currentLine < textLines.Length)
        {
            theText.text = textLines[currentLine];
        }

        // next line on button push
        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentLine += 1;
        }

        if (currentLine > endAtLine)
        {
            DisableTextBox();
        }


    }

    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;

        // restrict player movement when text box is active
        if (stopPlayerMovement)
        {
            //player.canMove = false;
            Time.timeScale = 0f;
        }
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;

        // set player free after text box is gone
        //player.canMove = true;
        Time.timeScale = 1.0f;
    }

    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }
}
