using System;

[Serializable]
public class SpawnDownData
{
    public SpawnDownData(int Level, float Speed, float SpawnInterval, bool IsFirstLevel, bool IsLastLevel)
    {
        level = Level;
        speed = Speed;
        spawnInterval = SpawnInterval;
        isFirstLevel = IsFirstLevel;
        isLastLevel = IsLastLevel;
    }
    public int level;
    public float speed;
    public float spawnInterval;
    public bool isFirstLevel;
    public bool isLastLevel;
}