using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


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
        load();
    }
	
    public string getLabel(string word)
    {
        if (database.ContainsKey(word.ToLower()) && labels.ContainsKey(database[word.ToLower()]))
        {
            return labels[database[word.ToLower()]];
        }
        else
        {
            return "Grammar Not Found";
        }
    }

    public void updateLanguage(string english, string newWord)
    {
        print("update called on " + english);
        if (!language.ContainsKey(english.ToLower()))
        {
            print(newWord + " was added to langauge");
            language[english.ToLower()] = newWord;
        }
    }

    public string checkLanguage(string english)
    {
        if (language.ContainsKey(english.ToLower()))
        {
            return language[english.ToLower()];
        }
        return english;
    }

    public void save()
    {
        
        delete();
        print("Saved");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream testLanguage = File.Create(Application.persistentDataPath + "/" + "testLanguage" + ".dat");
     
        bf.Serialize(testLanguage, language);
       
        testLanguage.Close();
      
    }

    public void load()
    {
        //string[] names = new string[3];
        string lanName = "/" + "testLanguage" + ".dat";

        if (File.Exists(Application.persistentDataPath + lanName))
        {
            print("found file");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + lanName, FileMode.Open);
            language = (Dictionary<string, string>)bf.Deserialize(file);
            file.Close();
        }
       else
        {
            print("coudln't find file");
        }
    }

    public void delete()
    {
        string lanName = "/" + "testLanguage" + ".dat";
        
        if (File.Exists(Application.persistentDataPath + lanName))
        {
            print("deleted the file");
            File.Delete(Application.persistentDataPath + lanName);
           
        }
       
    }
}
