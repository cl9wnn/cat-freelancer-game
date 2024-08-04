using System.Collections.Generic;
using System;
using static Game;

[Serializable]
public class GameData
{
    public GameData(float Score, List<Item> ShopItems, float ScoreIncrease, float OfflineTime, int TotalClick, int ColClicks, int Clicks, int MaxResult, float _offlineBonus)
    {
        score = Score;
        shopItems = ShopItems;
        date = DateTime.UtcNow;
        scoreIncrease = ScoreIncrease;
        offlineTime = OfflineTime;
        totalClick = TotalClick;
        colClicks = ColClicks;
        clicks = Clicks;
        maxResult = MaxResult;
        OfflineBonus = _offlineBonus;
    }
    public float score;
    public float scoreIncrease = 1;
    public int totalClick;
    public float offlineTime = 3600;
    public List<Item> shopItems;
    public DateTime date = new DateTime();
    public int colClicks;
    public int clicks;
    public int maxResult;
    public float OfflineBonus;
}