using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System.IO;

public class updateWord : MonoBehaviour {

    public Text english;
    public InputField translation;
    public Dictionary<string, string> langauge;

    public void updateLanguage()
    {
        print("Update Langauge English: " + english.text + " Translation: " + translation.text);
        langauge[english.text] = translation.text;
        save();
    }

    void save()
    {
        string lanName = "/" + "testLanguage" + ".dat";

        if (File.Exists(Application.persistentDataPath + lanName))
        {
            print("deleted the file");
            File.Delete(Application.persistentDataPath + lanName);

        }

        print("Saved");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream testLanguage = File.Create(Application.persistentDataPath + "/" + "testLanguage" + ".dat");

        bf.Serialize(testLanguage, langauge);

        testLanguage.Close();
    }
}
