using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceController : MonoBehaviour {

    public float speed;
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = transform.up * speed;
    }
}
