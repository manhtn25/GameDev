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
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<mainPlayerMovement>();

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

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

        }

        theText.text = textLines[currentLine];

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

    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
    }
}
