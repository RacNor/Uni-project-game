using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = .1f;
    public LayerMask colisonLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;

    }
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, colisonLayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
            print("ye");
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        print("nope");
        return false;
    }

    protected virtual void AttemptToMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        if (hit.transform == null)
            return;
        T hitComponent = hit.transform.GetComponent<T>();
        if(!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        rigidBody.MovePosition(end);
        yield return null;
        /*float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while(sqrRemainingDistance> float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rigidBody.position, end, inverseMoveTime * Time.deltaTime);
            rigidBody.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }*/
    }
    protected abstract void OnCantMove<T>(T component)
        where T : Component;

}

