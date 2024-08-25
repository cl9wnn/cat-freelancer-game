using System.Collections.Generic;
using System;
using static Game;
using YG;

[Serializable]
public class GameData
{
    public GameData(float Score, List<Item> ShopItems, float ScoreIncrease, float OfflineTime, int TotalClick, int ColClicks, int MaxResult, float _offlineBonus)
    {
        score = Score;
        shopItems = ShopItems;
        date = YandexGame.ServerTime();
        scoreIncrease = ScoreIncrease;
        offlineTime = OfflineTime;
        totalClick = TotalClick;
        colClicks = ColClicks;
        maxResult = MaxResult;
        OfflineBonus = _offlineBonus;
    }
    public float score;
    public float scoreIncrease = 1;
    public int totalClick;
    public float offlineTime = 3600;
    public List<Item> shopItems;
    public long date;
    public int colClicks;
    public int maxResult;
    public float OfflineBonus;
}