﻿using System;
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
            GameManager.instance.RemoveEnemyFromList(collision.gameObject.GetComponent<Enemy>());
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
