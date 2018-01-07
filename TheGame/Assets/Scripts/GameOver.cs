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
            name = RemoveWhitespace(name);
            GameManager.submitScore.SubmitScoreToServer(name, score);
        }
        else
        {
            GameManager.instance.ExitGame();
        }
        
    }
    public string RemoveWhitespace(string input)
    {
        return new string(input.ToCharArray()
            .Where(c => !Char.IsWhiteSpace(c))
            .ToArray());
    }
}

