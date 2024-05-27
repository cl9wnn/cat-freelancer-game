using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;


public class Achievements : MonoBehaviour
{
    public GameObject achievementAlbum;
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
    public Animator albumAnimator;
    public Animator rockingAlbum;
    public Game gmscript;

    public Sprite[] achievementsPrefab;
    public Image[] achievementsSprites;
    public bool[] isAchievementDone;
    public Text[] achievementsText;
    public Text[] resultTexts;
    public Text MyAchievememntsText;
    public int index = 0; //дл€ смены страницы
    private string fileName;
    public AudioSource CloseBook;
    public AudioSource OpenBook;
    public AudioSource GetAchievement;
    public ParticleSystem achieveGlow;





    private void Awake()
    {
        fileName = "Achievements.BIN";
        if (File.Exists(Application.persistentDataPath + "/Saves/" + fileName))
        {
            SavedData data = MyLoad.LoadFileBinary<SavedData>(fileName);
            for (int i = 0; i < isAchievementDone.Length; i++)
            {
                isAchievementDone[i] = data.isAchievementDone[i];
                achievementsCount = data.achievementsCount;
            }
        }
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
    public void OpenAlbum()
    {
        achieveGlow.gameObject.SetActive(false);
        albumAnimator.SetTrigger("open");
        rockingAlbum.SetTrigger("Wait");
        OpenBook.Play();
    }

    public void CloseAlbum()
    {
        CloseBook.Play();
        index = 0; //чтобы при переключении изначально была лева€ страница
        albumAnimator.SetTrigger("close");
    }

    public void TurnTo()
    {
        if (index == 0)
        {
            albumAnimator.SetTrigger("right");
            index = 1;
        }
        else if (index == 1)
        {
            albumAnimator.SetTrigger("left");
            index = 0;
        }
    }

    public void CompleteAchievement(int index)
    {
        if (isAchievementDone[index] == false)
        {
            AchievementsCount++;
            achieveGlow.gameObject.SetActive(true);
            GetAchievement.Play();
            rockingAlbum.SetTrigger("rock");
            achievementsSprites[index].sprite = achievementsPrefab[index];
            isAchievementDone[index] = true;

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


#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause (bool pause) {
        if (pause)
        {
            SavedData data = new SavedData (isAchievementDone, achievementsCount);
           Save(data);
        } else {
            Awake ();
        }
    }
#else
    private void OnApplicationQuit()
    {
        SavedData data = new SavedData(isAchievementDone, achievementsCount);
        Save(data);
    }
#endif

    private void Save(object Obj)
    {
        MySave.SaveFileBinary(Obj, fileName);
    }


    [Serializable]
    public class SavedData
    {
        public SavedData(bool[] IsAchievementDone, int AchievementsCount)
        {
            isAchievementDone = IsAchievementDone;
            achievementsCount = AchievementsCount;
        }
        public bool[] isAchievementDone;
        public int achievementsCount;
    }
}
