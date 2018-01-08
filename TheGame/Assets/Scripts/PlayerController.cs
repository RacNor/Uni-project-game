using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float fireRate;
    private float next;
    public GameObject spell1;
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 position = this.transform.position;
            position.y += (float)(speed * 0.1);
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 position = this.transform.position;
            position.y -= (float)(speed * 0.1);
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 position = this.transform.position;
            position.x -= (float)(speed * 0.1);
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 position = this.transform.position;
            position.x += (float)(speed * 0.1);
            this.transform.position = position;
        }
        if (next > 0)
        {
            next = next - Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            next = next + fireRate;
            Instantiate(spell1, transform.position, transform.rotation);
        }
    }

}
