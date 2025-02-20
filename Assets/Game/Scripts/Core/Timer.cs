using UnityEngine;
using UnityEngine.UI;
using YG;

public class Timer : MonoBehaviour, ISaveLoad
{
    public Text clickPerSec;
    
    private float BPStimer = 0;
    
    private Game _game;
    private Boost _boost;
    private Achievements _achievements;

    private float mouseClicks = 0;
    private float maxResult; //для ачивки

    private void Awake()
    {
        _game = GameSingleton.Instance.Game;
        _boost = GameSingleton.Instance.Boost;
        _achievements = GameSingleton.Instance.Achievements;
    }

    public void Save()
    {
        ref var data = ref YandexGame.savesData.timerData;

        if (data == null)
        {
            data = new TimerData(maxResult);
            return;
        }

        data.maxResult = maxResult;
    }
    public void Load()
    {
        var data = YandexGame.savesData.timerData;
        
        if (data == null) return;

        maxResult = data.maxResult;
    }


    public float MouseClicks
    {
        get => mouseClicks;
        set
        {
            mouseClicks = value;
            if (mouseClicks >= maxResult) maxResult = mouseClicks;

            if (mouseClicks < 30) _achievements.resultTexts[1].text = maxResult.ToString() + "/30";
            else if (mouseClicks >= 30 && !_achievements.isAchievementDone[1])
            {
                _achievements.CompleteAchievement(1);
            }
            if (_achievements.isAchievementDone[1]) _achievements.resultTexts[1].text = "";

        }
    }
   

    void Start()
    {
        clickPerSec.text = StringMethods.FormatMoney(MouseClicks * _game.ScoreIncrease + _game.TotalPassiveBonus) + LanguageSystem.lng.time[1];
    }
    public void ChangeLanguage()
    {
        clickPerSec.text = StringMethods.FormatMoney(MouseClicks * _game.ScoreIncrease + _game.TotalPassiveBonus) + LanguageSystem.lng.time[1];
    }

    void Update()
    {
        BPStimer += Time.deltaTime;
        if (BPStimer >= 1)
        {
            clickPerSec.text = StringMethods.FormatMoney(MouseClicks * _game.ScoreIncrease + _game.TotalPassiveBonus) + LanguageSystem.lng.time[1];

            if (_boost.IsBoostActive)
            {
                clickPerSec.text = StringMethods.FormatMoney(MouseClicks * _game.ScoreIncrease * 3 + _game.TotalPassiveBonus) + LanguageSystem.lng.time[1]; //на время, когда работает буст
            }
            BPStimer = 0f;
            MouseClicks = 0;
        }
    }

    public void OnClick()
    {
        if (BPStimer < 1)
        {
            MouseClicks++;
        }
    }
}


