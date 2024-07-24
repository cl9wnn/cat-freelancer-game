using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using YG;


public class Achievements : MonoBehaviour
{
    private int achievementsCount;
    
    public int AchievementsCount
    {
        get { return achievementsCount; }
        set
        {
            achievementsCount = value;
            achievementsCountText.text = achievementsCount.ToString();
        }
    }
    public Text achievementsCountText;//сделать счЄтчик достижений

    public Sprite[] achievementsPrefab;
    public Image[] achievementsSprites;
    public bool[] isAchievementDone;
    public Text[] achievementsText;
    public Text[] resultTexts;
    public Text MyAchievememntsText;
    public int index = 0; //дл€ смены страницы
    public AudioSource CloseBook;
    public AudioSource OpenBook;
    public AudioSource GetAchievement;
    public ParticleSystem achieveGlow;

    public event Action OnAchievementComplete;

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
        YandexGame.savesData.achievementsData = new AchievementsData(isAchievementDone, achievementsCount);
    }
    private void Load()
    {
        var data = YandexGame.savesData.achievementsData;

        if (data == null) return;

        isAchievementDone = data.isAchievementDone;

        achievementsCount = data.achievementsCount;
    }

    private void Start()
    {
        for (int i = 0; i < isAchievementDone.Length; i++)
        {
            if (isAchievementDone[i] == true)
            {
                achievementsSprites[i].sprite = achievementsPrefab[i];
            }
        }
    }

    public void CompleteAchievement(int index)
    {
        if (isAchievementDone[index] == false)
        {
            AchievementsCount++;
            //achieveGlow.gameObject.SetActive(true);
            GetAchievement.Play();
            achievementsSprites[index].sprite = achievementsPrefab[index];
            isAchievementDone[index] = true;

            OnAchievementComplete?.Invoke();
        }
    }
    // купите последнюю видеокарту
    //станьте (последн€€ работа)
    // приобретите самый мощный комп

    public void ChangeLanguage()
    {
        MyAchievememntsText.text = LanguageSystem.lng.achievements[8];
        for (int i = 0; i < achievementsText.Length; i++) achievementsText[i].text = LanguageSystem.lng.achievements[i];
        if (!isAchievementDone[2]) resultTexts[2].text = LanguageSystem.lng.achievements[9]; //не выполнено 3 ачивка
        else resultTexts[2].text = "";
        if (!isAchievementDone[4]) resultTexts[4].text = LanguageSystem.lng.achievements[9]; //не выполнено 3 ачивка
        else resultTexts[4].text = "";
        if (!isAchievementDone[6]) resultTexts[6].text = LanguageSystem.lng.achievements[9]; //не выполнено 3 ачивка
        else resultTexts[6].text = "";

        achievementsCountText.text = AchievementsCount.ToString();

    }
}