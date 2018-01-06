using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameOver:MonoBehaviour
{
    
    public void Start()
    {

    }
    public void Submit()
    {
        int score = GameManager.instance.score;
        if (GameManager.instance.nameField.activeSelf)
        {
            print("yes");
            Text text = GameManager.instance.nameField.GetComponent<Text>();
            string name = text.text;
            print(name);
        }
        else
        {
            print("no");
        }
        //GameManager.submitScore.SubmitScoreToServer("labas", score);
    }
}

