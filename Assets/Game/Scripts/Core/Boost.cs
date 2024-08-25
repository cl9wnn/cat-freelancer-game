using System;
using UnityEngine;
using UnityEngine.UI;
using YG;
using DG.Tweening;
using UnityEngine.Events;
using System.Globalization;
using System.Collections;

public class Boost : MonoBehaviour, ISaveLoad
{
    private const float DEFAULT_BOOST_DURATION = 20f;
    private const float DEFAULT_COOLDOWN_DURATION = 7200f;

    private const int MIN_AVAILABLE_COFFEE_FOR_DISPLAY = 2;
    private const float COOLDOWN_REWARD_THRESHOLD = 0.5f;

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

    [Header("Animations")]
    [SerializeField] private BounceAnimation coffeeAnimation;
    [SerializeField] private BoostAnimation boostAnimation;

    [Header("AD")]
    [SerializeField] private Image adImage;

    [Header("Sprites")]
    [SerializeField] private Sprite emptyBoostButtonSprite;
    [SerializeField] private Sprite fullBoostButtonSprite;

    private Achievements _achievements;

    private bool hasCooldownTimer;
    private float remainingCooldown;

    private UnityAction adAction;
    private UnityAction boostAction;

    public bool IsBoostActive { get; private set; }
    public int TotalCoffeeConsumed { get; private set; }
    public int AvailableCoffee { get; private set; }


    private void Awake()
    {
        _achievements = GameSingleton.Instance.Achievements;
    }

    public void Save()
    {
        ref var data = ref YandexGame.savesData.boostData;

        if (data == null)
        {
            data = new BoostData(remainingCooldown, TotalCoffeeConsumed, AvailableCoffee);
            return;
        }

        data.cooldownDuration = remainingCooldown;
        data.saveDate = YandexGame.ServerTime();
        data.totalCoffeeConsumed = TotalCoffeeConsumed;
        data.availableCoffee = AvailableCoffee;
    }
    public void Load()
    {
        var data = YandexGame.savesData.boostData;

        if (data == null)
            return;

        remainingCooldown = data.cooldownDuration;
        TotalCoffeeConsumed = data.totalCoffeeConsumed;
        AvailableCoffee = data.availableCoffee;

        var elapsed = YandexGame.ServerTime() - data.saveDate;
        remainingCooldown -= elapsed / 1000;
    }

    private void Start()
    {
        UpdateCoffeeUI();
        UpdateYesButtonState();

        CheckAchievements();
        CheckCooldown();
    }

    private void CheckCooldown()
    {
        hasCooldownTimer = AvailableCoffee <= 0 && !IsBoostActive;

        if (hasCooldownTimer)
            StartCoroutine(CooldownCoroutine());
        else
            UpdateBoostText(LanguageSystem.lng.time[4]);
    }

    private void UpdateCoffeeUI()
    {
        availableCoffeeCountImage.gameObject.SetActive(AvailableCoffee >= MIN_AVAILABLE_COFFEE_FOR_DISPLAY && !IsBoostActive);
        availbableCoffeeCountText.text = AvailableCoffee.ToString();

        coffeeAnimation.GetComponent<Image>().sprite = (IsBoostActive || AvailableCoffee <= 0) ? emptyBoostButtonSprite : fullBoostButtonSprite;

        boostButton.interactable = !IsBoostActive;

        if (AvailableCoffee >= 1 && !IsBoostActive) 
            coffeeAnimation.StartAnimation();
        else 
            coffeeAnimation.StopAnimation();
    }
    private void UpdateYesButtonState()
    {
        if (adAction != null)
        {
            enableButton.onClick.RemoveListener(adAction);
        }

        if (boostAction != null)
        {
            enableButton.onClick.RemoveListener(boostAction);
        }

        if (AvailableCoffee <= 0)
        {
            SetAdMode();
        }
        else
        {
            SetActivateBoostMode();
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
    private void SetActivateBoostMode()
    {
        infoText.text = LanguageSystem.lng.boostt[0];
        boostAction = ActivateBoost;
        enableButton.onClick.AddListener(boostAction);

        enableButton.GetComponent<Image>().color = Color.white;
        adImage.gameObject.SetActive(false);
    }

    private void ActivateBoost()
    {
        if (AvailableCoffee <= 0 || IsBoostActive) return;

        IsBoostActive = true;
        OnBoostActivated();

        ChangeCoffee(amount: -1);
        TotalCoffeeConsumed++;

        CheckAchievements();

        StartCoroutine(BoostCoroutine());
    }

    private IEnumerator BoostCoroutine()
    {
        var boostDuration = DEFAULT_BOOST_DURATION;

        while (IsBoostActive && (boostDuration -= Time.fixedDeltaTime) > 0)
        {
            UpdateBoostText(boostDuration.ToString("0.0") + LanguageSystem.lng.time[3]);
            yield return new WaitForFixedUpdate();
        }

        EndBoost();
    }
    private IEnumerator CooldownCoroutine()
    {
        while (hasCooldownTimer && (remainingCooldown -= Time.fixedDeltaTime) > 0)
        {
            UpdateBoostText(GetCooldownTime());
            yield return new WaitForFixedUpdate();  
        }

        EndCooldown();
    }

    private void EndBoost()
    {
        IsBoostActive = false;
        OnBoostDeactivated();

        UpdateCoffeeUI();
        CheckCooldown();
    }
    private void EndCooldown()
    {
        if (remainingCooldown >= COOLDOWN_REWARD_THRESHOLD)
        {
            hasCooldownTimer = false;
            remainingCooldown = DEFAULT_COOLDOWN_DURATION;
            return;
        }

        hasCooldownTimer = false;
        remainingCooldown = DEFAULT_COOLDOWN_DURATION;

        ChangeCoffee(amount: 1);
    }

    private string GetCooldownTime()
    {
        if (remainingCooldown <= 3600)
        {
            return (((int)remainingCooldown / 60) % 60).ToString("0") + LanguageSystem.lng.time[2];
        }
        if (remainingCooldown <= 60)
        {
            return (((int)remainingCooldown)).ToString("0") + LanguageSystem.lng.time[7];
        }
        return (((int)remainingCooldown / 3600).ToString("0") + LanguageSystem.lng.time[5] + (((int)remainingCooldown / 60) % 60).ToString("0") + LanguageSystem.lng.time[2]);
    }
    private void UpdateBoostText(string text)
    {
        boostText.text = text;
        boostCountdownText.text = text;
    }

    private void CheckAchievements()
    {
        if (TotalCoffeeConsumed < 5)
        {
            _achievements.resultTexts[7].text = TotalCoffeeConsumed.ToString() + "/5";
        }
        else if (TotalCoffeeConsumed == 5 && !_achievements.isAchievementDone[7])
        {
            _achievements.CompleteAchievement(7);
        }
        if (_achievements.isAchievementDone[7])
        {
            _achievements.resultTexts[7].text = "";
        }
    }

    private void OnBoostActivated()
    {
        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.DRINK_COFFEE).Play();
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.BOOST_MODE);

        boostAnimation.StartAnimation();
    }
    private void OnBoostDeactivated()
    {
        GameSingleton.Instance.MusicManager.PlayBackgroundMusic(BackgroundMusic.MAIN_GAME);

        boostAnimation.StopAnimation();
    }

    public void ChangeLanguage()
    {
        infoText.text = AvailableCoffee > 0 ? LanguageSystem.lng.boostt[0] : LanguageSystem.lng.boostt[4]; ;
        enableText.text = LanguageSystem.lng.boostt[1];
        watchAdText.text = LanguageSystem.lng.boostt[2];
        warningText.text = LanguageSystem.lng.boostt[5];
        
        UpdateBoostText(LanguageSystem.lng.time[4]);
    }
    public void ChangeCoffee(int amount)
    {
        if (AvailableCoffee + amount < 0) 
            return;

        AvailableCoffee += amount;
        
        CheckCooldown();

        UpdateCoffeeUI();
        UpdateYesButtonState();
    }
}
