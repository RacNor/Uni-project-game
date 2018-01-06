using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEntry: MonoBehaviour
{
    public GameObject position;
    public GameObject playerName;
    public GameObject date;
    public GameObject score;
    public void Awake()
    {
        ScoreController.instance.AddToList(this);
    }
    public void SetScore(HighScoresJson.ScoreJson data,int pos)
    {
        position.GetComponent<Text>().text = pos.ToString();
        playerName.GetComponent<Text>().text = data.name;
        //string scoreDate=data.date.Split(' ')[0];
        string scoreDate = data.date;
        date.GetComponent<Text>().text = scoreDate;
        score.GetComponent<Text>().text = data.score.ToString();
    }
    public void DeleteMyself()
    {
        Destroy(gameObject);
    }
}

