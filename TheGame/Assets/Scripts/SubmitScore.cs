using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SubmitScore: MonoBehaviour
{
    private static readonly string secretKey = "Ne,?Yt8$=F2;^6p*3u(]Rkc%H\"MB[CS9";
    private static readonly string addScoreURL = "http://serdcevas.com/addscore.php";
    private string highscoreURL = "http://serdcevas.com/display.php";
    HighScoresJson result;
    public void SubmitScoreToServer(string name,int score)
    { 
        StartCoroutine(PostScore(name,score));
    }
    
    public void GetData()
    {
        StartCoroutine(GetScores());
    }
    IEnumerator GetScores()
    {
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;
        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            result = JsonUtility.FromJson<HighScoresJson>(hs_get.text);
            GameManager.instance.CheckIfTopScore(result);
        }
    }
    IEnumerator PostScore(string name,int score)
    {
        string hash = Utils.Md5Sum(name + score + secretKey);
        /*List<IMultipartFormSection> formData = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("name=" + name),
            new MultipartFormDataSection("score="+score),
            new MultipartFormDataSection("hash="+hash)
        };
        UnityWebRequest www = UnityWebRequest.Post(addScoreURL, formData);*/
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("score", score);
        form.AddField("hash", hash);
        print(name);
        print(hash);
        WWW hs_post = new WWW(addScoreURL,form);
        yield return hs_post;
        if (hs_post.error != null)
        {
            print("There was an error posting the high score: " + hs_post.error);
        }
        else
        {
            Debug.Log("Success");
        }
       /* if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Success");
        }*/
        GameManager.instance.ExitGame();
    }
}
