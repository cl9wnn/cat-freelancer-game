
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LanguageSystem : MonoBehaviour
{
    public Image langBttnImg;
    public Sprite[] flags;
    public static lang lng = new lang();
    private int langIndex = 1;
    public Game game;
    public Boost boost;
    public Plot plot;
    public Settingss button;
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
        LangLoad();
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
    void LangLoad()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "Languages/" + PlayerPrefs.GetString("Language") + ".json");
        UnityWebRequest www = UnityWebRequest.Get(path);
        www.SendWebRequest();
       while (!www.isDone) ;
      json = www.downloadHandler.text;
#else
        json = File.ReadAllText(Application.streamingAssetsPath + "/Languages/" + PlayerPrefs.GetString("Language") + ".json");
#endif
        lng = JsonUtility.FromJson<lang>(json);
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
        LangLoad();
    }

}

public class lang
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
    public string[] moneyDrop;
    public string[] achievements;
    public string[] statsString;
    public string[] statsText;
}




