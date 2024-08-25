using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Game : MonoBehaviour, ISaveLoad
{
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
    [Header("Кнопки товаров")]
    public Button[] shopBttns;
    [Header("Панельки магазина")]
    public GameObject shopPan;
    public GameObject secPan;
    public GameObject skinPan;
    public GameObject infoPan;
    public PanelAnimation offlineEarningPanel;
    public Image _jobAlertIcon;
    public Image _graphicCardAlertIcon;
    public Image newImg3;
    public TextMeshProUGUI dohodSec;
    public TextMeshProUGUI dohodclick;
    public Text offlimemaximum;
    public Text MaximumLimitText;
    public Text collectText;
    public Text collectTextx2;
    public Text currentJob;
    public Text offlineText;
    public Text currentCard;
    public Text totalClickText;
    public Text DohodOfflineText;
    public Text absenceText;
    public int totalClick = 0;
    public GameObject moneyPref;

    public Canvas moneyPopupCanvas;

    [SerializeField] private bool IsMobile;

    public int maxResult; //для ачивки номер 3

    public int TotalClick
    {
        get => totalClick;
        set
        {

            totalClick = value;
            if (totalClick < 1000) _achievements.resultTexts[0].text = totalClick.ToString() + "/1000";
            else if (totalClick >= 1000 && !_achievements.isAchievementDone[0]) _achievements.CompleteAchievement(0);
            if (_achievements.isAchievementDone[0]) _achievements.resultTexts[0].text = "";
        }
    }

    public int clickNum;
    public Text offlineDohod;
    public float offlineTime = 7200;
    public float offlineBonus;
    public float totalBonusPS = 0;
    public int clicks;
    public ParticleSystem firePointer;

    private Achievements _achievements;
    private SpawnDown _spawnDown;
    private Boost _boost;
    private Plot _plot;

    public int Clicks
    {
        get => clicks;
        set
        {
            clicks = value;
            _spawnDown.clicKnumer.text = clicks == 0 ? "" : clicks.ToString();

            if (clicks >= maxResult) maxResult = clicks;
            if (clicks < 50) _achievements.resultTexts[3].text = maxResult.ToString() + "/50";
            else if (clicks >= 50 && !_achievements.isAchievementDone[3]) _achievements.CompleteAchievement(3); // поменять на 50
            if (_achievements.isAchievementDone[3]) _achievements.resultTexts[3].text = "";

        }
    }
    public int colClicks; //чтобы в инспекторе менять
    public int ColClicks
    {
        get => colClicks;
        set
        {
            colClicks = value;
            UpdateFirePointerSpeed();
            CheckAndActivateBoost();
            UpdateProgressSlider();
        }
    }

    private void UpdateFirePointerSpeed()
    {
        firePointer.startSpeed = colClicks > 1400 ? 1 : 0.25f;
    }
    private void CheckAndActivateBoost()
    {
        if (colClicks >= 1500 && !_boost.IsBoostActive)
        {
            _spawnDown.Activate();
            colClicks = 0;
        }
    }
    private void UpdateProgressSlider()
    {
        _spawnDown.progressSlider.value = colClicks;
    }

    public float _score; //private
    public float Score
    {
        get => _score;
        set
        {
            _score = value;

            UpdateUI();
            CheckAchievements();
            UpdateScoreTextEvents();
        }
    }

    private void UpdateUI()
    {
        UpdateJobAlertIcon();
        UpdateGraphicsCardAlertIcon();
        MoneyScore();
    }

    private void CheckAchievements() // TODO: remove
    {
        if (_score < 1000000)
        {
            _achievements.resultTexts[5].text = StringMethods.FormatMoney(_score) + "/1M";
        }
        else if (_score >= 1000000 && !_achievements.isAchievementDone[5])
        {
            _achievements.CompleteAchievement(5);
        }

        if (_achievements.isAchievementDone[5])
        {
            _achievements.resultTexts[5].text = "";
        }
    }

    private void UpdateScoreTextEvents()
    {
        string formattedScore = StringMethods.FormatMoney(_score);
        foreach (var scoreText in _plot.ScoreTextEvent)
        {
            scoreText.text = formattedScore;
        }
    } // TODO: remove

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

    private void Awake()
    {
        _achievements = GameSingleton.Instance.Achievements;
        _spawnDown = GameSingleton.Instance.SpawnDown;
        _boost = GameSingleton.Instance.Boost;
        _plot = GameSingleton.Instance.Plot;
    }

    public void Save()
    {
        YandexGame.NewLeaderboardScores("ScoreLeaderboard", (int)Score);
        
        ref var data = ref YandexGame.savesData.gameData;

        if (data == null)
        {
            data = new GameData(Score, shopItems, ScoreIncrease, offlineTime, TotalClick, colClicks, maxResult, offlineBonus);
            return;
        }

        data.score = Score;
        data.shopItems = shopItems;
        data.scoreIncrease = ScoreIncrease;
        data.offlineTime = offlineTime;
        data.date = YandexGame.ServerTime();
        data.totalClick = TotalClick;
        data.colClicks = colClicks;
        data.maxResult = maxResult;
        data.OfflineBonus = offlineBonus;
    }
    public void Load()
    {
        var data = YandexGame.savesData.gameData;

        if (data == null) return;

        Score = data.score;
        shopItems = data.shopItems;
        scoreIncrease = data.scoreIncrease;
        offlineTime = data.offlineTime;
        TotalClick = data.totalClick;
        ColClicks = data.colClicks;
        maxResult = data.maxResult;
        offlineBonus = data.OfflineBonus;

        totalBonusPS = 0;
        for (int i = 0; i < shopItems.Count; i++)
        {
            totalBonusPS += shopItems[i].bonusPerSec * shopItems[i].bonusCounter;
        }

        var milliseconds = YandexGame.ServerTime() - data.date;

        var seconds = milliseconds / 1000;
        var minutes = seconds / 60;
        var hours = minutes / 60;
        var days = hours / 24;

        absenceText.text = (seconds, minutes, hours, days) switch
        {
            var (s, _, _, _) when s < 60 => s + LanguageSystem.lng.time[7],
            var (_, m, _, _) when m < 60 => m + LanguageSystem.lng.time[2],
            var (_, m, h, _) when h < 24 => h + LanguageSystem.lng.time[0] + m + LanguageSystem.lng.time[2],
            var (_, _, h, d) when d < 30 => d + LanguageSystem.lng.time[6] + h + LanguageSystem.lng.time[0],
            _ => string.Empty 
        };

        MaximumLimitText.text = (offlineTime / 3600) + LanguageSystem.lng.time[0];

        if (seconds >= offlineTime)
        {
            offlineBonus += (offlineTime * totalBonusPS);
            if (offlineBonus <= 0.01f) return;  
        }
        else if (seconds > 30)
        {
            offlineBonus += (seconds * totalBonusPS);
            if (offlineBonus <= 0.01f) return;
        }

    }

    private void Start()
    {
        dohodclick.text = StringMethods.FormatMoney(ScoreIncrease) + LanguageSystem.lng.revenueper[1];

        if (offlineBonus > 0.01f) 
        {
            GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.DROP_DAILY_REWARD_PANEL).Play();
            DohodOfflineText.text = StringMethods.FormatMoney(offlineBonus);
            offlineEarningPanel.ShowPanel();

        }

        UpdateCosts();
        StartCoroutine(BonusPerSec());
        float tmp = 0;
        for (int i = 0; i < shopItems.Count; i++)
        {
            tmp += (shopItems[i].bonusCounter * shopItems[i].bonusPerSec);
        }
        PassiveBonusPerSec = tmp;

        if (clicks < 10) _achievements.resultTexts[3].text = maxResult.ToString() + "/50";
        if (_achievements.isAchievementDone[3]) _achievements.resultTexts[3].text = "";

    }

    private void Update()
    {
        totalClickText.text = TotalClick.ToString();
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
        MaximumLimitText.text = (offlineTime / 3600) + LanguageSystem.lng.time[0];
        offlineDohod.text = LanguageSystem.lng.info[0];
        offlineText.text = LanguageSystem.lng.info[1];
        dohodclick.text = StringMethods.FormatMoney(ScoreIncrease) + LanguageSystem.lng.revenueper[1];
        dohodSec.text = StringMethods.FormatMoney(passiveBonusPerSec) + LanguageSystem.lng.revenueper[0];
    }

    public void Offline()
    {
        Score += offlineBonus;
        offlineBonus = 0;

        offlineEarningPanel.HidePanel();

        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.COLLECT_MONEY).Play();
    }

    public void GetOfflineIncomeWithMultiplier(float multiplier)
    {
        Score += offlineBonus * multiplier;
        offlineBonus = 0;

        offlineEarningPanel.HidePanel();

        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.COLLECT_MONEY).Play();
    }

    void Inrct()
    {
        for (int i = 0; i < 20; i++)
        {
            if (Score >= shopItems[i].cost)
            {
                shopBttns[i].interactable = true;
                levelOfItemText[i].text = LanguageSystem.lng.revenueper[6] + " <color=#f4bc26>" + shopItems[i].levelOfItem.ToString() + "</color>";
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
                levelOfItemText[i].text = "<color=#404040>" + LanguageSystem.lng.revenueper[6] + " </color>" + "<color=#404040>" + shopItems[i].levelOfItem.ToString() + "</color>";
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
                levelOfItemText[i].text = "<color=#6c6c6c>" + LanguageSystem.lng.revenueper[6] + " </color>" + "<color=#5f9500>" + shopItems[i].levelOfItem.ToString() + "</color>";
                shopItemsText[i].color = new Color32(106, 166, 0, 255);
                coinForBttns[i].sprite = coinOpen;
                shopBttns[i].GetComponent<Image>().sprite = shopBttnSpritesOpen[i];
            }
            else
            {
                shopBttns[i].interactable = false;
                levelOfItemText[i].text = "<color=#404040>" + LanguageSystem.lng.revenueper[6] + " </color>" + "<color=#404040>" + shopItems[i].levelOfItem.ToString() + "</color>";
                shopItemsText[i].color = new Color32(63, 63, 63, 255);
                coinForBttns[i].sprite = coinClose;
                shopBttns[i].GetComponent<Image>().sprite = shopBttnSpritesClose[i];
            }
        }

    }

    void UpdateJobAlertIcon() // картинка нью
    {
        for (int i = 0; i < 20; i++)
        {
            if (Score >= shopItems[i].cost && shopItems[i].levelOfItem == 0)
            {
                _jobAlertIcon.enabled = true;
                return;
            }
        }
        _jobAlertIcon.enabled = false;
    }
    void UpdateGraphicsCardAlertIcon() // картинка нью
    {
        for (int i = 20; i < 40; i++)
        {
            if (Score >= shopItems[i].cost && shopItems[i].levelOfItem == 0)
            {
                _graphicCardAlertIcon.enabled = true;
                return;
            }
        }

        _graphicCardAlertIcon.enabled = false;
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

    public Item GetNextGraphicsCard()
    {
        var index = shopItems.FindLastIndex(item => !item.itsItemPerSec && item.levelOfItem > 0);

        return shopItems[Mathf.Clamp(index + 1, 20, shopItems.Count - 1)];
    }

    void MoneyScore()
    {
        var moneyText = StringMethods.FormatMoney(Score, wideText: true);

        scoreText[0].text = moneyText + "<sprite=0>";

        for (int i = 1; i < 5; i++)
        {
            scoreText[i].text = moneyText;
        }
    }

    public void BuyBttn(int index)
    {
        if (Score >= shopItems[index].cost)
        {
            if (shopItems[index].itsItemPerSec) 
                shopItems[index].bonusCounter++;
            else
                ScoreIncrease += shopItems[index].bonusIncrease;
            
            Score -= shopItems[index].cost;
            
            shopItems[index].cost = StringMethods.ParseFormattedCost(Mathf.Round(shopItems[index].cost * shopItems[index].costMultiplier));
            
            shopItems[index].levelOfItem++;
        }

        GameSingleton.Instance.SoundManager.CreateSound()
                                   .WithSoundData(SoundEffect.PURCHASE_UPGRADE)
                                   .Play();

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
                tmp += shopItems[i].bonusCounter * shopItems[i].bonusPerSec;
            }

            PassiveBonusPerSec = tmp;
            Score += tmp;
            yield return new WaitForSeconds(1);
        }
    }

    private void OnApplicationQuit()
    {
        if (Clicks > 0)
        {
            _spawnDown.ClosePan();
        }
    }


    public void OnClick()
    {
        GameSingleton.Instance.SoundManager.CreateSound()
                                           .WithSoundData(SoundEffect.CLICK_COMPUTER)
                                           .WithRandomPitch()
                                           .Play();

        TotalClick++;
        _plot.Total++; //счётчик для событий в Plot
        ColClicks++;
        if (_boost.IsBoostActive)
        {
            Vector3 targetScreenPosition;

            if (IsMobile)
                targetScreenPosition = new Vector3(UnityEngine.Random.Range(Screen.width / 6, Screen.width - Screen.width / 6), Screen.height , 0);
            else
                 targetScreenPosition = new Vector3(UnityEngine.Random.Range(Screen.width / 6, Screen.width / 2), Screen.height , 0);

            Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(targetScreenPosition);
            targetWorldPosition.z = 0;

            Instantiate(moneyPref, targetWorldPosition, Quaternion.identity, moneyPopupCanvas.transform);
            Score += ScoreIncrease * 3; // когда работает буст, умножаем доход на 3
        }
        else
            Score += ScoreIncrease;
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
}