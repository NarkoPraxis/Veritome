using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System.IO;


public class DictionaryManager : MonoBehaviour {

    public Dictionary<string, string> dictionary;
    public GameObject DictionaryWord;

    [SerializeField]
    private Scrollbar scroll;

    // Use this for initialization
    void Start () {
        print(Application.persistentDataPath);
        load();
        Screen.SetResolution(500, 750, false);
        if (dictionary == null) return;
        foreach (KeyValuePair<string, string> entry in dictionary)
        {
           // print("English: " + entry.Key + " Translation: " + entry.Value);
            GameObject word = Instantiate(DictionaryWord);
            Text english = word.GetComponentInChildren<Text>();
            InputField translation = word.GetComponentInChildren<InputField>();
            word.GetComponent<updateWord>().english = english;
            word.GetComponent<updateWord>().translation = translation;
            word.GetComponent<updateWord>().langauge = dictionary;

            english.text = entry.Key;
            translation.text = entry.Value;
           
            word.transform.SetParent(this.transform);
            word.transform.position = new Vector3(0, 0, 0);
            word.transform.localScale = new Vector3(1, 1, 1);
           
        }
        scroll.value = 1;
    }
	
	

    private void load()
    {
        string lanName = "/" + "testLanguage" + ".dat";

        if (File.Exists(Application.persistentDataPath + lanName))
        {
            print("found file");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + lanName, FileMode.Open);
            dictionary = (Dictionary<string, string>)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            print("coudln't find file");
        }
    }
}
