using System;


[Serializable]
public class IntRange
{
    public int mMin;
    public int mMax;

    public IntRange(int min, int max)
    {
        mMin = min;
        mMax = max;
    }
    public int Random
    {
        get { return UnityEngine.Random.Range(mMin, mMax); }
    }
}
