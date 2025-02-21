using System;
using YG;

public class BoostData
{
    public int totalCoffeeConsumed; // для ачивки
    public int availableCoffee;
    public float cooldownDuration;
    public long savedTime;

    public BoostData(float longTimer, int totalCoffeeConsumed, int availableCoffee)
    {
        this.cooldownDuration = longTimer;
        this.savedTime = YandexGame.ServerTime();
        this.totalCoffeeConsumed = totalCoffeeConsumed;
        this.availableCoffee = availableCoffee;
    }
}