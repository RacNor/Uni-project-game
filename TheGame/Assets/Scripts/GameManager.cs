using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject player;
    private MapGeneration mapScript;
    [HideInInspector] public int score = 0;

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
    public void SetPlayerPosition(Vector3 position)
    {
        player.transform.position = position;
    }
    public void GameOver()
    {
        enabled = false;
    }
    void InitGame()
    {
        mapScript.GenerateLevel(level);
    }
}


