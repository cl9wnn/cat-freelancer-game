using System;
using UnityEngine;
using UnityEngine.UI;
using YG;


public class Achievements : MonoBehaviour, ISaveLoad
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
    public ParticleSystem achieveGlow;

    public event Action OnAchievementComplete;

    public void Save()
    {
        ref var data = ref YandexGame.savesData.achievementsData;

        if (data == null)
        {
            data = new AchievementsData(isAchievementDone, achievementsCount);
            return;
        }
        
        data.isAchievementDone = isAchievementDone;
        data.achievementsCount = achievementsCount; 
    }
    public void Load()
    {
        var data = YandexGame.savesData.achievementsData;

        if (data == null) return;

        isAchievementDone = data.isAchievementDone;

        AchievementsCount = data.achievementsCount;
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

        ChangeLanguage();
    }

    public void CompleteAchievement(int index)
    {
        if (isAchievementDone[index] == false)
        {
            AchievementsCount++;
            
            GameSingleton.Instance.SoundManager.CreateSound()
                                               .WithSoundData(SoundEffect.ACHIEVEMENT_NOTIFICATION)
                                               .Play();

            achievementsSprites[index].sprite = achievementsPrefab[index];
            isAchievementDone[index] = true;
            ChangeLanguage();
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