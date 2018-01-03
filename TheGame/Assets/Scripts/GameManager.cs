using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public float turnDelay = .1f;
    public static GameManager instance = null;
    private static int EnemyID=-1;
    public GameObject player;
    [HideInInspector]public MapGeneration mapScript;
    [HideInInspector] public int score = 0;
    [HideInInspector] public bool playersTurn = true;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;

    void Awake()
    {
        print("awake");
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
        InitGame();
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
        instance.level++;
        instance.InitGame();
    }
    void Update()
    {
        if (playersTurn || enemiesMoving)
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
        enabled = false;
    }
    public void InitGame()
    {
        enemies.Clear();
        mapScript.GenerateLevel(level);
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


