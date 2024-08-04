using System;

public class BoostData
{
    public int totalCoffeeConsumed; // для ачивки
    public int availableCoffee;
    public float cooldownDuration;
    public float boostDuration;
    public DateTime saveDate;
    public bool isBoostActive;
    public bool canWatchAd;

    public BoostData(float longTimer, float timer, bool IsBoostActive, bool canBoostAd, int totalCoffeeConsumed, int availableCoffee)
    {
        this.cooldownDuration = longTimer;
        this.boostDuration = timer;
        this.saveDate = DateTime.UtcNow;
        this.isBoostActive = IsBoostActive;
        this.canWatchAd = canBoostAd;
        this.totalCoffeeConsumed = totalCoffeeConsumed;
        this.availableCoffee = availableCoffee;
    }
}