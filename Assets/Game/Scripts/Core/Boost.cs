using System;
using UnityEngine;
using UnityEngine.UI;
using YG;
using DG.Tweening;
using UnityEngine.Events;

public class Boost : MonoBehaviour
{
    [Header("Timers")]
    [SerializeField] private float boostDuration = 20f;
    [SerializeField] private float cooldownDuration = 7200f;

    [Header("UI Elements")]
    [SerializeField] private Button boostButton;
    [SerializeField] private Button enableButton;
    [SerializeField] private Text boostText;
    [SerializeField] private Text infoText;
    [SerializeField] private Text enableText;
    [SerializeField] private Text watchAdText;
    [SerializeField] private Text warningText;
    [SerializeField] private BounceAnimation coffeeAnimation;

    [Header("AD")]
    [SerializeField] private Image adImage;

    [Header("Sprites")]
    [SerializeField] private Sprite emptyBoostButtonSprite;
    [SerializeField] private Sprite fullBoostButtonSprite;

    private Settings _settings;
    private Fortune _fortune;
    private Achievements _achievements;

    private bool isBoostActive;
    private bool canWatchAd;
    private int coffeeCount;

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

    public int CoffeeCount
    {
        get => coffeeCount;
        set
        {
            coffeeCount = value;
            UpdateCoffeeCountUI();
            CheckAchievements();
        }
    }

    public Button BoostButton => boostButton;
    public Text BoostText => boostText;

    private void Awake()
    {
        _settings = GameSingleton.Instance.Settings;
        _fortune = GameSingleton.Instance.Fortune;
        _achievements = GameSingleton.Instance.Achievements;

        if (YandexGame.SDKEnabled)
        {
            Load();
        }
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
        YandexGame.savesData.boostData = new BoostData(cooldownDuration, boostDuration, IsBoostActive, CanWatchAd, CoffeeCount);
    }

    private void Load()
    {
        var data = YandexGame.savesData.boostData;

        if (data == null) return;

        cooldownDuration = data.cooldownDuration;
        boostDuration = data.boostDuration;
        CoffeeCount = data.coffeeCount;
        IsBoostActive = data.isBoostActive;
        CanWatchAd = data.canWatchAd;

        TimeSpan elapsed = DateTime.UtcNow - data.saveDate;
        if (boostDuration <= 0)
        {
            cooldownDuration -= (int)elapsed.TotalSeconds;
        }
    }

    private void Start()
    {
        if (!IsBoostActive)
        {
            coffeeAnimation.StartAnimation();
        }
        UpdateButtonState();
        UpdateBoostStatus();
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
        adAction = () => YandexGame.RewVideoShow(1);
        enableButton.onClick.AddListener(adAction);

        enableButton.GetComponent<Image>().color = new Color32(242, 200, 25, 255);
        adImage.gameObject.SetActive(true);
    }

    private void SetBoostMode()
    {
        boostAction = ActivateBoost;
        enableButton.onClick.AddListener(boostAction);

        enableButton.GetComponent<Image>().color = Color.white;
        adImage.gameObject.SetActive(false);
    }

    private void ActivateBoost()
    {
        if (!CanWatchAd)
        {
            IsBoostActive = true;
            
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
            _fortune.BoostText.gameObject.SetActive(false);
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
        _fortune.BoostText.gameObject.SetActive(IsBoostActive);
        _fortune.BoostText.text = text;
    }

    private void UpdateCoffeeCountUI()
    {
        if (CoffeeCount < 3)
        {
            _achievements.resultTexts[7].text = CoffeeCount.ToString() + "/3";
        }
        else if (CoffeeCount == 3 && !_achievements.isAchievementDone[7])
        {
            _achievements.CompleteAchievement(7);
        }
        if (_achievements.isAchievementDone[7])
        {
            _achievements.resultTexts[7].text = "";
        }
    }

    private void CheckAchievements()
    {
        if (CoffeeCount == 3 && !_achievements.isAchievementDone[7])
        {
            _achievements.CompleteAchievement(7);
        }
    }

    private void OnBoostActivated()
    {
        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.DRINK_COFFEE).Play();
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.BOOST_MODE);
        CoffeeCount++;

        BoostActivated?.Invoke();
    }

    private void OnBoostDeactivated()
    {
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.MAIN_GAME);

        BoostDeactivated?.Invoke();
    }

    public void UnlockBooster()
    {
        if (cooldownDuration > 0)
        {
            cooldownDuration = 0; 
            UpdateCooldownStatus(); 
        }
    }

    public void ChangeLanguage()
    {
        infoText.text = LanguageSystem.lng.boostt[0];
        enableText.text = LanguageSystem.lng.boostt[1];
        watchAdText.text = LanguageSystem.lng.boostt[2];
        warningText.text = LanguageSystem.lng.boostt[5];
        
        UpdateBoostText(LanguageSystem.lng.time[4]);
    }
}
