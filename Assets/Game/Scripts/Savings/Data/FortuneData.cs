using System;
using YG;

[Serializable]
public class FortuneData
{
    public FortuneData(float LongTimer, bool CanAd, float Timer)
    {
        remainingCooldownTime = LongTimer;
        isAdAvailable = CanAd;
        remainingRewardTime = Timer;
        saveDate = YandexGame.ServerTime();

    }

    public float remainingCooldownTime;
    public float remainingRewardTime;
    public bool isAdAvailable;
    public long saveDate;
}