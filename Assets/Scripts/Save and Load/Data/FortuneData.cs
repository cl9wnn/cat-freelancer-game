using System;

[Serializable]
public class FortuneData
{
    public FortuneData(float LongTimer, bool CanAd, bool Doo, float Timer)
    {
        remainingCooldownTime = LongTimer;
        isAdAvailable = CanAd;
        isCoffeeRewarded = Doo;
        remainingRewardTime = Timer;
        saveDate = DateTime.UtcNow;

    }

    public float remainingCooldownTime;
    public float remainingRewardTime;
    public bool isAdAvailable;
    public bool isCoffeeRewarded;
    public DateTime saveDate = new DateTime();
}