using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ML_TextParser : MonoBehaviour {

    public TextAsset small;
    public TextAsset large;
    public Dictionary<string, string> database;

    private List<string> words;
    private string[] pair;
    private string sFile;
    private string lFile;

	// Use this for initialization
	void Start () {
        words = new List<string>();
        pair = new string[2];
        database = new Dictionary<string, string>();

        sFile = large.text;
        words.AddRange(sFile.Split(" "[0]));
       
        int size = words.Count;
        for (int i = 0; i < size; i++)
        {
            pair = words[i].Split("_"[0]);
            if (!database.ContainsKey(pair[0].ToLower()))
            {
                database.Add(pair[0].ToLower(), pair[1]);
            }
        }

        print("Database Size: " + database.Count);

    }
	
    public string getLabel(string word)
    {
        if (database.ContainsKey(word.ToLower()))
        {
            return database[word.ToLower()];
        }
        else
        {
            return "";
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
