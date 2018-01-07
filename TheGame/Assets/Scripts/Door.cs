using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
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
    }
}
