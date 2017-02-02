using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour {

    public GameObject InputPanel;
    public GameObject ReplacePanel;
    public GameObject DisplayPanel;
    public GameObject ReplacementWords;

    public InputField English;
    public Text Translation;

    public InputField[] replaceFields;

    enum State
    {
        Input,
        Replace,
        Display
    };

    private State currentState;


	// Use this for initialization
	void Start () {
        ReplacePanel.SetActive(false);
        DisplayPanel.SetActive(false);
        replaceFields = new InputField[20];
        currentState = State.Input;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            changeState();
        }
	}

    public void changeState()
    {
        switch (currentState)
        {
            case State.Input:
                ParseEnglish();
                break;
            case State.Replace:
                ReplaceEnglish();
                break;
            case State.Display:
                startNewTranslation();
                break;
            default:
                break;
        }
    }

    public void ParseEnglish()
    {
        string english = English.text;
        if (english == "")
        {
            English.text = "You must Type Something";
            return;
        }
        if (english == "You must Type Something")
        {
            English.text = "";
            return;
        }
        string[] words = english.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            InputField newWord = Instantiate(English);
            newWord.text = words[i];
            newWord.gameObject.transform.parent = ReplacementWords.transform;
            newWord.transform.position = new Vector3(0, 0, 0);
            newWord.transform.localScale = new Vector3(1, 1, 1);
            replaceFields[i] = newWord;
        }
        InputPanel.SetActive(false);
        ReplacePanel.SetActive(true);
        currentState = State.Replace;
    }

    public void ReplaceEnglish()
    {
        string translation = "";
        for (int i = 0; i < replaceFields.Length; i++) 
        {
            if (replaceFields[i] != null)
            {
                translation += replaceFields[i].text + " ";
            }
        }
        translation.Remove(translation.Length - 1);
        Translation.text = translation;
        ReplacePanel.SetActive(false);
        DisplayPanel.SetActive(true);
        currentState = State.Display;

    }

    public void startNewTranslation()
    {
        ReplacePanel.SetActive(false);
        DisplayPanel.SetActive(false);
        InputPanel.SetActive(true);
        for (int i = 0; i < replaceFields.Length; i++)
        {
            if (replaceFields[i] != null)
            {
                Destroy(replaceFields[i].gameObject);
                replaceFields[i] = null; 
            }
        }
        English.text = "";
        Translation.text = "";
        currentState = State.Input;
    }
}
