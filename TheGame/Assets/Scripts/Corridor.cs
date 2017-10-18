using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Corridor
{
    public List<Utils.Coord> corridorTiles;

    public Corridor(int startX, int startY, int endX, int endY)
    {
        corridorTiles = new List<Utils.Coord>();
        int deltaX = endX - startX;
        int deltaY = endY - startY;
        int startingY = startY;
        int endingX = startX;
        bool movedX = false;
        if (deltaX < 0)
        {
            endingX--;
            movedX = true;
            for (; endingX > endX; endingX--)
            {
                corridorTiles.Add(new Utils.Coord(endingX, startingY));
            }
        }
        else if (deltaX > 0)
        {
            endingX++;
            movedX = true;
            for (; endingX < endX; endingX++)
            {
                corridorTiles.Add(new Utils.Coord(endingX, startingY));
            }
        }
        if (deltaY < 0)
        {
            if (!movedX)
            {
                startingY--;
            }
            for (; startingY > endY; startingY--)
            {
                corridorTiles.Add(new Utils.Coord(endingX, startingY));
            }
        }
        else if (deltaY > 0)
        {
            if (!movedX)
            {
                startingY++;
            }
            for (; startingY < endY; startingY++)
            {
                corridorTiles.Add(new Utils.Coord(endingX, startingY));
            }
        }
        //corridorTiles.Add(new Utils.Coord(endingX, startingY));
    }
}

