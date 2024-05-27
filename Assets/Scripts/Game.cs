using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static Game instance;
    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Game>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(Game)).AddComponent<Game>();
                }
            }
            return instance;
        }

    }


    [Header("Текст, отвечающий за отображение денег")]
    public TextMeshProUGUI[] scoreText;
    [Header("Магазин")]
    public List<Item> shopItems = new List<Item>();
    [Header("Текст на кнопках товаров")]
    public Text[] shopItemsText;
    public Text[] levelOfItemText;
    public Text[] bonusIncreaseText;
    public Image[] coinForBttns;
    public Sprite[] shopBttnSpritesOpen;
    public Sprite[] shopBttnSpritesClose;
    public Sprite coinOpen;
    public Sprite coinClose;
    public Text[] BonusNameText;
    private string fileName;
    [Header("Кнопки товаров")]
    public Button[] shopBttns;
    [Header("Панельки магазина")]
    public GameObject shopPan;
    public GameObject secPan;
    public GameObject skinPan;
    public GameObject infoPan;
    public Animator infoPanAnimator;
    public Image newImg1;
    public Image newImg2;
    public Image newImg3;
    public TextMeshProUGUI dohodSec;
    public TextMeshProUGUI dohodclick;
    public Text offlimemaximum;
    public Text MaximumLimitText;
    public Fortune fort;
    public Achievements achieve;
    public LanguageSystem language;
    public Plot plot;
    public Text collectText;
    public Text collectTextx2;
    public Text currentJob;
    public Boost bst;
    public Text offlineText;
    public Text currentCard;
    public Text infoAboutCount;
    public Text totalClickText;
    public Text DohodOfflineText;
    public Text absenceText;
    public int totalClick = 0;
    public AudioSource AudioBuy;
    public AudioSource FlyPanel;
    public GameObject moneyPref;


    public int maxResult; //для ачивки номер 3

    public int TotalClick
    {
        get => totalClick;
        set
        {

            totalClick = value;
            if (totalClick < 1000) achieve.resultTexts[0].text = totalClick.ToString() + "/1000";
            else if (totalClick >= 1000 && !achieve.isAchievementDone[0]) achieve.CompleteAchievement(0);
            if (achieve.isAchievementDone[0]) achieve.resultTexts[0].text = "";
        }
    }

    public int clickNum;
    public Text offlineDohod;
    public float offlineTime = 7200;
    public DateTime date = new DateTime();
    public float offlineBonus;
    public float totalBonusPS = 0;
    public SpawnDown spawnDown;
    public int clicks;
    public ParticleSystem firePointer;
    public int Clicks
    {
        get => clicks;
        set
        {
            clicks = value;
            spawnDown.clicKnumer.text = clicks == 0 ? "" : clicks.ToString();

            if (clicks >= maxResult) maxResult = clicks;
            if (clicks < 50) achieve.resultTexts[3].text = maxResult.ToString() + "/50";
            else if (clicks >= 50 && !achieve.isAchievementDone[3]) achieve.CompleteAchievement(3); // поменять на 50
            if (achieve.isAchievementDone[3]) achieve.resultTexts[3].text = "";

        }
    }
    public int colClicks; //чтобы в инспекторе менять
    public int ColClicks
    {
        get => colClicks;
        set
        {
            colClicks = value;

            if (colClicks > 1400)
            {
                firePointer.startSpeed = 0.7f;
            }
            else
            {
                firePointer.startSpeed = 0.25f;
            }

            if (colClicks >= 1500 && !bst.boostOn)
            {
                spawnDown.Aсtivate(); // MoneySpawner(твой SpawnDown) при 1000 кликов активирует спавн монет
                colClicks = 0;
            }

            spawnDown.progressSlider.value = colClicks; // Пусть когда клики меняются, то они сразу меняют то, что от них зависит
        }
    }

    private bool isFinalEventCalled = false; //для вызова метода FinalEvent

    public float _score; //private
    public float Score
    {
        get { return _score; }
        set
        {
            _score = value;

            NewImg1();
            NewImg2();
            MoneyScore();

            if (_score < 1000000) achieve.resultTexts[5].text = StringMethods.FormatMoney(_score) + "/1M";
            else if (_score >= 1000000 && !achieve.isAchievementDone[5]) achieve.CompleteAchievement(5);
            if (achieve.isAchievementDone[5]) achieve.resultTexts[5].text = "";

            if (_score >= 5000000000000 && !plot.isEnd && !isFinalEventCalled)
            {
                plot.FinalEvent();
                isFinalEventCalled = true;
            }

            for (int i = 0; i < plot.ScoreTextEvent.Length; i++) plot.ScoreTextEvent[i].text = StringMethods.FormatMoney(Score);

        }
    }
    private float scrCoins;
    private float crrntCost;
    public float scoreIncrease = 1;

    public float ScoreIncrease
    {
        get => scoreIncrease;

        set
        {
            scoreIncrease = value;
            dohodclick.text = StringMethods.FormatMoney(ScoreIncrease) + LanguageSystem.lng.revenueper[1];
        }
    }

    public float passiveBonusPerSec = 0;

    public float PassiveBonusPerSec
    {
        get => passiveBonusPerSec;

        set
        {
            passiveBonusPerSec = value;
            dohodSec.text = StringMethods.FormatMoney(passiveBonusPerSec) + LanguageSystem.lng.revenueper[0];

        }
    }
    private ClickObj[] clickTextPool = new ClickObj[50];

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        language.Initialize();//должно быть выше всех в авейке
        plot.Awake(); // чтобы булева переменная загружалась раньше Score
        fileName = "mainGame.BIN";
        if (File.Exists(Application.persistentDataPath + "/Saves/" + fileName))
        {
            SavedData data = MyLoad.LoadFileBinary<SavedData>(fileName);
            Score = data.score;
            scrCoins = data.scrCoins;
            shopItems = data.shopItems;
            ScoreIncrease = data.scoreIncrease;
            date = data.date;
            offlineTime = data.offlineTime;
            TotalClick = data.totalClick;
            ColClicks = data.colClicks;
            Clicks = data.clicks;
            maxResult = data.maxResult;
            offlineBonus = data.OfflineBonus;

            totalBonusPS = 0;
            for (int i = 0; i < shopItems.Count; i++)
            {
                totalBonusPS += shopItems[i].bonusPerSec * shopItems[i].bonusCounter;
            }
            TimeSpan ts = DateTime.UtcNow - date;

            if ((int)ts.TotalSeconds < 60)
            {
                absenceText.text = ts.Seconds + LanguageSystem.lng.time[7];
            }
            else if ((int)ts.TotalSeconds < 3600)
            {
                absenceText.text = ts.Minutes + LanguageSystem.lng.time[2];
            }
            else if ((int)ts.TotalSeconds < 86400)
            {
                absenceText.text = ts.Hours + LanguageSystem.lng.time[0] + ts.Minutes + LanguageSystem.lng.time[2];
            }
            else if ((int)ts.TotalSeconds < 2592000)
            {
                absenceText.text = ts.Days + LanguageSystem.lng.time[6] + ts.Hours + LanguageSystem.lng.time[0];
            }

            if ((int)ts.TotalSeconds >= offlineTime)
            {
                FlyPanel.Play();
                offlineBonus += (offlineTime * totalBonusPS);
                infoPanAnimator.SetTrigger("open");
            }

            if ((int)ts.TotalSeconds > 45 && (int)ts.TotalSeconds < offlineTime)
            {
                FlyPanel.Play();
                infoPanAnimator.SetTrigger("open");
                offlineBonus += ((int)ts.TotalSeconds * totalBonusPS);
            }
            DohodOfflineText.text = StringMethods.FormatMoney(offlineBonus);
        }
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        UpdateCosts();
        StartCoroutine(BonusPerSec());
        float tmp = 0;
        for (int i = 0; i < shopItems.Count; i++)
        {
            tmp += (shopItems[i].bonusCounter * shopItems[i].bonusPerSec);
        }
        PassiveBonusPerSec = tmp;

        if (clicks < 10) achieve.resultTexts[3].text = maxResult.ToString() + "/50";
        if (achieve.isAchievementDone[3]) achieve.resultTexts[3].text = "";

    }

    private void Update()
    {
        totalClickText.text = TotalClick.ToString();
        infoAboutCount.text = LanguageSystem.lng.info[6];
        Inrct();
        BonussNameText();
    }
    public void ChangeLanguage()
    {
        for (int i = 0; i < 20; i++)
        {
            bonusIncreaseText[i].text = LanguageSystem.lng.BonusIncreaseName[i];
        }
        for (int i = 20; i < 40; i++)
        {
            bonusIncreaseText[i].text = LanguageSystem.lng.BonusIncreaseName[i];
        }
        for (int i = 0; i < shopItems.Count; i++)
        {
            BonusNameText[i].text = LanguageSystem.lng.bonusName[i];
        }
        collectText.text = LanguageSystem.lng.info[3];
        collectTextx2.text = LanguageSystem.lng.info[4];
        offlimemaximum.text = LanguageSystem.lng.info[2];
        MaximumLimitText.text = ((offlineTime / 3600) + LanguageSystem.lng.time[0]);
        offlineDohod.text = LanguageSystem.lng.info[0];
        offlineText.text = LanguageSystem.lng.info[1];
        dohodclick.text = StringMethods.FormatMoney(ScoreIncrease) + LanguageSystem.lng.revenueper[1];
        dohodSec.text = StringMethods.FormatMoney(passiveBonusPerSec) + LanguageSystem.lng.revenueper[0];
    }

    public void Offline()
    {
        Score += offlineBonus;
        offlineBonus = 0;
        infoPanAnimator.SetTrigger("close");
    }

    void Inrct()
    {
        for (int i = 0; i < 20; i++)
        {
            if (Score >= shopItems[i].cost)
            {
                shopBttns[i].interactable = true;
                levelOfItemText[i].text = "lvl " + "<color=#f4bc26>" + shopItems[i].levelOfItem.ToString() + "</color>";
                shopItemsText[i].color = new Color32(112, 86, 167, 255);
                coinForBttns[i].sprite = coinOpen;
                bonusIncreaseText[i].color = new Color32(55, 39, 86, 255);
                BonusNameText[i].color = new Color32(76, 55, 117, 255);
                shopBttns[i].GetComponent<Image>().sprite = shopBttnSpritesOpen[i];
            }
            else
            {
                shopBttns[i].interactable = false;
                bonusIncreaseText[i].color = new Color32(33, 33, 33, 255);
                BonusNameText[i].color = new Color32(33, 33, 33, 255);
                levelOfItemText[i].text = "<color=#404040>" + "lvl " + "</color>" + "<color=#404040>" + shopItems[i].levelOfItem.ToString() + "</color>";
                shopItemsText[i].color = new Color32(63, 63, 63, 255);
                coinForBttns[i].sprite = coinClose;
                shopBttns[i].GetComponent<Image>().sprite = shopBttnSpritesClose[i];

            }
        }

        for (int i = 20; i < shopItems.Count; i++)
        {
            if (Score >= shopItems[i].cost)
            {
                shopBttns[i].interactable = true;
                levelOfItemText[i].text = "<color=#6c6c6c>" + "lvl " + "</color>" + "<color=#5f9500>" + shopItems[i].levelOfItem.ToString() + "</color>";
                shopItemsText[i].color = new Color32(106, 166, 0, 255);
                coinForBttns[i].sprite = coinOpen;
                shopBttns[i].GetComponent<Image>().sprite = shopBttnSpritesOpen[i];
            }
            else
            {
                shopBttns[i].interactable = false;
                levelOfItemText[i].text = "<color=#404040>" + "lvl " + "</color>" + "<color=#404040>" + shopItems[i].levelOfItem.ToString() + "</color>";
                shopItemsText[i].color = new Color32(63, 63, 63, 255);
                coinForBttns[i].sprite = coinClose;
                shopBttns[i].GetComponent<Image>().sprite = shopBttnSpritesClose[i];
            }
        }

    }

    void NewImg1() // картинка нью
    {
        for (int i = 0; i < 20; i++)
        {
            if (Score >= shopItems[i].cost && shopItems[i].levelOfItem == 0)
            {
                newImg1.enabled = true;
                return;
            }
        }

        newImg1.enabled = false;
    }
    void NewImg2() // картинка нью
    {
        for (int i = 20; i < 40; i++)
        {
            if (Score >= shopItems[i].cost && shopItems[i].levelOfItem == 0)
            {
                newImg2.enabled = true;
                return;
            }
        }

        newImg2.enabled = false;
    }

    void BonussNameText()
    {
        currentCard.text = LanguageSystem.lng.infoGame[4];

        for (int i = 20; i < shopItems.Count; i++)
        {
            if (shopItems[i].levelOfItem > 0)
            {
                currentCard.text = LanguageSystem.lng.bonusName[i];
            }
        }

        currentJob.text = LanguageSystem.lng.infoGame[3];

        for (int i = 0; i < 20; i++)
        {
            if (shopItems[i].levelOfItem > 0)
            {
                currentJob.text = LanguageSystem.lng.bonusName[i];
            }
        }
    }

    void MoneyScore()
    {
        scrCoins = Score;
        for (int i = 0; i < 5; i++)
        {
            scoreText[i].text = scrCoins.ToString("0");
            scoreText[i].text = StringMethods.FormatMoney(Score);
        }
    }

    public void BuyBttn(int index)
    {
        if (Score >= shopItems[index].cost)
        {
            if (shopItems[index].itsItemPerSec) shopItems[index].bonusCounter++;
            else ScoreIncrease += shopItems[index].bonusIncrease;
            Score -= shopItems[index].cost;
            shopItems[index].cost = Mathf.Round(shopItems[index].cost * shopItems[index].costMultiplier);
            shopItems[index].levelOfItem++;
        }
        AudioBuy.Play();
        UpdateCosts();
    }

    public void UpdateCosts()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            crrntCost = shopItems[i].cost;
            shopItemsText[i].text = StringMethods.FormatMoney(crrntCost);
        }
    }
    IEnumerator BonusPerSec()
    {
        while (true)
        {
            float tmp = 0;
            for (int i = 0; i < shopItems.Count; i++)
            {
                tmp += (shopItems[i].bonusCounter * shopItems[i].bonusPerSec);
                Score += (shopItems[i].bonusCounter * shopItems[i].bonusPerSec);
            }
            PassiveBonusPerSec = tmp;
            yield return new WaitForSeconds(1);
        }
    }
#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause (bool pause) {
         if (pause) {
            Save();
        } else {
            Awake ();
        }     
    }
#else
    private void OnApplicationQuit()
    {
        if (Clicks > 0)
        {
            spawnDown.ClosePan();
        }
        Save();
    }
#endif

    public void OnClick()
    {
        TotalClick++;
        plot.Total++; //счётчик для событий в Plot
        ColClicks++;
        if (bst.BoostOn == false && fort.doo == false)
        {
            Score += ScoreIncrease;
        }
        if (bst.BoostOn == true)
        {
            Instantiate(moneyPref, new Vector2(UnityEngine.Random.Range(-2.5f, 2.5f), 5.4f), Quaternion.identity);
            Score += ScoreIncrease * 3; // когда работает буст, умножаем доход на 3
        }
        if (fort.doo == true)
        {
            Score += ScoreIncrease * 3; // когда работает буст, умножаем доход на 3
        }


    }
    public void Closeeeee()
    {
        infoPanAnimator.SetTrigger("close");
    }


    [Serializable]
    public class Item
    {
        [Tooltip("Название используется на кнопках")]
        public string name;
        [Tooltip("Цена товара")]
        [SerializeField]
        public float cost;
        [Tooltip("Бонус, который добавляется к бонусу при клике")]
        public float bonusIncrease;
        [HideInInspector]
        public int levelOfItem;
        [Space]
        [Tooltip("Множитель для цены")]
        public float costMultiplier;
        [Space]
        [Tooltip("Этот товар даёт бонус в секунду?")]
        public bool itsItemPerSec;
        [Tooltip("Бонус, который даётся в секунду")]
        public float bonusPerSec;
        [HideInInspector]
        public int bonusCounter;
        [Space]
        [Tooltip("Индекс товара, который будет управляться бонусом (Умножается переменная bonusPerSec этого товара)")]
        public int itemIndex;

    }
    private void Save()
    {
        SavedData data = new SavedData(Score, scrCoins, shopItems, ScoreIncrease, offlineTime, TotalClick, colClicks, Clicks, maxResult, offlineBonus);
        MySave.SaveFileBinary(data, fileName);
        Debug.Log(Score);
    }

    [Serializable]
    private class SavedData
    {
        public SavedData(float Score, float ScrCoins, List<Item> ShopItems, float ScoreIncrease, float OfflineTime, int TotalClick, int ColClicks, int Clicks, int MaxResult, float _offlineBonus)
        {
            score = Score;
            scrCoins = ScrCoins;
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
        public float scrCoins;
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
}