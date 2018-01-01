using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    bool isOpened;
    public Sprite openedDoor;
    private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void OpenDoor()
    {
        spriteRenderer.sprite = openedDoor;
        isOpened = true;
        /*BoxCollider2D collider2D = gameObject.GetComponent<BoxCollider2D>();
        collider2D.isTrigger*/
    }
}
