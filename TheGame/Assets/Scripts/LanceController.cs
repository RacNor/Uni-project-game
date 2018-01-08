using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceController : MovingObject {

    public float speed;
    private Rigidbody2D rb2d;

    void Update()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = -1 * transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            print("kill!!!!!");
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Player player = GameManager.instance.player.GetComponent<Player>();
            player.AddScore(enemy.score);
            GameManager.instance.RemoveEnemyFromList(enemy);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        if(collision.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Destroy(this);
    }
}
