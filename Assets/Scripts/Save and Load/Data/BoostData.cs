using System;

public class BoostData
{
    public int countOfCoffee; // для ачивки
    public float longTimer;
    public float timer;
    public DateTime date;
    public bool boostOn;
    public bool canBoostAd;

    public BoostData(float longTimer, float timer, bool boostOn, bool canBoostAd, int countOfCoffee)
    {
        this.longTimer = longTimer;
        this.timer = timer;
        this.date = DateTime.UtcNow;
        this.boostOn = boostOn;
        this.canBoostAd = canBoostAd;
        this.countOfCoffee = countOfCoffee;
    }
}