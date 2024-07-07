using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using YG;


public class SpawnDown : MonoBehaviour
{
    public GameObject obj;
    public Game gmscript;
    public float spawnInterval = 0.5f;
    public Text clicKnumer;
    public Text EarnedMoneyText;
    public Text CollectedStarsCountText;
    public Slider progressSlider;
    public GameObject dropPanel;
    public GameObject itogPanel;
    public Animator animator;
    public Animator dropMoneyPanelAnimator;
    public int totalObjectsToSpawn = 50;
    public int objectsSpawned = 0;
    public Animator[] stars;
    public GameObject ButtonAccept;
    public Text countdownText;
    public float timerReady = 4;
    public Achievements achieve;
    public Settings settingss;
    public int level; //для инспектора

    public Image levelImg;

    public Sprite[] levelSprites;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            levelImg.sprite = levelSprites[level];

        }
    }
    public Text levelText;
    public spez spez;
    public SoundFade soundFade;
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

    public AudioSource RewardMoney;
    public AudioSource CaughtCoin;
    public AudioSource ChainsSound;
    public AudioSource Message;
    public AudioSource Whoosh;
    public AudioSource Counter;
    public AudioSource LvlUp;
    public AudioSource PixelMusic;


    private static SpawnDown instance;
    public static SpawnDown Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SpawnDown>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(SpawnDown)).AddComponent<SpawnDown>();
                }
            }
            return instance;
        }

    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

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
        YandexGame.savesData.spawnDownData = new SpawnDownData(Level, spez.speed, spawnInterval, isFirstLevel, isLastLevel);
    }
    private void Load()
    {
        var data = YandexGame.savesData.spawnDownData;

        if (data == null) return;

        Level = data.level;
        spawnInterval = data.spawnInterval;
        isFirstLevel = data.isFirstLevel;
        isLastLevel = data.isLastLevel;
        spez.speed = 0.8f + (Level * 0.3f);
    }
    public void Aсtivate()
    {
        if (isFirstLevel) StartCoroutine(HandleCoinsSpawningFirstLevel()); // Запускает корутину - обработчик спавна монет
        else if (isLastLevel == true && Level == 5) StartCoroutine(HandleCoinsSpawningLastLevel());
        else StartCoroutine(HandleCoinsSpawning());
    }

    IEnumerator HandleCoinsSpawning() // основная корутина 
    {
        if (!isFirstLevel && isLastLevel && level != 5 || !isFirstLevel && !isLastLevel) dropMoneyPanelAnimator.SetTrigger("open");
        yield return StartCoroutine(Countdown(3, 0));
        clicKnumer.text = "0";
        yield return StartCoroutine(SpawningCoins(totalObjectsToSpawn)); // начинаем спавн монет. дожидаемся его завершения.
        HandleMoneySpawnCompletion(); // Запускаем обработчик завершения спавна монет
    }
    IEnumerator HandleCoinsSpawningFirstLevel() //корутина для первого лвла
    {
        Message.Play();
        firstLevelBttn.SetActive(true);
        clicKnumer.text = "";
        dropMoneyPanelAnimator.SetTrigger("firstLvlOpen");
        yield return new WaitForSeconds(1f);
    }
    IEnumerator HandleCoinsSpawningLastLevel() //корутина для последнего лвла
    {
        Message.Play();
        lastLevelBttn.SetActive(true);
        clicKnumer.text = "";
        dropMoneyPanelAnimator.SetTrigger("lastLvlOpen");
        yield return new WaitForSeconds(1f);
    }
    public void StartFirstLevel()
    {
        Whoosh.Play();
        firstLevelBttn.SetActive(false);
        dropMoneyPanelAnimator.SetTrigger("firstLvlClose");
        StartCoroutine(HandleCoinsSpawning());
        isFirstLevel = false;
    }
    public void StartLastLevel()
    {
        Whoosh.Play();
        lastLevelBttn.SetActive(false);
        dropMoneyPanelAnimator.SetTrigger("lastLvlClose");
        StartCoroutine(HandleCoinsSpawning());
        isLastLevel = false;
    }
    private void HandleMoneySpawnCompletion() // Обработчик завершения спавна
    {
        animator.SetTrigger("open");
        ChainsSound.Play();
        objectsSpawned = 0;
        CollectedStarsCountText.text = gmscript.Clicks + "/" + totalObjectsToSpawn.ToString();
        EarnedMoneyText.text = LanguageSystem.lng.info[9] + StringMethods.FormatMoney(gmscript.ScoreIncrease * gmscript.Clicks * (10 + 3 * (Level + 1)));
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
    IEnumerator Countdown(int from, int to)
    {
        {
            levelText.text = LanguageSystem.lng.moneyDrop[6] + "\n<color=#FFDA00>" + LanguageSystem.lng.moneyDrop[Level] + "</color>";
            countdownText.text = LanguageSystem.lng.info[10];
            yield return new WaitForSeconds(2.5f);

            settingss.audioSourceMusic.volume = 0f;
            Counter.Play();

            for (int i = from; i > to; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            countdownText.text = "";
            levelText.text = "";
        }
    }
    IEnumerator SpawningCoins(int maxCount) // Сам спавн монет
    {
        PixelMusic.volume = 1;
        PixelMusic.Play();
        GameObject lastCoin = null;
        while (maxCount-- > 0) // пока максимальное кол-во монет не достигли, спавним новые
        {
            Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(-1.7f, 1.7f), 5.4f);

            lastCoin = Instantiate(obj, spawnPosition, Quaternion.identity);
            objectsSpawned++;

            yield return new WaitForSeconds(spawnInterval); // Ожидаем интервал между спавном
        }
        yield return new WaitWhile(() => lastCoin != null && lastCoin.transform.position.y > -5.4f);

    }

    void ShowStars()
    {
        soundFade.FadeOut();
        ButtonAccept.SetActive(true);
        double procent = (double)gmscript.Clicks / totalObjectsToSpawn;
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
            spez.speed = 0.8f + (Level * 0.3f);
        }
        else if (procent >= 0.75 && procent <= 1 && Level >= 5)
        {
            countStars = 3;
            if (!achieve.isAchievementDone[6])
            {
                achieve.CompleteAchievement(6);
                achieve.resultTexts[6].text = "";
            }
        }

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < countStars);
            if (i < countStars) stars[i].SetTrigger("show");
        }
        LvlUp.Play();
    }


    public void ClosePan()
    {
        settingss.audioSourceMusic.volume = 100f;

        RewardMoney.Play();
        dropMoneyPanelAnimator.SetTrigger("close");
        gmscript.Score += (gmscript.ScoreIncrease * gmscript.Clicks * (10 + 3 * Level));
        gmscript.Clicks = 0;
        animator.SetTrigger("close");
    }
}
