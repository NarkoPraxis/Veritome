using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManipulation : MonoBehaviour {

	// Use this for initialization
    public void LoadTranslation()
    {
        SceneManager.LoadScene("MVP");
    }

    public void LoadDictionary()
    {
        SceneManager.LoadScene("Dictionary");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
