using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System.IO;
using System;
using YG;

public class Timer : MonoBehaviour
{
    public Game gmscript;
    private float BPStimer = 0;
    public Text clickPerSec;
    public Achievements achieve;

    private float mouseClicks = 0;
    private float maxResult; //для ачивки

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
            Load();
    }

    private void OnEnable()
    {
        SaveManager.OnSaveEvent += Save;
        SaveManager.OnLoadEvent += Load;
    }
    private void OnDisable()
    {
        SaveManager.OnSaveEvent -= Save;
        SaveManager.OnLoadEvent -= Load;
    }

    private void Save()
    {
        YandexGame.savesData.timerData = new TimerData(maxResult);
    }
    private void Load()
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

            if (mouseClicks < 30) achieve.resultTexts[1].text = maxResult.ToString() + "/30";
            else if (mouseClicks >= 30 && !achieve.isAchievementDone[1])
            {
                achieve.CompleteAchievement(1);
            }
            if (achieve.isAchievementDone[1]) achieve.resultTexts[1].text = "";

        }
    }
    public Boost bts;
    public Fortune fort;

    void Start()
    {
        clickPerSec.text = StringMethods.FormatMoney(MouseClicks * gmscript.ScoreIncrease + gmscript.PassiveBonusPerSec) + LanguageSystem.lng.time[1];
    }
    public void ChangeLanguage()
    {
        clickPerSec.text = StringMethods.FormatMoney(MouseClicks * gmscript.ScoreIncrease + gmscript.PassiveBonusPerSec) + LanguageSystem.lng.time[1];
    }

    void Update()
    {
        BPStimer += Time.deltaTime;
        if (BPStimer >= 1)
        {
            clickPerSec.text = StringMethods.FormatMoney(MouseClicks * gmscript.ScoreIncrease + gmscript.PassiveBonusPerSec) + LanguageSystem.lng.time[1];

            if (bts.BoostOn == true || fort.coffeeRewarded == true)
            {
                clickPerSec.text = StringMethods.FormatMoney(MouseClicks * gmscript.ScoreIncrease * 3 + gmscript.PassiveBonusPerSec) + LanguageSystem.lng.time[1]; //на время, когда работает буст
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


