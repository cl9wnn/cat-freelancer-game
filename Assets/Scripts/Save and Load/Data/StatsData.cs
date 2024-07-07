using System;

[Serializable]
public class StatsData
{
    public StatsData(float TotalPlayTime)
    {
        totalPlayTime = TotalPlayTime;
    }
    public float totalPlayTime;
}