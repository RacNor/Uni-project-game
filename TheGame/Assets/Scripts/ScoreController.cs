using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

class ScoreController:MonoBehaviour
{
    public GameObject Score_Table;
    public GameObject Score_Entry;
    public static ScoreController instance;
    private List<ScoreEntry> Scores;
    private string highscoreURL = "http://serdcevas.com/display.php";
    void Awake()
    {
        Scores = new List<ScoreEntry>();
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void OnEnable()
    {
        StartCoroutine(GetScores());
    }
    IEnumerator GetScores()
    {
        Text text = gameObject.GetComponent<Text>();
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;
        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            HighScoresJson result= JsonUtility.FromJson<HighScoresJson>(hs_get.text);
            ShowScore(result);
        }
    }
    public void ShowScore(HighScoresJson data)
    {
        for(int i=0;i<data.data.Count;i++)
        {
            GameObject something = Instantiate(Score_Entry, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            something.transform.SetParent(Score_Table.transform);
            RectTransform rectTransform = something.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1f, 1f, 1f);
            Scores[i].SetScore(data.data[i], i+1);
        }
    }
    public void AddToList(ScoreEntry script)
    {
        Scores.Add(script);
    }
    void OnDisable()
    {
        DeleteList();
    }
    void DeleteList()
    {
        foreach (ScoreEntry entry in Scores)
        {
            entry.DeleteMyself();
        }
        Scores.Clear();
    }
}

