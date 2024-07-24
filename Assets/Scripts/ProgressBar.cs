using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using YG;


public class ProgressBar : MonoBehaviour
{

    public Slider progressSlider;
    public Text progressText;
    public Text levelText;
    public bool proydeno = true;
    public int Level;
    public float MaxLevelValue;
    public AudioSource levelUpSound;

    private Game _game;

    private void Awake()
    {
        _game = GameSingleton.Instance.Game;

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
        YandexGame.savesData.progressBarData = new ProgressBarData(proydeno, Level, MaxLevelValue);
    }
    private void Load()
    {
        var data = YandexGame.savesData.progressBarData;

        if (data == null) return;

        proydeno = data.proydeno;
        Level = data.level;
        MaxLevelValue = data.maxLevelValue;
    }

    private void Start() 
    {
        if (Level == 0) Level = 1;
    }
    void Update()
    {
        ProgressSlider();
        Traversed();
        EndGame();
    }
    void ProgressSlider()
    {
        if (proydeno == true)
        {
            progressSlider.value = (float)_game.Score;
            progressSlider.maxValue = (float)MaxLevelValue;

            if (_game.Score >= MaxLevelValue)
            {
                ++Level;
                levelUpSound.Play();
                MaxLevelValue *= 100;
            }

            progressText.text = (progressSlider.value * 100 / progressSlider.maxValue).ToString("G") + "%";
            levelText.text = LanguageSystem.lng.settings[2] + Level.ToString();
        }

    }
    void Traversed()
    {
        if (_game.Score >= 5000000000000 && proydeno == true)
        {
            proydeno = false;
        }
    }
    void EndGame()
    {
        if (proydeno == false)
        {
            progressText.text = LanguageSystem.lng.info[5];
        }
    }
}