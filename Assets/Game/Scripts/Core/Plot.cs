using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Unity.Mathematics;
using YG;
using UnityEngine.Audio;

public class Plot : MonoBehaviour
{
    public bool[] isEventDone;
    public bool isStart;
    public bool isEnd;
    public Text[] FurtherText;
    public Text[] eventText;
    public Text[] rewardText;
    public Text[] ScoreTextEvent;
    public string[] plusminusString;
    public Text startGameText;
    public Text languageText;
    public GameEventAnimation[] eventPanels;
    public float[] scoreCoefficent;
    public int total;
    public float moneyReward;

    public GameObject fingerImg;
    [SerializeField] private Vector2 FingerStartPosition;

    public int Total
    {
        get => total;
        set
        {

            total = value;

            CheckAndTriggerEvents();
        }
    }
    private void CheckAndTriggerEvents()
    {
        if (isEventDone == null)
        {
            Debug.LogError("isEventDone array is not initialized.");
            return;
        }

        int requiredClicks = 400;
        int increment = 2000;

        for (int i = 0; i < isEventDone.Length; i++)
        {
            if (total < requiredClicks)
                break;

            if (!isEventDone[i])
            {
                PlotEvent(i);
            }

            requiredClicks += increment;
        }
    }

    private Game _game;

    public void Awake()
    {
        _game = GameSingleton.Instance.Game;

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

    }
    void StartGameEvent()
    {
        if (!isStart)
            GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.LOADING_MENU);
        else 
            GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.MAIN_GAME);
    }
    public void StartGame()
    {
        GameSingleton.Instance.SoundManager.CreateSound()
                                           .WithSoundData(SoundEffect.CLICK_START_BUTTON)
                                           .Play();

        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.MAIN_GAME);

        isStart = true;
        Instantiate(fingerImg, FingerStartPosition, Quaternion.identity);
    }

    public void OpenMobileTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OpenPcTutorial()
    {
        SceneManager.LoadScene("PC Tutorial");
    }

    public void PlotEvent(int index)
    {
        GameSingleton.Instance.SoundManager.CreateSound()
                                           .WithSoundData(SoundEffect.EVENT_NOTIFICATION)
                                           .Play();

        eventPanels[index].ShowPanel();
        eventText[index].text = LanguageSystem.lng.events[index];
        rewardText[index].text = plusminusString[index] + StringMethods.FormatMoney(CounterMoney(index));
        moneyReward = CounterMoney(index);
    }

    public void RewardEvent(int index)
    {
        eventPanels[index].HidePanel();
        isEventDone[index] = true;
        if (index == 0 || index == 4) _game.Score += moneyReward;
        else _game.Score -= moneyReward;

        GameSingleton.Instance.SoundManager.CreateSound()
                                           .WithSoundData(SoundEffect.COLLECT_REWARD)
                                           .Play();
    }
    public float CounterMoney(int index)
    {
        if (index == 0 || index == 4) return _game.ScoreIncrease * scoreCoefficent[index];
        else
        {
            Debug.Log((long)(_game.Score * scoreCoefficent[index]));
            return (long)(_game.Score * scoreCoefficent[index]);
        }
    }

    public void ResumeGame()
    {
        isEnd = true;

    }
}