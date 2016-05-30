using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Nicknames : MonoBehaviour {
    private int szerokoscPrzycisku = 250;
    private int wysokoscPrzycisku = 40;
    public static string player1Name;
    public static string player2Name;


    // Use this for initialization
    void Start () {
        player1Name = "";
        player2Name = "";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        #region POLA

        player1Name = GUI.TextField(new Rect((Screen.width / 2 - szerokoscPrzycisku / 2), 200, szerokoscPrzycisku, wysokoscPrzycisku), player1Name, 25);

        player2Name = GUI.TextField(new Rect((Screen.width / 2 - szerokoscPrzycisku / 2), 260, szerokoscPrzycisku, wysokoscPrzycisku), player2Name, 25);


        #endregion

        #region PRZYCISKI
        //Wyjście
        if (GUI.Button(new Rect((Screen.width / 2 - szerokoscPrzycisku / 2), 320, szerokoscPrzycisku, wysokoscPrzycisku), "GRAJ!"))
        {
           SceneManager.LoadScene("Billiard room");
        }

        #endregion
    }
}
