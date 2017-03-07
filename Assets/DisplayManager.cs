using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayManager : MonoBehaviour {

    public GameObject InputPanel;
    public GameObject ReplacePanel;
    public GameObject DisplayPanel;
    public GameObject ReplacementWords;

    public ML_TextParser textparser;
    public InputField English;
    public GameObject WordPanel;
    public Text Translation;

    public Text[] englishWords;
    public InputField[] replaceFields;
    public GameObject[] replacePanels;

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
        replaceFields = new InputField[50];
        replacePanels = new GameObject[50];
        englishWords = new Text[50];
        currentState = State.Input;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            changeState();
        }
        if (currentState == State.Replace && Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < replaceFields.Length; i++)
            {
                if (replaceFields[i].isFocused)
                {
                    if (i+1 < replaceFields.Length && replaceFields[i+1] != null)
                    {
                        giveFocus(replaceFields[i+1].gameObject);
                    }
                    else
                    {
                        giveFocus(replaceFields[0].gameObject);
                    }
                }
            }
            
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
            English.text = "You must type something";
            return;
        }
        if (english == "You must type something")
        {
            English.text = "";
            return;
        }
        string[] words = english.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            GameObject panel = Instantiate(WordPanel);
            InputField newWord = panel.GetComponentInChildren<InputField>();
            Text[] texts = panel.GetComponentsInChildren<Text>();

            string label = textparser.getLabel(words[i]);
            print(words[i] + " is labeled " + label);
            englishWords[i] = new GameObject().AddComponent<Text>();
            englishWords[i].text = words[i];

           
            for (int j = 0; j < texts.Length; j++)
            {
                if (texts[j].text == "Grammar")
                {
                    texts[j].text = label;
                }
                else if (texts[j].text == "Word")
                {
                    texts[j].text = words[i];
                }
                else
                {
                    print(texts[j].text);
                }
            }

            words[i] = textparser.checkLanguage(words[i].ToLower());
            newWord.text = words[i];
            newWord.transform.SetParent(panel.transform);
            newWord.transform.position = new Vector3(0, -0, 0);
            newWord.transform.localScale = new Vector3(1, 1, 1);
            replaceFields[i] = newWord;
            replacePanels[i] = panel;

            panel.transform.SetParent(ReplacementWords.transform);
            panel.transform.position = new Vector3(0, 0, 0);
            panel.transform.localScale = new Vector3(1, 1, 1);

            
           
            
        }
        giveFocus(replaceFields[0].gameObject);
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
                textparser.updateLanguage(englishWords[i].text, replaceFields[i].text);
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
                Destroy(replacePanels[i].gameObject);
                replacePanels[i] = null;
                replaceFields[i] = null;
                
            }
        }
        englishWords = new Text[50];
        English.text = "";
        Translation.text = "";
        currentState = State.Input;
    }

    private void giveFocus(GameObject needsFocus)
    {
        EventSystem.current.SetSelectedGameObject(needsFocus, null);
    }
}
