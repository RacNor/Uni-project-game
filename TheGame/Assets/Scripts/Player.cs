using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected override void AttemptToMove<T>(int xDir, int yDir)
    {
        base.AttemptToMove<T>(xDir, yDir);
        RaycastHit2D hit;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
           // Invoke("Restart", 0.5f);
           // enabled = false;
        }
    }
    private void OnDisable()
    {
        GameManager.instance.score = score;
    }
    public void PlayerDmg(int dmg)
    {
        health -= dmg;
        CheckIfGameOver();
    }
    public void PlayerHeal(int heal)
    {
        health += heal;
    }
    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
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
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            print("hi");
            AttemptToMove<Door>(horizontal, vertical);
        }
        
	}
}
