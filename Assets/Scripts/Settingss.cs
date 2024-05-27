using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;


public class Settingss : MonoBehaviour {
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
    public bool isMuted;
    public Image buttonImage;
    private string fileName;
    public GameObject aboutGamePan;
    public Animator settinsAnimator;
    public Animator TurnAroundAnimator;
    public Plot plot;
    public AudioSource AudioOpenSettings;
    public AudioSource OpenSceneMusic;


    private void Awake()
    {
        fileName = "BttnSaveLoad.BIN";
        if (File.Exists(Application.persistentDataPath + "/Saves/" + fileName))
        {
            SavedData data = MyLoad.LoadFileBinary<SavedData>(fileName);
            isMuted = data.isMuted;
        }
        if (!plot.isStart) OpenSceneMusic.Play();
        else audioSourceMusic.Play();

        audioSourceMusic.mute = isMuted;
        buttonImage.sprite = isMuted ? offSprite : onSprite;
    }

    void Update ()
    {
        audioSourceMusic.mute = isMuted;
        buttonImage.sprite = isMuted ? offSprite : onSprite;
    }
    public void ChangeLanguage()
    {
        resumeText.text = LanguageSystem.lng.settings[0];
        authorsText.text = LanguageSystem.lng.settings[1];
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
        isMuted = !isMuted;
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

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause (bool pause) {
        if (pause) {
            SavedData data = new SavedData (isMuted);
            Save(data);
        }
        else{
            Awake();
        }
    }

#else
    private void OnApplicationQuit()
    {
        SavedData data = new SavedData(isMuted);
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
        public SavedData(bool IsMuted)
        {
            isMuted = IsMuted;
        }
        public bool isMuted;
    }
}