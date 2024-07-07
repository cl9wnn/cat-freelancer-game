using System;

[Serializable]
public class FortuneData
{
    public FortuneData(float LongTimer, bool CanAd, bool Doo, float Timer)
    {
        longTimer = LongTimer;
        canAd = CanAd;
        doo = Doo;
        timer = Timer;
        date = DateTime.UtcNow;

    }

    public float longTimer;
    public float timer;
    public bool canAd;
    public bool doo;
    public DateTime date = new DateTime();
}