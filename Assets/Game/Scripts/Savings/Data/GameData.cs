using System.Collections.Generic;
using System;
using static Game;
using YG;

[Serializable]
public class GameData
{
    public GameData(float Score, List<Item> ShopItems, float ScoreIncrease, float scorePassive, float OfflineTime, int TotalClick, int ColClicks, int MaxResult)
    {
        score = Score;
        shopItems = ShopItems;
        savedTime = YandexGame.ServerTime();
        scoreIncrease = ScoreIncrease;
        this.scorePassive = scorePassive;
        offlineTime = OfflineTime;
        totalClick = TotalClick;
        colClicks = ColClicks;
        maxResult = MaxResult;
    }
    public float score;
    public float scoreIncrease = 1;
    public float scorePassive;
    public int totalClick;
    public float offlineTime = 3600;
    public List<Item> shopItems;
    public long savedTime;
    public int colClicks;
    public int maxResult;
}