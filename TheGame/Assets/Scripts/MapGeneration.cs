using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{

    public enum TileType
    {
        Wall, Floor, Corridor, Door, Edges, UnbreakableWalls, BreakableWalls,
    }
    public IntRange numRooms = new IntRange(4, 10);
    public IntRange roomWidth = new IntRange(3, 10);
    public IntRange roomHeight = new IntRange(3, 10);

    public IntRange coordinates = new IntRange(50, 60);
    public int rows = 100;
    public int columns = 100;
    private List<Room> rooms;
    private List<Corridor> corridors;
    public GameObject[] mFloor;
    public GameObject[] mWall;
    public GameObject[] mDoor;
    private GameObject mBoardHolder;
    TileType[,] map;
    void Start()
    {
        mBoardHolder = new GameObject("BoardHolder");
        generate();

    }
    void generate()
    {
        map = new TileType[columns, rows];
        CreateRooms();
        Intersect();
        ClearConnectedRooms();
        corridors = new List<Corridor>();
        ConnectRooms();
        InnerWallPadding(5, TileType.UnbreakableWalls);
        BreakableWalls();
        InnerWallPadding();
        InitializeTyles();
        initCorridors();
        PlaceGameobject();
        //ConnectRooms();
    }

    private void BreakableWalls(int padding = 4, TileType tileType = TileType.BreakableWalls)
    {
        foreach (Room room in rooms)
        {
            int maxX = room.xPos + room.roomWidth + padding - 2;
            int maxY = room.yPos + room.roomHeight + padding - 2;
            for (int xCoord = room.xPos - padding; xCoord < maxX + 1; xCoord++)
            {
                for (int yCoord = room.yPos - padding; yCoord < maxY + 1; yCoord++)
                {
                    map[xCoord, yCoord] = tileType;
                }
            }
        }
    }
    private void InnerWallPadding(int padding = 1, TileType tileType = TileType.Edges)
    {
        foreach (Room room in rooms)
        {
            int maxX = room.xPos + room.roomWidth + padding - 2;
            int maxY = room.yPos + room.roomHeight + padding - 2;
            for (int xCoord = room.xPos - padding; xCoord < maxX + 1; xCoord++)
            {
                map[xCoord, room.yPos - padding] = tileType;
                map[xCoord, maxY] = tileType;

            }
            for (int yCoord = room.yPos - padding; yCoord < maxY; yCoord++)
            {
                map[room.xPos - padding, yCoord] = tileType;
                map[maxX, yCoord] = tileType;
            }
        }
    }

    void initializeArray()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                map[x, y] = TileType.Wall;
            }
        }
    }


    private void ClearConnectedRooms()
    {
        foreach (Room room in rooms)
        {
            room.connectedRooms.Clear();
            room.isConnectedToMain = false;
        }
        rooms[0].isConnectedToMain = true;
    }

    private void Intersect()
    {
        bool continueLoop = false;
        int loopCount = 50;
        do
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = i + 1; j < rooms.Count; j++)
                {
                    Room A = rooms[i];
                    Room B = rooms[j];
                    int xCollide = A.intersect(B, 0);
                    int yCollide = A.intersect(B, 1);
                    if (xCollide != 0 && yCollide != 0)
                    {
                        continueLoop = true;
                        if (Mathf.Abs(xCollide) < Mathf.Abs(yCollide))
                        {
                            int shiftA = (int)Math.Floor(xCollide * 0.5);
                            int shiftB = -1 * (xCollide - shiftA);
                            A.SetupRoom(shiftA, 0);
                            B.SetupRoom(shiftB, 0);
                        }
                        else
                        {
                            int shiftA = (int)Math.Floor(yCollide * 0.5);
                            int shiftB = -1 * (yCollide - shiftA);
                            A.SetupRoom(shiftA, 1);
                            B.SetupRoom(shiftB, 1);
                        }
                    }
                }
            }
            loopCount--;
            if (loopCount <= 0)
            {
                continueLoop = false;
            }
        } while (continueLoop);
    }

    void ConnectRooms(bool forceConnectionToMain = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceConnectionToMain)
        {
            foreach (Room room in rooms)
            {
                if (room.isConnectedToMain)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = rooms;
            roomListB = rooms;
        }
        float optimalDistance = 0;
        bool foundPath = false;
        Utils.Coord optimaslACoord = new Utils.Coord();
        Utils.Coord optimaslBCoord = new Utils.Coord();
        Room optimalARoom = new Room();
        Room optimalBRoom = new Room();
        foreach (Room A in roomListA)
        {
            if (!forceConnectionToMain)
            {
                foundPath = false;
                if (A.connectedRooms.Count > 0)
                {
                    continue;
                }
            }
            foreach (Room B in roomListB)
            {
                if (A == B || A.isConnected(B))
                {
                    continue;
                }
                for (int indexA = 0; indexA < A.edges.Count; indexA++)
                {
                    for (int indexB = 0; indexB < B.edges.Count; indexB++)
                    {
                        Utils.Coord aCoord = A.edges[indexA];
                        Utils.Coord bCoord = B.edges[indexB];
                        float distance = (Mathf.Pow(aCoord.x - bCoord.x, 2) + Mathf.Pow(aCoord.y - bCoord.y, 2));
                        if (distance < optimalDistance || !foundPath)
                        {
                            optimalDistance = distance;
                            optimaslACoord = aCoord;
                            optimaslBCoord = bCoord;
                            optimalARoom = A;
                            optimalBRoom = B;
                            foundPath = true;
                        }
                    }
                }
            }
            if (foundPath && !forceConnectionToMain)
            {
                CreatePassage(optimalARoom, optimalBRoom, optimaslACoord, optimaslBCoord);
            }
        }
        if (foundPath && forceConnectionToMain)
        {
            CreatePassage(optimalARoom, optimalBRoom, optimaslACoord, optimaslBCoord);
            ConnectRooms(true);
        }
        if (!forceConnectionToMain)
        {
            ConnectRooms(true);
        }
    }
    private void initCorridors()
    {

        foreach (Corridor corridor in corridors)
        {
            int a = 0;
            int size = corridor.corridorTiles.Count - 1;
            foreach (Utils.Coord coord in corridor.corridorTiles)
            {
                if (a == 0 || a == size)
                {
                    map[coord.x, coord.y] = TileType.Door;
                }
                else
                {
                    map[coord.x, coord.y] = TileType.Corridor;
                }
                a++;
            }

        }
    }
    private void CreatePassage(Room optimalARoom, Room optimalBRoom, Utils.Coord optimaslACoord, Utils.Coord optimaslBCoord)
    {
        Room.connectRooms(optimalARoom, optimalBRoom);
        corridors.Add(new Corridor(optimaslACoord.x, optimaslACoord.y, optimaslBCoord.x, optimaslBCoord.y));
    }
    void CreateRooms()
    {
        int size = numRooms.Random;

        rooms = new List<Room>();
        rooms.Add(new Room());
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);
        for (int i = 1; i < size; i++)
        {
            rooms.Add(new Room());
            rooms[i].SetupRoom(roomWidth, roomHeight, coordinates.Random, coordinates.Random, 0);
        }
    }
    void InitializeTyles()
    {
        foreach (Room currentRoom in rooms)
        {
            for (int j = 0; j < currentRoom.roomWidth - 1; j++)
            {
                int xCoord = currentRoom.xPos + j;
                for (int k = 0; k < currentRoom.roomHeight - 1; k++)
                {
                    int yCoord = currentRoom.yPos + k;
                    if (xCoord > 0 && xCoord < columns && yCoord > 0 && yCoord < rows)
                        map[xCoord, yCoord] = TileType.Floor;
                }
            }
        }
    }
    void PlaceGameobject()
    {
        if (map != null)
        {
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (map[x, y] == TileType.Door)
                    {
                        InstantiateFromArray(mDoor, x, y);
                    }
                    //InstantiateFromArray(mFloor, x, y);
                    if (map[x, y] == TileType.Floor || map[x,y]==TileType.Corridor)
                     {
                         InstantiateFromArray(mFloor, x, y);
                     }
                    if (map[x, y] == TileType.Edges)
                    {
                        InstantiateFromArray(mWall, x, y);
                    }
                    
                }
            }
        }
    }

    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = UnityEngine.Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = mBoardHolder.transform;
    }
    /*
    void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    switch (map[x, y])
                    {
                        case TileType.Floor:
                            Gizmos.color = Color.black;
                            break;
                        case TileType.Corridor:
                            Gizmos.color = Color.blue;
                            break;
                        case TileType.Edges:
                            Gizmos.color = Color.cyan;
                            break;
                        case TileType.Door:
                            Gizmos.color = Color.green;
                            break;
                        case TileType.UnbreakableWalls:
                            Gizmos.color = Color.magenta;
                            break;
                        case TileType.BreakableWalls:
                            Gizmos.color = Color.gray;
                            break;
                        default:
                            Gizmos.color = Color.white;
                            break;
                    }
                    Vector3 pos = new Vector3(-columns / 2 + .5f + x, 0, -rows / 2 + .5f + y);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }*/

}

