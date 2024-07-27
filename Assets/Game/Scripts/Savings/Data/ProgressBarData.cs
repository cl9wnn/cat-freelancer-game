using System;

[Serializable]
public class ProgressBarData
{
    public ProgressBarData(bool _proydeno, int _level, float maxLevelValue)
    {
        proydeno = _proydeno;
        level = _level;
        this.maxLevelValue = maxLevelValue;
    }
    public bool proydeno;
    public int level;
    public float maxLevelValue;
}