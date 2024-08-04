using System;

[Serializable]
public class FortuneData
{
    public FortuneData(float LongTimer, bool CanAd, float Timer)
    {
        remainingCooldownTime = LongTimer;
        isAdAvailable = CanAd;
        remainingRewardTime = Timer;
        saveDate = DateTime.UtcNow;

    }

    public float remainingCooldownTime;
    public float remainingRewardTime;
    public bool isAdAvailable;
    public DateTime saveDate = new DateTime();
}