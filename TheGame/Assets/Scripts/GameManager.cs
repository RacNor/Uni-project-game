using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private MapGeneration mapScript;
    private int level = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
            
        DontDestroyOnLoad(this);
        mapScript = GetComponent<MapGeneration>();
        InitGame();
    }
    void InitGame()
    {
        mapScript.GenerateLevel(level);
    }
}


