using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using YG;
using UnityEngine.Analytics;


public class Settings : MonoBehaviour {
    public GameObject settingsPan;
    public Text resumeText;
    public Text authorsText;
    public Text volumeText;
    public Text settingsText;
    public Text languageText;
    public Text musicText;
    public Text aboutGameTitleText;
    public Text aboutGameInfoText;
    public AudioSource audioSourceMusic;
    public Sprite onSprite;
    public Sprite offSprite;

    public Canvas StartCanvas;

    public bool IsMuted
    {
        get => audioSourceMusic.mute;
        set
        {
            audioSourceMusic.mute = value;
            buttonImage.sprite = IsMuted ? offSprite : onSprite;
        }
    }
    public Image buttonImage;
    public GameObject aboutGamePan;
    public Animator settinsAnimator;
    public Animator TurnAroundAnimator;
    public Plot plot;
    public AudioSource AudioOpenSettings;


    public void Awake()
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
        YandexGame.savesData.settingsData = new SettingsData(IsMuted);
    }
    private void Load()
    {
        var data = YandexGame.savesData.settingsData;

        StartCanvas.gameObject.SetActive(!plot.isStart);
        
        if (data == null) return;

        IsMuted = data.isMuted;
        
        
        if (plot.isStart) audioSourceMusic.Play();

    }

    public void ChangeLanguage()
    {
        resumeText.text = LanguageSystem.lng.settings[0];
       // authorsText.text = LanguageSystem.lng.settings[1]; // В ЯИ были вырезаны
        volumeText.text = LanguageSystem.lng.settings[3];
        settingsText.text = LanguageSystem.lng.settings[4];
        languageText.text = LanguageSystem.lng.settings[5];
        musicText.text = LanguageSystem.lng.settings[6];
        aboutGameTitleText.text = LanguageSystem.lng.settings[7];
    }
     
     public void ShowSettingsPan()
     {
        AudioOpenSettings.Play();
        settinsAnimator.SetTrigger("open");
        TurnAroundAnimator.SetTrigger("Turn");
    }
    public void Resume()
    {
        AudioOpenSettings.Play();
        settinsAnimator.SetTrigger("close");
        TurnAroundAnimator.SetTrigger("TurnClose");
    }
    public void ShowAboutGamePan()
    {
        aboutGamePan.SetActive(true);
    }
    public void ExitAboutGamePan()
    {
        aboutGamePan.SetActive(false);

    }
    public void OpenAboutGame()
     {
        aboutGamePan.SetActive(true);

     }
    public void SwitchMusic()
    {
        IsMuted = !IsMuted;
    }
    public void Vka()
    {
     Application.OpenURL ("https://vk.com/cl9wn");
    }
    public void Telegram()
    {
        Application.OpenURL("https://t.me/cl_wn");
    }
    public void GooglePlay()
    {
        //Application.OpenURL("ссылка на игру");
    }
}