using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Koniec : MonoBehaviour {
    private int szerokoscPrzycisku = 250;
    private int wysokoscPrzycisku = 40;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnGUI()
    {

        #region PRZYCISKI

        //Start
        if (GUI.Button(new Rect((Screen.width / 2 - szerokoscPrzycisku / 2), 200, szerokoscPrzycisku, wysokoscPrzycisku), "Menu"))
        {
            SceneManager.LoadScene("Menu");
        }
        #endregion
    }
}
