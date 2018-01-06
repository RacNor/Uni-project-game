using System;

public class Utils
{

    public Utils()
    {
    }
    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
    public class Coord :IComparable<Coord>, IEquatable<Coord>
    {
        public int x;
        public int y;
        public int score;
        public Coord cameFrom=null;
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
            cameFrom=null;
        }
        public Coord(int x, int y, side coordSide)
        {
            this.x = x;
            this.y = y;
            this.coordSide = coordSide;
        }

        public int CompareTo(Coord other)
        {
            if (this.x.CompareTo(other.x) != 0)
            {
                return this.x.CompareTo(other.x);
            }
            if (this.y.CompareTo(other.y) != 0)
            {
                return this.y.CompareTo(other.y);
            }
            return 0;
        }

        public bool Equals(Coord other)
        {
            return this.x == other.x && this.y == other.y;
        }
    }
    
}


