using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int xPos;    //The x coordinate top left
    public int yPos;    //The y coordinate top left
    public int roomWidth;
    public int roomHeight;
    public bool isMainRoom;
    public bool isConnectedToMain;
    public List<Room> connectedRooms;
    public List<Utils.Coord> edges;
    public Room()
    {
        connectedRooms = new List<Room>();
        edges = new List<Utils.Coord>();
    }
    public void SetupRoom(IntRange widthRange, IntRange heightRange
        , int columns, int rows)
    {
        isMainRoom = true;
        isConnectedToMain = true;
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;
        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
        yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);
        AddEdges(xPos, yPos, roomWidth, roomHeight);
    }
    public void SetIsConnectedToMain()
    {
        if (!isConnectedToMain)
        {
            isConnectedToMain = true;
            foreach (Room room in connectedRooms)
            {
                room.SetIsConnectedToMain();
            }
        }

    }
    public void SetupRoom(int shift, int shiftCase)
    {
        edges.Clear();
        switch (shiftCase)
        {
            case 0:
                xPos += shift;
                break;
            default:
                yPos += shift;
                break;
        }

        AddEdges(xPos, yPos, roomWidth, roomHeight);
    }
    public void SetupRoom(IntRange widthRange, IntRange heightRange
        , int x, int y, int a)
    {

        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;
        xPos = x;
        yPos = y;
        AddEdges(xPos, yPos, roomWidth, roomHeight);
    }
    void AddEdges(int x, int y, int width, int height)
    {
        int maxX = x + width - 2;
        int maxY = y + height - 2;
        for (int xCoord = x; xCoord < maxX + 1; xCoord++)
        {
            edges.Add(new Utils.Coord(xCoord, y, Utils.Coord.side.top));
            edges.Add(new Utils.Coord(xCoord, maxY, Utils.Coord.side.bottom));
        }
        for (int yCoord = y; yCoord < maxY; yCoord++)
        {
            edges.Add(new Utils.Coord(x, yCoord, Utils.Coord.side.left));
            edges.Add(new Utils.Coord(maxX, yCoord, Utils.Coord.side.right));
        }
    }
    public static void connectRooms(Room A, Room B)
    {
        if (A.isConnectedToMain)
        {
            B.SetIsConnectedToMain();
        }
        if (B.isConnectedToMain)
        {
            A.SetIsConnectedToMain();
        }
        A.connectedRooms.Add(B);
        B.connectedRooms.Add(A);
    }
    public bool isConnected(Room otherRoom)
    {
        return connectedRooms.Contains(otherRoom);
    }
    public int intersect(Room otherRoom, int intersectCase)
    {
        int min1;
        int max1;
        int min2;
        int max2;
        switch (intersectCase)
        {
            case 0:
                min1 = xPos;
                max1 = xPos + roomWidth - 1;
                min2 = otherRoom.xPos;
                max2 = otherRoom.xPos + otherRoom.roomWidth - 1;
                break;
            default:
                min1 = yPos;
                max1 = yPos + roomHeight - 1;
                min2 = otherRoom.yPos;
                max2 = otherRoom.yPos + otherRoom.roomHeight - 1;
                break;
        }
        if ((max1 >= min2) && (min1 <= max2))
        {
            int dist1 = max2 - min1;
            int dist2 = max1 - min2;
            if (dist2 < dist1)
            {
                return (dist2 + 5) * -1;
            }
            else
            {

                return dist1 + 5;
            }
        }
        else
        {
            return 0;
        }

    }
}