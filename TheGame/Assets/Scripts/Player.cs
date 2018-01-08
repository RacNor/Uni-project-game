using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MovingObject {

    [HideInInspector]public int score;
    public int health = 100;
    public Text HealthText;
    public Text ScoreText;
    public GameObject spell1;
    private Animator animator;

    protected override void OnCantMove<T>(T component)
    {
    }

    // Use this for initialization
    protected override void Start () {
        score = GameManager.instance.score;
        health = GameManager.instance.health;
        HealthText.text = "Health: " + health;
        ScoreText.text = "Score: " + score;
        animator = GetComponent<Animator>();
        base.Start();
	}
    protected override bool AttemptToMove<T>(int xDir, int yDir)
    {
        HealthText.text = "Health: " + health;
        ScoreText.text = "Score: " + score;
        animator.SetTrigger("PlayerMove");
        bool result=base.AttemptToMove<T>(xDir, yDir);
        if (result)
        {
            GameManager.instance.playersTurn = false;
            return result;
        }
        //RaycastHit2D hit;
        
        return result;

    }
    public void AddScore(int score)
    {
        ScoreText.text = "Score: " + this.score+" +"+score;
        this.score += score;
    }
    private bool AttemptToShoot()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        Instantiate(spell1, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        GameManager.instance.playersTurn = false;
        return true;
    }
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return (Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg) - 90;
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttemptToShoot();
        }
        else
        {
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
}
