using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DisplayManager : MonoBehaviour {

    public GameObject InputPanel;
    public GameObject ReplacePanel;
    public GameObject DisplayPanel;
    public GameObject ReplacementWords;

    public ML_TextParser textparser;
    public InputField English;
    public GameObject WordPanel;
    public Text Translation;

    public List<string> englishWords;
    public List<InputField> replaceFields;
    public List<GameObject> replacePanels;

    enum State
    {
        Input,
        Replace,
        Display
    };

    private State currentState;


	// Use this for initialization
	void Start () {
        Screen.SetResolution(1057, 173, false);
        ReplacePanel.SetActive(false);
        DisplayPanel.SetActive(false);
        replaceFields = new List<InputField>();
        replacePanels = new List<GameObject>();
        englishWords = new List<string>();
        currentState = State.Input;
        giveFocus(InputPanel.gameObject.GetComponentInChildren<InputField>().gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            changeState();
        }

        if (currentState == State.Replace && Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < replaceFields.Count; i++)
            {
                if (replaceFields[i].isFocused)
                {
                    print("found Focus at " + replaceFields[i].text);
                    if (i+1 < replaceFields.Count && replaceFields[i+1] != null)
                    {
                        giveFocus(replaceFields[i+1].gameObject);
                    }
                    else
                    {
                        giveFocus(replaceFields[0].gameObject);
                    }
                }else
                {
                    print("didn't find focus");
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
                giveFocus(replaceFields[0].gameObject);
                break;
            case State.Replace:
                ReplaceEnglish();
                break;
            case State.Display:
                startNewTranslation();
                giveFocus(InputPanel.gameObject.GetComponentInChildren<InputField>().gameObject);
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
        int height = 0;
        print("Words  " + words.Length);
        
        height = (int)(words.Length / 7);
        print("height " + height);
        
        Screen.SetResolution(Screen.width, 260 + (int)(103.24 * height), false);
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Equals(""))
            {
                print("----------------found space");
                continue;
            }
            words[i] = scrub(words[i]).ToLower();
            GameObject panel = Instantiate(WordPanel);
            panel.name = words[i];
            InputField newWord = panel.GetComponentInChildren<InputField>();
            Text[] texts = panel.GetComponentsInChildren<Text>();

            string label = textparser.getLabel(words[i]);
            print(words[i] + " is labeled " + label);
            
            
            englishWords.Add(words[i]);
              
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

 
            replacePanels.Add(panel);
            replaceFields.Add(newWord);
            

            panel.transform.SetParent(ReplacementWords.transform);
            panel.transform.position = new Vector3(0, 0, 0);
            panel.transform.localScale = new Vector3(1, 1, 1);
        }
        InputPanel.SetActive(false);
        ReplacePanel.SetActive(true);
        currentState = State.Replace;
    }

    public void ReplaceEnglish()
    {
        string translation = "";
        for (int i = 0; i < replaceFields.Count; i++) 
        {
            if (replaceFields[i] != null)
            {
                textparser.updateLanguage(englishWords[i], replaceFields[i].text);
                translation += replaceFields[i].text + " ";
            }
        }
        translation.Remove(translation.Length - 1);
        Translation.text = translation;
        ReplacePanel.SetActive(false);
        DisplayPanel.SetActive(true);
        currentState = State.Display;
        GUIUtility.systemCopyBuffer = translation;
        textparser.save();
    }

    public void startNewTranslation()
    {
        ReplacePanel.SetActive(false);
        DisplayPanel.SetActive(false);
        InputPanel.SetActive(true);
        for (int i = 0; i < replaceFields.Count;)
        {
           Destroy(replacePanels[i].gameObject);
           replacePanels.RemoveAt(i);
           replaceFields.RemoveAt(i);
        }

        englishWords.RemoveRange(0, englishWords.Count);
        English.text = "";
        Translation.text = "";
        currentState = State.Input;
    }

    private void giveFocus(GameObject needsFocus)
    {
        EventSystem.current.SetSelectedGameObject(needsFocus, null);
    }

    private string scrub(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (   word[i].Equals('.')
                || word[i].Equals(',')
                || word[i].Equals('?')
                || word[i].Equals(';')
                || word[i].Equals(':')
                || word[i].Equals('\"')
                || word[i].Equals('!')
                || word[i].Equals(' '))
            {
                print("removed: " + word[i]);
                return word.Remove(i);
            }
        }
        return word;
    }

    public void Delete()
    {
        textparser.delete();
        textparser.language.Clear();
        if (textparser.language.ContainsKey("i"))
        {
            print("delete");
        }
        else
        {
            print("delete didn't work");
        }
    }

    public void displayDictionary()
    {
        SceneManager.LoadScene("Dictionary");
    }
}
