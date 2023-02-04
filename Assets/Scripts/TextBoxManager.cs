using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class TextBoxManager is used to control textbox events in-game.
/// Class is based on code from "Unity Basics - Activate & De-Activate Dialogue Box! - Creating a Dialogue Box" YouTube Tutorials by gamesplusjames.
/// </summary>

public class TextBoxManager : MonoBehaviour
{
    public GameObject textBox;
    //public GameObject spriteObject;
    public TMP_Text theText;
    public TMP_Text characterText;
    public List<TextAsset> textFiles = new List<TextAsset>();
    //public List<Sprite> characterSprites = new List<Sprite>();
    private TextAsset textFile;
    public int textFileIndex = 0;
    public List<string> textLines = new List<string>();
    public List<string> characterLines = new List<string>();
    public int currentLine;
    public int endAtLine;
    public bool isActive;
    public bool hasCharacters;
    public bool pauseWhenActive;
    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        textFile = textFiles[textFileIndex];

        //Reading the text file lines into the textLines and characterLines lists.
        if(!hasCharacters)
        {
            if (textFile != null)
            {
                textLines = textFile.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None).ToList();
            }
        }

        if(hasCharacters)
        {
            ReadDialogueFromTxt(textFile);
            characterText.text = characterLines[currentLine];
            //ChangeSprite(currentLine);
        }

        currentLine = 0;
        endAtLine = textLines.Count - 1;

        if(isActive)
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
        if(!isActive)
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            if(!isTyping)
            {
                currentLine++;

                if (currentLine > endAtLine)
                {
                    DisableTextBox();
                } else
                {
                    if(hasCharacters)
                    {
                        characterText.text = characterLines[currentLine];
                    }
                    StartCoroutine(TextScroll(textLines[currentLine]));
                }
            } else if (isTyping && !cancelTyping)
            {
                cancelTyping = true;
            }
        }

        if(hasCharacters)
        {
            if(currentLine <= endAtLine)
            {
                //ChangeSprite(currentLine);
            }
        }

    }

    private IEnumerator TextScroll (string lineOfText)
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;
        while(isTyping && !cancelTyping && (letter < lineOfText.Length -1))
        {
            theText.text += lineOfText[letter];
            letter++;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;

    }

    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;
        if(pauseWhenActive)
        {
            PauseGame();
            //GameObject.Find("Player").GetComponent<PlayerController>().enabled = false;
        }

        StartCoroutine(TextScroll(textLines[currentLine]));

        textFileIndex++;
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;
        if (pauseWhenActive)
        {
            //GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
            ResumeGame();
        }
    }

    //Loads the next text file from the list
    public void ReloadScript()
    {
        TextAsset newTextFile = textFiles[textFileIndex];

        if (!hasCharacters)
        {
            if (newTextFile != null)
            {
                textLines = new List<string>();
                textLines = newTextFile.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None).ToList();
            }
        }

        if (hasCharacters)
        {
            ReadDialogueFromTxt(newTextFile);

        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    //If textfile has dialogue (characters talking), method adds characters to the character list and lines to the line list
    void ReadDialogueFromTxt(TextAsset textFile)
    {
        if (textFile != null)
        {
            List<string> tempList = textFile.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None).ToList();
            textLines = new List<string>();
            characterLines = new List<string>();

            for (int i = 0; i <= tempList.Count - 1; i++)
            {
                if(i % 2 == 0)
                {
                    characterLines.Add(tempList[i]);
                }
                else if (i % 2 != 0)
                {
                    textLines.Add(tempList[i]);
                }
            }
        }
    }

    //Changes the sprite in the dialoguebox according to who is speaking. The sprites in the sprite list must be named the same as in the dialogue text file.
    /*public void ChangeSprite(int line)
    {
        string characterName = characterLines[line];

        for (int i = 0; i <= characterSprites.Count - 1; i++)
        {
            if (characterSprites[i].name.Equals(characterName))
            {
                spriteObject.GetComponent<SpriteRenderer>().sprite = characterSprites[i];
            }
        }
    }*/

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
