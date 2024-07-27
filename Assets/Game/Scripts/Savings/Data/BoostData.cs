using System;

public class BoostData
{
    public int coffeeCount; // для ачивки
    public float cooldownDuration;
    public float boostDuration;
    public DateTime saveDate;
    public bool isBoostActive;
    public bool canWatchAd;

    public BoostData(float longTimer, float timer, bool IsBoostActive, bool canBoostAd, int countOfCoffee)
    {
        this.cooldownDuration = longTimer;
        this.boostDuration = timer;
        this.saveDate = DateTime.UtcNow;
        this.isBoostActive = IsBoostActive;
        this.canWatchAd = canBoostAd;
        this.coffeeCount = countOfCoffee;
    }
}