using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System;

public class Enemy : MovingObject, IEquatable<Enemy>
{
    private int ID;
    private Transform target;
    private bool skipMove;
    private ArrayList path;
    private Animator animator;
    private bool find = false;
    public int vision=4;
    public int explode = 2;
    private bool isexploding = false;
    public static int attackSpeed=1;
    public int damage=50;
    private int countDown = attackSpeed;
    public Animation deathAnimation;
    private float animationLength=0f;
    protected override void Start()
    {
        ID = GameManager.instance.GetEnemyId();
        GameManager.instance.AddEnemyToList(this);
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        target = GameObject.Find("TestPlayer").transform;
        animator = GetComponent<Animator>();
        base.Start();
    }
    protected override bool AttemptToMove<T>(int xDir, int yDir)
    {

        return base.AttemptToMove<T>(xDir, yDir);

    }
    public bool MoveEnemy()
    {
        if (isexploding)
        {
            Explode();
            return false;
        }
        Utils.Coord myCoord = new Utils.Coord((int)transform.position.x, (int)transform.position.y);
        Utils.Coord targetCoord = new Utils.Coord((int)target.transform.position.x, (int)target.transform.position.y);
        int x = (int)Mathf.Abs(myCoord.x- targetCoord.x);
        int y = (int)Mathf.Abs(myCoord.y- targetCoord.y);
        if (x <= vision && y <= vision)
        {
            find = true;
        }
        if (find)
        {
            print("x= " + targetCoord.x + " y= " + targetCoord.y);
            path = FindPath(myCoord, targetCoord);
        }
        find = false;
        /* int xDir = 0;
         int yDir = 0;
         if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
         {
             yDir = target.position.y > transform.position.y ? 1 : -1;
         }
         else
         {
             xDir = target.position.x > transform.position.x ? 1 : -1;
         }*//*
        if (skipMove)
        {
            skipMove = false;
            return false;
        }
        skipMove = true;*/
        if (path == null)
        {
            return false;
        }
        if (path.Count > 0)
        {
            Utils.Coord coord = (Utils.Coord)path[0];
            int xDir = (int)(coord.x- transform.position.x);
            int yDir = (int)(coord.y- transform.position.y);
            if (AttemptToMove<Player>(xDir, yDir))
            {
                path.RemoveAt(0);
                myCoord = new Utils.Coord((int)transform.position.x, (int)transform.position.y);
                x = (int)Mathf.Abs(myCoord.x - targetCoord.x);
                y = (int)Mathf.Abs(myCoord.y - targetCoord.y);
                if (x <= explode && y <= explode)
                {
                    Explode();
                }
                return true;
            }
        }
        return false;
        //AttemptToMove<Player>(xDir, yDir);
    }
    private void Explode()
    {
        print("ye");
        if (!isexploding)
        {
            animator.SetTrigger("EnemyExploding");
        }
        isexploding = true;
        if (countDown != 0)
        {
            print("what");
            countDown--;
            return;
        }
        Utils.Coord myCoord = new Utils.Coord((int)transform.position.x, (int)transform.position.y);
        Utils.Coord targetCoord = new Utils.Coord((int)target.transform.position.x, (int)target.transform.position.y);
        int x = (int)Mathf.Abs(myCoord.x - targetCoord.x);
        int y = (int)Mathf.Abs(myCoord.y - targetCoord.y);
        x = (int)Mathf.Abs(myCoord.x - targetCoord.x);
        y = (int)Mathf.Abs(myCoord.y - targetCoord.y);
        if (x <= explode && y <= explode)
        {
            Player player=target.GetComponent<Player>();
            player.PlayerDmg(damage);
            
            //gameObject.SetActive(false);
        }
        animator.SetTrigger("EnemyExplode");
        new WaitForEndOfFrame();
        GetClipLength();
        GameManager.instance.RemoveEnemyFromList(this);
        Destroy(this.gameObject, 1f);
    }
    IEnumerator GetClipLength()
    {
        yield return new WaitForEndOfFrame();
        animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
    }
    protected override void OnCantMove<T>(T component)
    {
        
    }
    public ArrayList FindPath(Utils.Coord start, Utils.Coord dest)
    {
        Debug.Log("find");
        this.path = new ArrayList();
        List<Utils.Coord> openList = new List<Utils.Coord>();
        List<Utils.Coord> closedList = new List<Utils.Coord>();
        ArrayList cameFrom = new ArrayList();
        int width = GameManager.instance.mapScript.columns;
        int height = GameManager.instance.mapScript.rows;
        int[,] gScore = NewArray();
        gScore[start.x, start.y] = 0;
        int[,] fScore = NewArray();
        fScore[start.x, start.y] = Cost_Estimate(start, dest);
        openList.Add(start);
        while (openList.Count>0)
        {
            Utils.Coord current = GetWithLowestScore(openList, fScore);
            if (current.Equals(dest))
            {
                return GetPath(current,start);
            }
            openList.Remove(current);
            closedList.Add(current);
            ArrayList neighbours = GameManager.instance.mapScript.Neigbours(current);
            if (neighbours == null) continue;
            foreach(Utils.Coord neighbour in neighbours)
            {
                if (closedList.Contains(neighbour))
                {
                    continue;
                }
                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
                int tentativeScore = gScore[current.x, current.y] + 1;
                if (tentativeScore >= gScore[neighbour.x, neighbour.y])
                {
                    continue;
                }
                neighbour.cameFrom = current;
                gScore[neighbour.x, neighbour.y] = tentativeScore;
                fScore[neighbour.x, neighbour.y] = gScore[neighbour.x, neighbour.y] + Cost_Estimate(neighbour, dest);
            }
        }
        return null;
    }
    private ArrayList GetPath(Utils.Coord current,Utils.Coord start)
    {
        ArrayList path = new ArrayList();
        path.Insert(0, current);
        while (current.cameFrom != null)
        {
            current = current.cameFrom;
            if (current.Equals(start))
            {
                break;
            }
            path.Insert(0, current);
        }
        return path;
    }
    private Utils.Coord GetWithLowestScore(List<Utils.Coord> list, int[,] fScore)
    {
        Utils.Coord result=(Utils.Coord)list[0];
        for(int i = 1; i < list.Count; i++)
        {
            Utils.Coord coord= (Utils.Coord)list[i];
            int resultScore = fScore[result.x, result.y];
            int currentScore = fScore[coord.x, coord.y];
            if (resultScore > currentScore)
            {
                result = coord;
            }
        }
        return result;
    }
    private int Cost_Estimate(Utils.Coord start, Utils.Coord end)
    {
        return (int)(Mathf.Abs(start.x - end.x) +
            Mathf.Abs(start.y - end.y));
    }
    private int[,] NewArray()
    {
        int width = GameManager.instance.mapScript.columns;
        int height = GameManager.instance.mapScript.rows;
        int[,] score = new int[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                score[x, y] = int.MaxValue;
            }
        }
        return score;
    }

    public bool Equals(Enemy other)
    {
        return this.ID.Equals(other.ID);
    }
}

