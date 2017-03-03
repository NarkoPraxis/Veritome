using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ML_TextParser : MonoBehaviour {

    public TextAsset small;
    public TextAsset large;
    public Dictionary<string, string> language;
    public Dictionary<string, string> database;
    public Dictionary<string, string> labels = new Dictionary<string, string>()
    {
        { ",", "Comma" },
        { ".", "Period" },
        { "CC", "Conjunction" },
        { "CD", "Numeral" },
        { "DT", "Determiner" },
        { "EX", "Existential there" },
        { "FW", "Foreign Word" },
        { "IN", "Preposition" },
        { "JJ", "Adjective" },
        { "JJR", "Adjective" },
        { "JJS", "Adjective" },
        { "LS", "List Item" },
        { "MD", "Modal Auxiliary" },
        { "NN", "Noun" },
        { "NNP", "Proper Noun" },
        { "NNS", "Noun" },
        { "PDT", "Pre-Determiner" },
        { "POS", "Genitive Marker" },
        { "PRP", "Pronoun" },
        { "PRPS", "Pronoun" },
        { "RB", "Adverb" },
        { "RBR", "Adverb" },
        { "RBS", "Adverb" },
        { "RP", "Particle" },
        { "SYM", "Symbol" },
        { "TO", "Preposition" },
        { "UH", "Interjection" },
        { "VB", "Verb" },
        { "VBD", "Verb" },
        { "VBG", "Verb" },
        { "VBN", "Verb" },
        { "VBP", "Verb" },
        { "VBZ", "Verb" },
        { "WDT", "Determiner" },
        { "WP", "Pronoun" },
        { "WP$", "Pronoun" },
        { "WRB", "Adverb" }
    };

    private List<string> words;
    private string[] pair;
    private string sFile;
    private string lFile;

    

	// Use this for initialization
	void Start () {
        words = new List<string>();
        pair = new string[2];
        database = new Dictionary<string, string>();
        language = new Dictionary<string, string>();
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
        if (database.ContainsKey(word.ToLower()) && labels.ContainsKey(database[word.ToLower()]))
        {
            return labels[database[word.ToLower()]];
        }
        else
        {
            return "";
        }
    }

    public void updateLanguage(string english, string newWord)
    {
        if (!language.ContainsKey(english))
        {
            language[english] = newWord;
        }
    }

    public string checkLanguage(string english)
    {
        if (language.ContainsKey(english))
        {
            return language[english];
        }
        return english;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
