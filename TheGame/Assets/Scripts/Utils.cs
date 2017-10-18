using System;


public class Utils
{

    public Utils()
    {
    }
    public class Coord
    {
        public int x;
        public int y;
        public side coordSide;
        public enum side
        {
            left, top, right, bottom,
        }
        public Coord()
        {

        }
        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Coord(int x, int y, side coordSide)
        {
            this.x = x;
            this.y = y;
            this.coordSide = coordSide;
        }
    }
}


