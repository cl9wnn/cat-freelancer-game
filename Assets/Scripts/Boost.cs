
using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;
using YG;

public class Boost : MonoBehaviour
{

    public Game gmscript;
    public Settings settingss;
    public float timer = 20;
    public float longTimer = 7200;
    public bool boostOn = false;

    public bool BoostOn
    {
        get => boostOn;
        set
        {
            boostOn = value;
            if (boostOn)
            {
                settingss.audioSourceMusic.volume = 0f;
                CofeeDrink.Play();
                phonk.Play();
                CountOfCofee++;
            }
        }
    }
    public Button BttnTmer;
    public Button BttnEn;
    public GameObject ADBttn;
    public Text BoostText; //позиция текста и размер буста меняется в скрипте!
    public Text infoText;
    public Text startText;
    public Text WatchAdText;
    public Text warningText;
    public GameObject boostPan;
    public GameObject BstBttn;
    public Sprite BstBttnPustaya;
    public Sprite BstBttnPolnaya;
    public Fortune fort;
    public Achievements achieve;
    public Animator boostAnimator;
    public bool canBoostAd = false;
    public int countOfCofee;

    public bool soundPlayed;

    public AudioSource CofeeDrink;
    public AudioSource phonk;

    public int CountOfCofee
    {
        get => countOfCofee;
        set
        {
            countOfCofee = value;

            if (countOfCofee < 3) achieve.resultTexts[7].text = countOfCofee.ToString() + "/3";
            else if (countOfCofee == 3 && !achieve.isAchievementDone[7]) achieve.CompleteAchievement(7);
            if (achieve.isAchievementDone[7]) achieve.resultTexts[7].text = "";
        }
    }

    private void Awake()
    {
        soundPlayed = false;

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
        YandexGame.savesData.boostData = new BoostData(longTimer, timer, BoostOn, canBoostAd, countOfCofee);
    }
    private void Load()
    {
        var data = YandexGame.savesData.boostData;

        if (data == null) return;

        longTimer = data.longTimer;
        timer = data.timer;
        TimeSpan ts = DateTime.UtcNow - data.date;
        if (timer <= 0)
        {
            longTimer -= (int)ts.TotalSeconds;
        }
        CountOfCofee = data.countOfCoffee;
        BoostOn = data.boostOn;
        canBoostAd = data.canBoostAd;
    }

    void Start()
    {
        if (!BoostOn) BstBttn.GetComponent<Animator>().SetTrigger("jump");
    }
    public void OnClick()
    {
        if (!canBoostAd)
        {
            BoostOn = true;
            boostAnimator.SetTrigger("close");
        }
    }


    public void OpenPan()
    {
        boostAnimator.SetTrigger("open");
    }
    public void ClosePan()
    {
        boostAnimator.SetTrigger("close");
    }

    void FixedUpdate()
    {
        BoostText.text = LanguageSystem.lng.time[4];

        if (BoostOn == true)
        {
            BstBttn.GetComponent<Image>().sprite = BstBttnPustaya;
            BstBttn.GetComponent<Animator>().SetTrigger("wait");
            if (timer > 0)
            {
                timer -= Time.fixedDeltaTime;
                BttnTmer.interactable = false;
                BoostText.text = LanguageSystem.lng.time[8];
                fort.BoostTextt.gameObject.SetActive(true);
                fort.BoostTextt.text = timer.ToString("0.0") + LanguageSystem.lng.time[3];
            }
            if (timer <= 0)
            {
                BoostOn = false;
                settingss.audioSourceMusic.volume = 100;
                fort.BoostTextt.gameObject.SetActive(false);
            }
        }
        if (longTimer > 0 && timer <= 0)
        {
            infoText.text = LanguageSystem.lng.boostt[4];
            canBoostAd = true;
            longTimer -= Time.fixedDeltaTime;
            BstBttn.GetComponent<Image>().sprite = BstBttnPustaya;
            BttnTmer.interactable = true;
            BoostText.text = (((int)longTimer / 3600).ToString("0") + LanguageSystem.lng.time[5] + (((int)longTimer / 60) % 60).ToString("0") + LanguageSystem.lng.time[2]);
            if (longTimer <= 3600)
            {
                BoostText.text = (((int)longTimer / 60) % 60).ToString("0") + LanguageSystem.lng.time[2];
            }
            if (longTimer <= 60)
            {
                BoostText.text = (((int)longTimer)).ToString("0") + LanguageSystem.lng.time[7];
            }
        }
        if (longTimer <= 0)
        {
            infoText.text = LanguageSystem.lng.boostt[0];
            BstBttn.GetComponent<Image>().sprite = BstBttnPolnaya;
            BstBttn.GetComponent<Animator>().SetTrigger("jump");
            BttnTmer.interactable = true;
            BoostText.text = LanguageSystem.lng.time[4];
            canBoostAd = false;
            longTimer = 7200;
            timer = 20;

        }
    }
    public void ChangeLanguage()
    {
        infoText.text = LanguageSystem.lng.boostt[0];
        startText.text = LanguageSystem.lng.boostt[1];
        WatchAdText.text = LanguageSystem.lng.boostt[2];
        warningText.text = LanguageSystem.lng.boostt[5];
    }
}