using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Fortune : MonoBehaviour, ISaveLoad
{
    [Header("Settings")]
    [SerializeField] private float spinDuration = 6f;
    [SerializeField] private float cooldownDuration = 7200f;
    [SerializeField] private float rewardTimer = 20f;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject circle;
    [SerializeField] private Button spinButton;
    [SerializeField] private GameObject adButton;
    [SerializeField] private Text winText;
    [SerializeField] private Text waitText;
    [SerializeField] private Sprite buttonReadySprite;
    [SerializeField] private Sprite buttonLockedSprite;
    [SerializeField] private Image spinButtonImage;
    [SerializeField] private Image alertImage;

    private Game _game;
    private Achievements _achievements;
    
    private bool isSpinning;
    private bool isAdAvailable;
    private float remainingCooldownTime;
    private float remainingRewardTime;
    private float[] sectorAngles;
    private float finalAngle;
    private float startAngle;
    private float currentLerpTime;

    public event Action WheelStartedSpinning;
    public event Action WheelStoppedSpinning;

    private void Awake()
    {
        _game = GameSingleton.Instance.Game;
        _achievements = GameSingleton.Instance.Achievements;

        if (YandexGame.SDKEnabled)
        {
            Load();
        }
    }

    public void Save()
    {
        ref var data = ref YandexGame.savesData.fortuneData;

        if (data == null)
        {
            data = new FortuneData(remainingCooldownTime, isAdAvailable, remainingRewardTime);
            return;
        }
    
        data.remainingCooldownTime = remainingCooldownTime;
        data.isAdAvailable = isAdAvailable;
        data.remainingRewardTime = remainingRewardTime;
    }
    public void Load()
    {
        var data = YandexGame.savesData.fortuneData;

        if (data == null) return;

        remainingCooldownTime = data.remainingCooldownTime;
        isAdAvailable = data.isAdAvailable;
        remainingRewardTime = data.remainingRewardTime;
        TimeSpan elapsed = DateTime.UtcNow - data.saveDate;
        remainingCooldownTime -= (int)elapsed.TotalSeconds;
    }

    public void ChangeLanguage()
    {
        winText.text = LanguageSystem.lng.fortune[0];
    }

    public void OnClick()
    {
        GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.SPIN_WHEEL_OF_FORTUNE).Play();

        StartSpin();
    }

    private void FixedUpdate()
    {
        if (isSpinning)
        {
            HandleSpinning();
        }
        else
        {
            HandleCooldown();
        }
    }

    private void StartSpin()
    {
        sectorAngles = new float[] { 45, 90, 135, 180, 225, 270, 315, 360 };
        int fullCircles = 10;
        float randomFinalAngle = sectorAngles[UnityEngine.Random.Range(0, sectorAngles.Length)];
        finalAngle = -(fullCircles * 360 + randomFinalAngle);
        startAngle = circle.transform.eulerAngles.z;
        currentLerpTime = 0f;
        isSpinning = true;
        spinButton.interactable = false;
        remainingCooldownTime = cooldownDuration;
        winText.text = LanguageSystem.lng.fortune[1];
        spinButtonImage.sprite = buttonLockedSprite;

        WheelStartedSpinning?.Invoke();
    }

    private void HandleSpinning()
    {
        currentLerpTime += Time.fixedDeltaTime;
        if (currentLerpTime > spinDuration || Mathf.Approximately(circle.transform.eulerAngles.z, finalAngle))
        {
            StopSpin();
            return;
        }
        
        float t = Mathf.Clamp01(currentLerpTime / spinDuration);
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        float angle = Mathf.Lerp(startAngle, finalAngle, t);
        circle.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void StopSpin()
    {
        isSpinning = false;
        DetermineAward();
        spinButtonImage.sprite = buttonLockedSprite;

        WheelStoppedSpinning?.Invoke();
    }

    private void DetermineAward()
    {
        float moneyMultiplier = _game.PassiveBonusPerSec == 0 ? 1 : _game.PassiveBonusPerSec;
        switch (Math.Round(circle.transform.eulerAngles.z % 360, 0, MidpointRounding.AwayFromZero))
        {
            case 0:
                AwardMoney(3600, moneyMultiplier);
                break;
            case 135:
                remainingCooldownTime = 0;
                winText.text = LanguageSystem.lng.fortune[6];
               
                GameSingleton.Instance.SoundManager.CreateSound()
                                                   .WithSoundData(SoundEffect.WIN_SOUND_2)
                                                   .Play();
                
                break;
            case 90:
                AwardMoney(2700, moneyMultiplier);
                break;
            case 315:
                winText.text = LanguageSystem.lng.fortune[4];

                GameSingleton.Instance.SoundManager.CreateSound()
                                                   .WithSoundData(SoundEffect.LOSE)
                                                   .Play();
                break;
            case 180:
                AwardMoney(900, moneyMultiplier);
                break;
            case 225:
                _game.ScoreIncrease *= 2;
                winText.text = LanguageSystem.lng.fortune[5];

                GameSingleton.Instance.SoundManager.CreateSound()
                                                   .WithSoundData(SoundEffect.WIN_SOUND_2)
                                                   .Play();
                break;
            case 270:
                AwardMoney(1800, moneyMultiplier);
                break;
            case 45:
                winText.text = LanguageSystem.lng.fortune[3];

                GameSingleton.Instance.Boost.ChangeCoffee(amount: 1);

                GameSingleton.Instance.SoundManager.CreateSound()
                                                   .WithSoundData(SoundEffect.WIN_SOUND_2)
                                                   .Play();
                break;
        }

        isAdAvailable = true;
    }

    private void AwardMoney(float amount, float multiplier)
    {
        winText.text = StringMethods.FormatMoney(amount * multiplier) + LanguageSystem.lng.fortune[2];
        _game.Score += amount * multiplier;
        if (!_achievements.isAchievementDone[2])
        {
            _achievements.resultTexts[2].text = "";
            _achievements.CompleteAchievement(2);
        }

        GameSingleton.Instance.SoundManager.CreateSound()
                                           .WithSoundData(SoundEffect.WIN_SOUND_1)
                                           .Play();
    }

    private void HandleCooldown()
    {
        if (remainingCooldownTime > 0)
        {
            remainingCooldownTime -= Time.fixedDeltaTime;
            UpdateCooldownUI();
        }
        else
        {
            ResetCooldown();
        }
    }

    private void UpdateCooldownUI()
    {
        alertImage.enabled = false;
        spinButtonImage.sprite = buttonLockedSprite;
        spinButton.interactable = false;
        waitText.text = FormatTime(remainingCooldownTime);
        adButton.SetActive(isAdAvailable);
    }

    private void ResetCooldown()
    {
        alertImage.enabled = true;
        spinButtonImage.sprite = buttonReadySprite;
        spinButton.interactable = true;
        waitText.text = LanguageSystem.lng.time[4];
        isAdAvailable = false;
    }

    private string FormatTime(float time)
    {
        if (time <= 3600)
        {
            return ((int)time / 60 % 60).ToString("0") + LanguageSystem.lng.time[2];
        }
        if (time <= 60)
        {
            return ((int)time).ToString("0") + LanguageSystem.lng.time[7];
        }
        return ((int)time / 3600).ToString("0") + LanguageSystem.lng.time[5] +
               ((int)time / 60 % 60).ToString("0") + LanguageSystem.lng.time[2];
    }

    public void UnlockFortuneWheel()
    {
        if (isSpinning)
        {
            Debug.LogWarning("Cannot unlock the booster while spinning.");
            return;
        }

        remainingCooldownTime = 0f;
        remainingRewardTime = 0f;

        isAdAvailable = false;

        UpdateUIForUnlockedBooster();
    }

    private void UpdateUIForUnlockedBooster()
    {
        alertImage.enabled = true;
        spinButtonImage.sprite = buttonReadySprite;
        spinButton.interactable = true;
        waitText.text = LanguageSystem.lng.time[4];
        adButton.SetActive(isAdAvailable);

        if (circle != null)
        {
            circle.transform.eulerAngles = new Vector3(0, 0, startAngle);
        }
    }
}
