using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameOver:MonoBehaviour
{
    public void Submit()
    {
        int score = GameManager.instance.score;
        if (GameManager.instance.nameField.activeSelf)
        {
            InputField field= GameManager.instance.nameField.GetComponent<InputField>();
            string name = field.text;
            GameManager.submitScore.SubmitScoreToServer(name, score);
        }
        GameManager.instance.ExitGame();
    }
}

