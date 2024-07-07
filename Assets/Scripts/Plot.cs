using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Unity.Mathematics;
using YG;

public class Plot : MonoBehaviour

{
    public Game gmscript;
    public bool[] isEventDone;
    public bool isStart;
    public bool isEnd;
    public GameObject startImg;
    public GameObject endImg;
    public Text endText;
    public Text[] FurtherText;
    public Text[] eventText;
    public Text[] rewardText;
    public Text[] ScoreTextEvent;
    public string[] plusminusString;
    public Text startGameText;
    public Text languageText;
    public GameObject[] eventImg;
    public float[] scoreCoefficent;
    public int total;
    public float moneyReward;
    public AudioSource GetMoney;
    public AudioSource ThrowEvent;
    private float delay = 6;
    public GameObject fingerImg;
    public Settings settingss;
    public int Total
    {
        get => total;
        set
        {

            total = value;

            for (int i = 0, clicks = 600; i < isEventDone.Length; i++, clicks += 2500)
            {
                if (total >= clicks && isEventDone[i] == false) PlotEvent(i);
            }
        }
    }



    public void Awake()
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
        YandexGame.savesData.plotData = new PlotData(isEventDone, Total, isStart, isEnd);
    }
    private void Load()
    {
        var data = YandexGame.savesData.plotData;

        if (data == null) return;

        isStart = data.isStart;
        isEnd = data.isEnd;
        for (int i = 0; i < isEventDone.Length; i++)
        {
            isEventDone[i] = data.isEventDone[i];
        }
        Total = data.total;
    }


    void Start()
    {
        StartGameEvent();
    }
    void Update()
    {
    }
    public void ChangeLanguage()
    {
        for (int i = 0; i < FurtherText.Length; i++)
        {
            FurtherText[i].text = LanguageSystem.lng.ok[i];  //обучение - training
        }
        startGameText.text = LanguageSystem.lng.ok[8]; // начать игру 
        languageText.text = LanguageSystem.lng.ok[9]; // поменять язык

        for (int i = 0; i < eventText.Length; i++)
        {
            eventText[i].text = LanguageSystem.lng.events[i];
        }
        endText.text = LanguageSystem.lng.events[5];

    }
    void StartGameEvent()
    {
        if (isStart == false)
        {
            startImg.SetActive(true);
        }
    }
    public void StartGame()
    {
        settingss.OpenSceneMusic.mute = true;
        settingss.audioSourceMusic.Play();
        startImg.SetActive(false);
        isStart = true;
        Vector2 spawnPosition = new Vector2(-1.8f, 2.05f);
        Instantiate(fingerImg, spawnPosition, Quaternion.identity);
    }

    public void Training()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void PlotEvent(int index)
    {
        ThrowEvent.Play();
        eventImg[index].GetComponent<Animator>().SetTrigger("open");
        eventText[index].text = LanguageSystem.lng.events[index];
        rewardText[index].text = plusminusString[index] + StringMethods.FormatMoney(CounterMoney(index));
        moneyReward = CounterMoney(index);
    }
    public void RewardEvent(int index)
    {
        eventImg[index].GetComponent<Animator>().SetTrigger("close");
        isEventDone[index] = true;
        if (index == 0 || index == 4) gmscript.Score += moneyReward;
        else gmscript.Score -= moneyReward;
        GetMoney.Play();
    }
    public float CounterMoney(int index)
    {
        if (index == 0 || index == 4) return gmscript.ScoreIncrease * scoreCoefficent[index];
        else return gmscript.Score * scoreCoefficent[index];
    }

    public void ResumeGame()
    {
        endImg.SetActive(false);
        isEnd = true;

    }
}