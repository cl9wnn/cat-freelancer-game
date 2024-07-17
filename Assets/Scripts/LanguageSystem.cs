
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using YG;
using System.Text;


public class LanguageSystem : MonoBehaviour
{
    public Image langBttnImg;
    public Sprite[] flags;
    public static LanguageData lng = new LanguageData();
    private int langIndex = 1;
    public Game game;
    public Boost boost;
    public Plot plot;
    public Settings button;
    public Timer timer;
    public SkinCoin skins;
    public SpawnDown spawnDown;
    public Achievements achiev;
    public Fortune fortune;
    public Stats stats;
    private string json;
    private string[] langArray = { "ru_RU", "en_US" };

    public void Initialize ()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
            {
                PlayerPrefs.SetString("Language", "ru_RU");
            }
            else PlayerPrefs.SetString("Language", "en_US");
        }

        LoadLanguage();
    }

    void Start()
    {
        for (int i = 0; i < langArray.Length; i++)
        {
            if (PlayerPrefs.GetString("Language") == langArray[i])
            {
                langIndex = i + 1;
                langBttnImg.sprite = flags[i];
                break;
            }
        }
    }
    void LoadLanguage()
    {
        string path = PlayerPrefs.GetString("Language");
        
        json = Resources.Load<TextAsset>(path).text;
        
        Debug.Log("Язык: " + PlayerPrefs.GetString("Language") + " " + (json == null ? " Не загружен" : "Загружен"));

        lng = JsonUtility.FromJson<LanguageData>(json);

        game.ChangeLanguage();
        button.ChangeLanguage();
        skins.ChangeLanguage();
        timer.ChangeLanguage();
        boost.ChangeLanguage();
        plot.ChangeLanguage();
        spawnDown.ChangeLanguage();
        achiev.ChangeLanguage();
        fortune.ChangeLanguage();
        stats.ChangeLanguage();
    }
    public void SwitchBttn()
    {
        if (langIndex != langArray.Length)
        {
            langIndex++;
        }
        else langIndex = 1;
        PlayerPrefs.SetString("Language", langArray[langIndex - 1]);
        langBttnImg.sprite = flags[langIndex - 1];
        
        StopAllCoroutines();
        LoadLanguage();

    }

}

public class LanguageData
{
    public string[] bonusName;
    public string[] settings;
    public string[] BonusIncreaseName;
    public string[] NamesOfPanels;
    public string[] OfflineDohod;
    public string[] revenueper;
    public string[] time;
    public string[] info;
    public string[] ok;
    public string[] events;
    public string[] skinTextDop;
    public string[] boostt;
    public string[] infoGame;
    public string[] fortune;
    public string[] training;
    public string[] moneyDrop;
    public string[] achievements;
    public string[] statsString;
    public string[] statsText;
}