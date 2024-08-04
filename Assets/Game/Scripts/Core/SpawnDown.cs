﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using YG;


public class SpawnDown : MonoBehaviour
{
    [SerializeField] private DropMoneyPanel dropMoneyPanel;
    [SerializeField] private PanelAnimation starsPanel; 

    public FallingCoin falingCoinPrefab;
    public float spawnInterval = 0.5f;
    public Text clicKnumer;
    public Text EarnedMoneyText;
    public Text CollectedStarsCountText;
    public Slider progressSlider;
    public GameObject dropPanel;
    public GameObject itogPanel;
    public int totalObjectsToSpawn = 50;
    public int objectsSpawned = 0;
    public StarAnimation[] stars;
    public GameObject ButtonAccept;
    public Text countdownText;
    public float timerReady = 4;
  
    public int level; //для инспектора

    public Image levelImg;
    public Sprite[] levelSprites;

    public Text levelText;
    public bool isFirstLevel = true;
    public bool isLastLevel = true;
    public GameObject firstLevelBttn;
    public GameObject lastLevelBttn;

    public Text[] authorOfMessage;
    public Text[] timeOfMessage;
    public Text[] messageName;
    public Text firstInfoMessage;
    public Text LastInfoMessage;
    public Text firstAnswerInfoMessage;
    public Text LastAnswerInfoMessage;
    public Text[] StartLvl;
    
    [Header("Coins Spawn Canvas")]
    public Canvas coinsCanvas;

    private Game _game;
    private Settings _settings;
    private Achievements _achievements;

    public int Level
    {
        get => level;
        set
        {
            level = value;
            levelImg.sprite = levelSprites[level];

        }
    }
    
    private void Awake()
    {
        _game = GameSingleton.Instance.Game;
        _settings = GameSingleton.Instance.Settings;
        _achievements = GameSingleton.Instance.Achievements;

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
        YandexGame.savesData.spawnDownData = new SpawnDownData(Level, 0.8f + (Level * 0.3f), spawnInterval, isFirstLevel, isLastLevel);
    }
    private void Load()
    {
        var data = YandexGame.savesData.spawnDownData;

        if (data == null) return;

        Level = data.level;
        spawnInterval = data.spawnInterval;
        isFirstLevel = data.isFirstLevel;
        isLastLevel = data.isLastLevel;
    }
    public void Activate()
    {
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.NONE);

        if (isFirstLevel) StartCoroutine(HandleCoinsSpawningFirstLevel()); // Запускает корутину - обработчик спавна монет
        else if (isLastLevel == true && Level == 5) StartCoroutine(HandleCoinsSpawningLastLevel());
        else StartCoroutine(HandleCoinsSpawning());
    }

    IEnumerator HandleCoinsSpawning() // основная корутина 
    {
        dropMoneyPanel.CountdownDuration = 3;

        levelText.text = LanguageSystem.lng.moneyDrop[6] + "\n<color=#FFDA00>" + LanguageSystem.lng.moneyDrop[Level] + "</color>";
        countdownText.text = LanguageSystem.lng.info[10];
        
        yield return dropMoneyPanel.HandleSubsequentLaunch();

        countdownText.text = "";
        levelText.text = "";
        clicKnumer.text = "0";

        yield return StartCoroutine(SpawningCoins(totalObjectsToSpawn)); // начинаем спавн монет. дожидаемся его завершения.
        yield return new WaitForSeconds(1.0f);
        HandleMoneySpawnCompletion(); // Запускаем обработчик завершения спавна монет
    }
    IEnumerator HandleCoinsSpawningFirstLevel() //корутина для первого лвла
    {
        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.NOTIFICATION_MESSAGE).Play();

        clicKnumer.text = "";
        
        dropMoneyPanel.HandleFirstLaunch();
        
        yield return new WaitForSeconds(1f);
    }
    IEnumerator HandleCoinsSpawningLastLevel() //корутина для последнего лвла
    {
        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.NOTIFICATION_MESSAGE).Play();

        clicKnumer.text = "";

        dropMoneyPanel.HandleEndLaunch();

        yield return new WaitForSeconds(1f);
    }
    public void StartFirstLevel()
    {
        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.SKIP_MESSAGE).Play();

        firstLevelBttn.SetActive(false);
        StartCoroutine(HandleCoinsSpawning());
        isFirstLevel = false;
    }
    public void StartLastLevel()
    {
        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.SKIP_MESSAGE).Play();

        lastLevelBttn.SetActive(false);
        StartCoroutine(HandleCoinsSpawning());
        isLastLevel = false;
    }
    private void HandleMoneySpawnCompletion() // Обработчик завершения спавна
    {
        starsPanel.ShowPanel();

        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.DROP_RESULT_PANEL).Play();

        objectsSpawned = 0;

        CollectedStarsCountText.text = _game.Clicks + "/" + totalObjectsToSpawn.ToString();
        EarnedMoneyText.text = LanguageSystem.lng.info[9] + StringMethods.FormatMoney(_game.ScoreIncrease * _game.Clicks * (10 + 3 * (Level + 1)));

        ShowStars();
    }


    public void ChangeLanguage()
    {
        for (int i = 0; i < 2; i++) authorOfMessage[i].text = LanguageSystem.lng.moneyDrop[8];
        for (int i = 0; i < 2; i++) timeOfMessage[i].text = LanguageSystem.lng.moneyDrop[9];
        for (int i = 0; i < 2; i++) messageName[i].text = LanguageSystem.lng.moneyDrop[7];
        for (int i = 0; i < 2; i++) StartLvl[i].text = LanguageSystem.lng.moneyDrop[14];
        firstInfoMessage.text = LanguageSystem.lng.moneyDrop[10];
        LastInfoMessage.text = LanguageSystem.lng.moneyDrop[11];
        firstAnswerInfoMessage.text = LanguageSystem.lng.moneyDrop[12];
        LastAnswerInfoMessage.text = LanguageSystem.lng.moneyDrop[13];

    }
    IEnumerator SpawningCoins(int maxCoinCount) // Сам спавн монет
    {
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.MINI_GAME);

        if (falingCoinPrefab == null || coinsCanvas == null)
        {
            Debug.LogError("Prefab or Canvas is not assigned.");
            yield break;
        }

        float coinSpeed = 1.1f + (Level * 0.5f);

        FallingCoin newCoin = null;

        for (int i = 0; i < maxCoinCount; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-1.7f, 1.7f), 5.4f);

            newCoin = Instantiate(falingCoinPrefab, spawnPosition, Quaternion.identity, coinsCanvas.transform);
            newCoin.Speed = coinSpeed;
            objectsSpawned++;

            yield return new WaitForSeconds(spawnInterval);
        }
        Debug.Log(newCoin.transform.position.y);
        yield return new WaitWhile(() =>
        (newCoin != null && newCoin.transform.position.y > -5.4f) || (coinsCanvas.transform.childCount != 0));

    }

    void ShowStars()
    {
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.NONE);

        double procent = (double)_game.Clicks / totalObjectsToSpawn;
        int countStars = 0;
        if (procent >= 0.25 && procent < 0.5)
        {
            countStars = 1;
        }
        else if (procent >= 0.5 && procent < 0.75)
        {
            countStars = 2;
        }
        else if (procent >= 0.75 && procent <= 1 && Level < 5)
        {
            countStars = 3;
            Level++;
            spawnInterval -= 0.05f;
        }
        else if (procent >= 0.75 && procent <= 1 && Level >= 5)
        {
            countStars = 3;
            if (!_achievements.isAchievementDone[6])
            {
                _achievements.CompleteAchievement(6);
                _achievements.resultTexts[6].text = "";
            }
        }

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < countStars);
            if (i < countStars) stars[i].AnimateStar();
        }
    }


    public void ClosePan()
    {
        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.COLLECT_REWARD).Play();
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.MAIN_GAME);

        _game.Score += (_game.ScoreIncrease * _game.Clicks * (10 + 3 * Level));
        _game.Clicks = 0;

        starsPanel.HidePanel();
        dropMoneyPanel.HidePanel();
    }
}
