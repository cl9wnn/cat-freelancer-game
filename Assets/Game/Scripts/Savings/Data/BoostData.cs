using System;

public class BoostData
{
    public int totalCoffeeConsumed; // для ачивки
    public int availableCoffee;
    public float cooldownDuration;
    public DateTime saveDate;

    public BoostData(float longTimer, int totalCoffeeConsumed, int availableCoffee)
    {
        this.cooldownDuration = longTimer;
        this.saveDate = DateTime.UtcNow;
        this.totalCoffeeConsumed = totalCoffeeConsumed;
        this.availableCoffee = availableCoffee;
    }
}