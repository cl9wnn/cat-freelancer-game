using System;

[Serializable]
public class PlotData
{
    public PlotData(bool[] IsEventDone, int Total, bool IsStart, bool IsEnd)
    {
        isEventDone = IsEventDone;
        total = Total;
        isEnd = IsEnd;
        isStart = IsStart;
    }
    public bool[] isEventDone;
    public int total;
    public bool isStart;
    public bool isEnd;
}