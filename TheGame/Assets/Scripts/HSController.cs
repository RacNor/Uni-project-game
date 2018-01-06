using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HSController : MonoBehaviour
{

    private string secretKey = "Ne,?Yt8$=F2;^6p*3u(]Rkc%H\"MB[CS9";
    private string highscoreURL = "http://serdcevas.com/display.php";
    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetScores());
    }
    IEnumerator GetScores()
    {
        Text text=gameObject.GetComponent<Text>();
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;
        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            text.text = hs_get.text; // this is a GUIText that will display the scores in game.
        }
    }
}
