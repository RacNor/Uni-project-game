using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay=2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    private static int EnemyID=-1;
    [HideInInspector] public GameObject player;
    [HideInInspector] public MapGeneration mapScript;
    [HideInInspector] public int score = 0;
    [HideInInspector] public int health = 100;
    [HideInInspector] public bool playersTurn = true;

    private int level = 0;
    private GameObject levelImage;
    private Text levelText;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool loading;

    void Awake()
    {
        print(health);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        enemies = new List<Enemy>();
        DontDestroyOnLoad(this);
        mapScript = GetComponent<MapGeneration>();
       
        //InitGame();
    }
    public int GetEnemyId()
    {
        EnemyID++;
        return EnemyID;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        print(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name=="Main")
        {
            instance.level++;
            instance.InitGame();
        }
    }
    void Update()
    {
        if (playersTurn || enemiesMoving|| loading)
        {
            return;
        }
        StartCoroutine(MoveEnemies());
    }
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
    public void RemoveEnemyFromList(Enemy script)
    {
        enemies.Remove(script);
    }
    public void SetPlayerPosition(Vector3 position)
    {
        player.transform.position = position;
    }
    public void GameOver()
    {
        levelText.text = "You died\n your Score: " + score;
        levelImage.SetActive(true);
        enabled = false;
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }
    public void InitGame()
    {
        player = GameObject.Find("TestPlayer");
        loading = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Level " + level;
        levelImage.SetActive(true);
        int start=DateTime.Now.Second;
        enemies.Clear();
        mapScript.GenerateLevel(level);
        int finish = DateTime.Now.Second - start;
        float left = levelStartDelay - finish;
        if (left > 0)
        {
            Invoke("HideLevelImage", left);
        }
        else
        {
            HideLevelImage();
        }
    }
    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        loading = false;
    }
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        bool atleastOneMoved = false;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for(int i=0;i<enemies.Count;i++)
        {
            if (enemies[i].MoveEnemy())
            {
                atleastOneMoved = true;
                yield return new WaitForSeconds(enemies[i].moveTime);
            }
        }
        if (!atleastOneMoved)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        playersTurn = true;
        enemiesMoving = false;
    }
}


