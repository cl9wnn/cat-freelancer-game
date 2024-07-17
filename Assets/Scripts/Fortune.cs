using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using System;
using YG;


public class Fortune : MonoBehaviour
{
    public float speed;
    public bool can = false;
    public bool coffeeRewarded = false; // буст x3
    public bool canAd = false;
    public GameObject circle;
    public Game gmscript;
    public Button startBttn;
    public GameObject ADtBttn;
    public Text winText;
    public Text BoostTextt;
    public Text waitText;
    public float longTimer = 0;
    public float timer = 20;
    public Sprite bttnReady;
    public Sprite bttnLock;
    public Image bttnStart;
    public Boost bst;
    private float[] _sectorsAngles;
    private float _finalAngle;
    private float _startAngle = 0;
    private float _currentLerpRotationTime;
    public Image newImgFortune;
    public Achievements achieve;

    public AudioSource winSound1;
    public AudioSource winSound2;
    public AudioSource loseSound;


    private void Awake()
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
        YandexGame.savesData.fortuneData = new FortuneData(longTimer, canAd, coffeeRewarded, timer);
    }
    private void Load()
    {
        var data = YandexGame.savesData.fortuneData;

        if (data == null) return;

        longTimer = data.longTimer;
        canAd = data.canAd;
        coffeeRewarded = data.doo;
        timer = data.timer;
        TimeSpan ts = DateTime.UtcNow - data.date;
        longTimer -= (int)ts.TotalSeconds;
    }

    public void ChangeLanguage()
    {
        winText.text = LanguageSystem.lng.fortune[0];
    }

    void Start()
    {
    }
    public void OnClick()
    {
        _currentLerpRotationTime = 0f;

        _sectorsAngles = new float[] {45, 90, 135, 180, 225, 270, 315, 360};

        int fullCircles = 10;
        float randomFinalAngle = _sectorsAngles[UnityEngine.Random.Range(0, _sectorsAngles.Length)];

        // Here we set up how many circles our wheel should rotate before stop
        _finalAngle = -(fullCircles * 360 + randomFinalAngle);
        can = true;
    }

    void FixedUpdate()
    {
        if (can == true)
        {
            winText.text = LanguageSystem.lng.fortune[1];
            startBttn.interactable = false;
            longTimer = 7200;
            float maxLerpRotationTime = 6f;
            _currentLerpRotationTime += Time.deltaTime;
            if (_currentLerpRotationTime > maxLerpRotationTime || circle.transform.eulerAngles.z == _finalAngle)
            {
                _currentLerpRotationTime = maxLerpRotationTime;
                can = false;
                _startAngle = _finalAngle % 360;
                GiveAwardByAngle();
            }
            float t = _currentLerpRotationTime / maxLerpRotationTime;

            t = t * t * t * (t * (6f * t - 15f) + 10f);

            float angle = Mathf.Lerp(_startAngle, _finalAngle, t);
            circle.transform.eulerAngles = new Vector3(0, 0, angle);
            
            bttnStart.sprite = bttnLock;
        }
        if (longTimer > 0)
        {
            newImgFortune.enabled = false;
            bttnStart.sprite = bttnLock;
            startBttn.interactable = false;
            longTimer -= Time.fixedDeltaTime;
            waitText.text = (((int)longTimer / 3600).ToString("0") + LanguageSystem.lng.time[5] + (((int)longTimer / 60) % 60).ToString("0") + LanguageSystem.lng.time[2]);
            if (longTimer <= 3600)
            {
                waitText.text = (((int)longTimer / 60) % 60).ToString("0") + LanguageSystem.lng.time[2];
            }
            if (longTimer <= 60)
            {
                waitText.text = (((int)longTimer)).ToString("0") + LanguageSystem.lng.time[7];
            }
        }
        if (longTimer <= 0)
        {
            newImgFortune.enabled = true;
            bttnStart.sprite = bttnReady;
            startBttn.interactable = true;
            waitText.text = LanguageSystem.lng.time[4];
            canAd = false;
        }
        if (canAd == true)
        {
            ADtBttn.SetActive(true);
        }
        else ADtBttn.SetActive(false);

        if (coffeeRewarded == true)
        {
            bst.BttnTmer.interactable = false;
            bst.BoostText.text = LanguageSystem.lng.time[8]; ;
            if (timer > 0)
            {
                timer -= Time.fixedDeltaTime;
                BoostTextt.gameObject.SetActive(true);
                BoostTextt.text = timer.ToString("0.0") + LanguageSystem.lng.time[3];
            }
            if (timer <= 0)
            {
                coffeeRewarded = false;
                BoostTextt.gameObject.SetActive(false);
                bst.BttnTmer.interactable = true;
                bst.BoostText.text = LanguageSystem.lng.time[4];
                timer = 20;
            }
        }
    }
    private void GiveAwardByAngle()
    {
        float moneyMultiplier = gmscript.PassiveBonusPerSec == 0 ? 1 : gmscript.PassiveBonusPerSec;
        switch ((int)_startAngle)
        {
            case 0:
                winText.text = StringMethods.FormatMoney(moneyMultiplier * 3600) + LanguageSystem.lng.fortune[2];
                gmscript.Score += moneyMultiplier * 3600;
                if (!achieve.isAchievementDone[2])
                {
                    achieve.resultTexts[2].text = "";
                    achieve.CompleteAchievement(2);
                }
                winSound1.Play();
                Debug.Log("score*2");
                break;
            case -225:
                longTimer = 0;
                winText.text = LanguageSystem.lng.fortune[6];
                winSound2.Play();
                Debug.Log("еще раз");
                break;
            case -270:
                winText.text =  StringMethods.FormatMoney(moneyMultiplier * 2700) + LanguageSystem.lng.fortune[2];
                gmscript.Score += moneyMultiplier * 2700;
                winSound1.Play();
                Debug.Log("score*1");
                break;
            case -45:
                winText.text = LanguageSystem.lng.fortune[4];
                loseSound.Play();
                Debug.Log("ничего");
                break;
            case -180:
                winText.text = StringMethods.FormatMoney(moneyMultiplier * 900) + LanguageSystem.lng.fortune[2];
                gmscript.Score += moneyMultiplier * 900;
                winSound1.Play();
                Debug.Log("1/4 score");
                break;
            case -135:
                gmscript.ScoreIncrease = gmscript.ScoreIncrease * 2;
                winText.text = LanguageSystem.lng.fortune[5];
                winSound2.Play();
                Debug.Log("*2 scoreIncrease");
                break;
            case -90:
                winText.text = StringMethods.FormatMoney(moneyMultiplier *1800) + LanguageSystem.lng.fortune[2];
                gmscript.Score += moneyMultiplier * 1800;
                winSound1.Play();
                Debug.Log("*1/2 score");
                break;
            case -315:
                coffeeRewarded = true;
                winText.text = LanguageSystem.lng.fortune[3];
                winSound2.Play();
                Debug.Log("кофе");
                break;
        }

        canAd = true;
    }
}
