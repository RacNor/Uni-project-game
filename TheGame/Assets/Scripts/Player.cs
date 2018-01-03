using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MovingObject {

    private int score;
    public int health = 100;

    protected override void OnCantMove<T>(T component)
    {
        print("Door");
    }

    // Use this for initialization
    protected override void Start () {
        score = GameManager.instance.score;
        base.Start();
	}
    protected override bool AttemptToMove<T>(int xDir, int yDir)
    {
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
            Invoke("Restart", 0.5f);

            //Disable the player object since level is over.
            //enabled = false;
            // Invoke("Restart", 0.5f);
            // enabled = false;
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
    }
    public void PlayerDmg(int dmg)
    {
        health -= dmg;
        print("dmg");
        //CheckIfGameOver();
    }
    public void PlayerHeal(int heal)
    {
        health += heal;
    }
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
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
