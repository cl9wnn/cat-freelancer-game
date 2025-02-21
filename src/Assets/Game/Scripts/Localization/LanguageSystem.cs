using UnityEngine;
using UnityEngine.UI;

public class LanguageSystem : MonoBehaviour
{
    public Image langBttnImg;
    public Sprite[] flags;
 
    public static LanguageData lng = new LanguageData();
   
    private int langIndex = 1;
    public VolumeSettings volumeSettings;

    private Game _game;
    private Plot _plot;
    private Boost _boost;
    private Timer _timer;
    private Stats _stats;
    private SkinPC _skins;
    private Fortune _fortune;
    private Settings _settings;
    private SpawnDown _spawnDown;
    private Achievements _achievements;

    private string json;
    private string[] langArray = { "ru_RU", "en_US" };

    public string LanguageCode => PlayerPrefs.GetString("Language");

    private void Awake()
    {
        _game = GameSingleton.Instance.Game;
        _plot = GameSingleton.Instance.Plot;
        _boost = GameSingleton.Instance.Boost;
        _timer = GameSingleton.Instance.Timer;
        _stats = GameSingleton.Instance.Stats;
        _skins = GameSingleton.Instance.Skins;
        _fortune = GameSingleton.Instance.Fortune;
        _settings = GameSingleton.Instance.Settings;
        _spawnDown = GameSingleton.Instance.SpawnDown;
        _achievements = GameSingleton.Instance.Achievements;

        Initialize();
    }

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
                volumeSettings.UpdateLanguageSprites(langIndex - 1);
                _achievements.UpdateLanguageSprites(langIndex - 1);
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

        _game.ChangeLanguage();
        _settings.ChangeLanguage();
        _skins.ChangeLanguage();
        _timer.ChangeLanguage();
        _boost.ChangeLanguage();
        _plot.ChangeLanguage();
        _spawnDown.ChangeLanguage();
        _achievements.ChangeLanguage();
        _fortune.ChangeLanguage();
        _stats.ChangeLanguage();
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
        volumeSettings.UpdateLanguageSprites(langIndex - 1);
        _achievements.UpdateLanguageSprites(langIndex - 1);

        StopAllCoroutines();
        LoadLanguage();

    }
    public void SetLanguage(string languageCode)
    {
        langIndex = System.Array.IndexOf(langArray, languageCode);

        if (langIndex == -1)
        {
            Debug.LogError($"Language code '{languageCode}' not found in langArray.");
            return;
        }

        PlayerPrefs.SetString("Language", languageCode);

        langBttnImg.sprite = flags[langIndex];

        volumeSettings.UpdateLanguageSprites(langIndex);
        _achievements.UpdateLanguageSprites(langIndex);

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