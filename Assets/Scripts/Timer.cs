using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System.IO;
using System;

public class Timer : MonoBehaviour
{
    public Game gmscript;
    private float BPStimer = 0;
    public Text clickPerSec;
    public Achievements achieve;
    private string fileName;

    private float mouseClicks = 0;
    private float maxResult; //для ачивки

    private void Awake()
    {
        fileName = "Timer.BIN";
        if (File.Exists(Application.persistentDataPath + "/Saves/" + fileName))
        {
            SavedData data = MyLoad.LoadFileBinary<SavedData>(fileName);
            maxResult = data.maxResult;
        }
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

            if (bts.BoostOn == true || fort.doo == true)
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

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause (bool pause) {
        if (pause)
        {
            SavedData data = new SavedData (maxResult);
           Save(data);
        } else {
            Awake ();
        }
    }
#else
    private void OnApplicationQuit()
    {
        SavedData data = new SavedData(maxResult);
        Save(data);
    }
#endif

    private void Save(object Obj)
    {
        MySave.SaveFileBinary(Obj, fileName);
    }


    [Serializable]
    public class SavedData
    {
        public SavedData(float MaxResult)
        {
            maxResult = MaxResult;
        }
        public float maxResult;
    }
}


