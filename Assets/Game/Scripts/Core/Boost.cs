using System;
using UnityEngine;
using UnityEngine.UI;
using YG;
using DG.Tweening;
using UnityEngine.Events;
using System.Globalization;

public class Boost : MonoBehaviour, ISaveLoad
{
    [Header("Timers")]
    [SerializeField] private float boostDuration = 20f;
    [SerializeField] private float cooldownDuration = 7200f;

    [Header("UI Elements")]
    [SerializeField] private Button boostButton;
    [SerializeField] private Button enableButton;
    [SerializeField] private Text boostText;
    [SerializeField] private Text boostCountdownText;
    [SerializeField] private Text infoText;
    [SerializeField] private Text enableText;
    [SerializeField] private Text watchAdText;
    [SerializeField] private Text warningText;
    [SerializeField] private Text availbableCoffeeCountText;
    [SerializeField] private Image availableCoffeeCountImage;
    [SerializeField] private BounceAnimation coffeeAnimation;

    [Header("AD")]
    [SerializeField] private Image adImage;

    [Header("Sprites")]
    [SerializeField] private Sprite emptyBoostButtonSprite;
    [SerializeField] private Sprite fullBoostButtonSprite;

    private Achievements _achievements;

    private bool isBoostActive;
    private bool canWatchAd;
    private int totalCoffeeConsumed;
    private int availableCoffee;

    public event Action BoostActivated;
    public event Action BoostDeactivated;

    private UnityAction adAction;
    private UnityAction boostAction;

    public bool CanWatchAd
    {
        get => canWatchAd;
        set
        {
            if (canWatchAd != value)
            {
                canWatchAd = value;
                UpdateButtonState();
            }
        }
    }

    public bool IsBoostActive
    {
        get => isBoostActive;
        set
        {
            if (isBoostActive == value) return;
            isBoostActive = value;

            if (isBoostActive)
            {
                OnBoostActivated();
            }
            else
            {
                OnBoostDeactivated();
            }
        }
    }

    public int TotalCoffeeConsumed
    {
        get => totalCoffeeConsumed;
        set
        {
            totalCoffeeConsumed = value;
            CheckAchievements();
        }
    }

    public int AvailableCoffee
    {
        get => availableCoffee;
        set
        {
            availableCoffee = value;
            UpdateAvailableCoffeeUI();
        }
    }

    private void Awake()
    {
        _achievements = GameSingleton.Instance.Achievements;

        if (YandexGame.SDKEnabled)
        {
            Load();
        }
    }

    public void Save()
    {
        ref var data = ref YandexGame.savesData.boostData;

        if (data == null)
        {
            data = new BoostData(cooldownDuration, CanWatchAd, TotalCoffeeConsumed, AvailableCoffee);
            return;
        }

        data.cooldownDuration = cooldownDuration;   
        data.canWatchAd = CanWatchAd;   
        data.totalCoffeeConsumed = TotalCoffeeConsumed;
        data.availableCoffee = availableCoffee;
    }
    public void Load()
    {
        var data = YandexGame.savesData.boostData;

        if (data == null)
        {
            AddCoffee(1);
            return;
        }
        cooldownDuration = data.cooldownDuration;
        boostDuration = 0;
        TotalCoffeeConsumed = data.totalCoffeeConsumed;
        AvailableCoffee = data.availableCoffee;
        CanWatchAd = data.canWatchAd;

        TimeSpan elapsed = DateTime.UtcNow - data.saveDate;
        if (boostDuration <= 0)
        {
            cooldownDuration -= (int)elapsed.TotalSeconds;
        }
    }

    private void Start()
    {
        coffeeAnimation.StartAnimation();

        UpdateButtonState();
        UpdateAvailableCoffeeUI();
    }
    [ContextMenu("Add")]
    public void Add()
    {
        AddCoffee(1);
    }
    private void UpdateButtonState()
    {
        if (adAction != null)
        {
            enableButton.onClick.RemoveListener(adAction);
        }

        if (boostAction != null)
        {
            enableButton.onClick.RemoveListener(boostAction);
        }

        if (canWatchAd)
        {
            Debug.Log("AD");
            SetAdMode();
        }
        else
        {
            Debug.Log("AD 2");
            SetBoostMode();
        }
    }

    private void SetAdMode()
    {
        infoText.text = LanguageSystem.lng.boostt[4];
        adAction = () => YandexGame.RewVideoShow(1);
        enableButton.onClick.AddListener(adAction);

        enableButton.GetComponent<Image>().color = new Color32(242, 200, 25, 255);
        adImage.gameObject.SetActive(true);
    }

    private void SetBoostMode()
    {
        infoText.text = LanguageSystem.lng.boostt[0];
        boostAction = ActivateBoost;
        enableButton.onClick.AddListener(boostAction);

        enableButton.GetComponent<Image>().color = Color.white;
        adImage.gameObject.SetActive(false);
    }

    private void ActivateBoost()
    {
        if (AvailableCoffee > 0 && !CanWatchAd)
        {
            IsBoostActive = true;
            AvailableCoffee--;
        }
    }

    private void FixedUpdate()
    {
        UpdateBoostStatus();
        UpdateCooldownStatus();
    }

    private void UpdateBoostStatus()
    {
        if (IsBoostActive)
        {
            HandleActiveBoost();
        }
    }

    private void HandleActiveBoost()
    {
        coffeeAnimation.GetComponent<Image>().sprite = emptyBoostButtonSprite;
        coffeeAnimation.StopAnimation();

        if (boostDuration > 0)
        {
            boostDuration -= Time.fixedDeltaTime;
            boostButton.interactable = false;
            UpdateBoostText(boostDuration.ToString("0.0") + LanguageSystem.lng.time[3]);
        }
        else
        {
            IsBoostActive = false;

            if (AvailableCoffee > 0)
            {
                cooldownDuration = 0;
                HandleCooldownEnd();
            }

            UpdateAvailableCoffeeUI();
        }
    }

    private void UpdateCooldownStatus()
    {
        if (cooldownDuration > 0 && boostDuration <= 0)
        {
            infoText.text = LanguageSystem.lng.boostt[4];
            CanWatchAd = true;
            cooldownDuration -= Time.fixedDeltaTime;
            coffeeAnimation.GetComponent<Image>().sprite = emptyBoostButtonSprite;
            boostButton.interactable = true;
            UpdateBoostText(GetFormattedCooldownTime());
        }

        if (cooldownDuration <= 0)
        {
            HandleCooldownEnd();
            AddCoffee(1);
        }
    }

    private void HandleCooldownEnd()
    {
        coffeeAnimation.GetComponent<Image>().sprite = fullBoostButtonSprite;
        coffeeAnimation.StartAnimation();
        boostButton.interactable = true;
        UpdateBoostText(LanguageSystem.lng.time[4]);
        CanWatchAd = false;
        cooldownDuration = 7200;
        boostDuration = 20;
    }

    private string GetFormattedCooldownTime()
    {
        if (cooldownDuration <= 3600)
        {
            return (((int)cooldownDuration / 60) % 60).ToString("0") + LanguageSystem.lng.time[2];
        }
        if (cooldownDuration <= 60)
        {
            return (((int)cooldownDuration)).ToString("0") + LanguageSystem.lng.time[7];
        }
        return (((int)cooldownDuration / 3600).ToString("0") + LanguageSystem.lng.time[5] + (((int)cooldownDuration / 60) % 60).ToString("0") + LanguageSystem.lng.time[2]);
    }

    private void UpdateBoostText(string text)
    {
        boostText.text = text;
        boostCountdownText.text = text;
    }

    private void CheckAchievements()
    {
        if (TotalCoffeeConsumed < 3)
        {
            _achievements.resultTexts[7].text = TotalCoffeeConsumed.ToString() + "/3";
        }
        else if (TotalCoffeeConsumed == 3 && !_achievements.isAchievementDone[7])
        {
            _achievements.CompleteAchievement(7);
        }
        if (_achievements.isAchievementDone[7])
        {
            _achievements.resultTexts[7].text = "";
        }
    }

    private void UpdateAvailableCoffeeUI()
    {
        availableCoffeeCountImage.gameObject.SetActive(AvailableCoffee > 1 && !IsBoostActive);
        availbableCoffeeCountText.text = AvailableCoffee.ToString();
    }

    private void OnBoostActivated()
    {
        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.DRINK_COFFEE).Play();
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.BOOST_MODE);
        TotalCoffeeConsumed++;

        BoostActivated?.Invoke();
    }

    private void OnBoostDeactivated()
    {
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.MAIN_GAME);

        BoostDeactivated?.Invoke();
    }

    public void ChangeLanguage()
    {
        infoText.text = LanguageSystem.lng.boostt[0];
        enableText.text = LanguageSystem.lng.boostt[1];
        watchAdText.text = LanguageSystem.lng.boostt[2];
        warningText.text = LanguageSystem.lng.boostt[5];
        
        UpdateBoostText(LanguageSystem.lng.time[4]);
    }

    public void AddCoffee(int amount)
    {
        AvailableCoffee += amount;
        if (AvailableCoffee == amount)
        {
            if (cooldownDuration > 0)
            {
                cooldownDuration = 0;
                HandleCooldownEnd();
            }

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        UpdateAvailableCoffeeUI();
        UpdateButtonState();
    }
}
