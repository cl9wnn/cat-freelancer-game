using System;

public class BoostData
{
    public int totalCoffeeConsumed; // для ачивки
    public int availableCoffee;
    public float cooldownDuration;
    public DateTime saveDate;
    public bool canWatchAd;

    public BoostData(float longTimer, bool canBoostAd, int totalCoffeeConsumed, int availableCoffee)
    {
        this.cooldownDuration = longTimer;
        this.saveDate = DateTime.UtcNow;
        this.canWatchAd = canBoostAd;
        this.totalCoffeeConsumed = totalCoffeeConsumed;
        this.availableCoffee = availableCoffee;
    }
}