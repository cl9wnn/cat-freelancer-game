using System;

[Serializable]
public class TimerData
{
    public TimerData(float MaxResult)
    {
        maxResult = MaxResult;
    }
    public float maxResult;
}