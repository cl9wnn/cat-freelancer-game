using System;
using YG;

public class BoostData
{
    public int totalCoffeeConsumed; // для ачивки
    public int availableCoffee;
    public float cooldownDuration;
    public long saveDate;

    public BoostData(float longTimer, int totalCoffeeConsumed, int availableCoffee)
    {
        this.cooldownDuration = longTimer;
        this.saveDate = YandexGame.ServerTime();
        this.totalCoffeeConsumed = totalCoffeeConsumed;
        this.availableCoffee = availableCoffee;
    }
}