using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MovingObject {

    private int score;
    public int health = 100;
    public Text HealthText;
    public Text ScoreText;

    protected override void OnCantMove<T>(T component)
    {
    }

    // Use this for initialization
    protected override void Start () {
        score = GameManager.instance.score;
        health = GameManager.instance.health;
        HealthText.text = "Health: " + health;
        ScoreText.text = "Score: " + score;
        base.Start();
	}
    protected override bool AttemptToMove<T>(int xDir, int yDir)
    {
        HealthText.text = "Health: " + health;
        ScoreText.text = "Score: " + score;
        bool result=base.AttemptToMove<T>(xDir, yDir);
        if (result)
        {
            GameManager.instance.playersTurn = false;
            return result;
        }
        //RaycastHit2D hit;
        
        return result;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            int level = GameManager.instance.level;
            int cof = level < 10 ? level : 10;
            score += cof * 10;
            enabled = false;
            Invoke("Restart", 0.5f);
        }
        if (other.tag == "Door")
        {
            Door door = other.gameObject.GetComponent<Door>();
            door.OpenDoor();
        }
    }
    private void OnDisable()
    {
        GameManager.instance.score = score;
        GameManager.instance.health = health;
    }
    public void PlayerDmg(int dmg)
    {
        health -= dmg;
        HealthText.text ="-"+dmg+ " Health: " + health;
        CheckIfGameOver();
    }
    public void PlayerHeal(int heal)
    {
        health += heal;
    }
    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
	private void CheckIfGameOver()
    {
        if (health <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.playersTurn) return;
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0)
        {
            vertical = 0;
        }
        if (horizontal != 0 || vertical != 0)
        {
            AttemptToMove<Wall>(horizontal, vertical);
        }
	}
}
