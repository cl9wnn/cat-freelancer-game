using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;


public class ProgressBar : MonoBehaviour
{

    public Game gmscript;
    public Slider progressSlider;
    public Text progressText;
    public Text levelText;
    public bool proydeno = true;
    public int Level;
    public float MaxLevelValue;
    public AudioSource levelUpSound;

    private string fileName;

    void Update()
    {
        ProgressSlider();
        Traversed();
        EndGame();
    }

    private void Awake()
    {
        fileName = "progress.BIN";

        if (File.Exists(Application.persistentDataPath + "/Saves/" + fileName))
        {
            SavedData data = MyLoad.LoadFileBinary<SavedData>(fileName);
            proydeno = data.proydeno;
            Level = data.level;
            MaxLevelValue = data.maxLevelValue;
        }
        else
        {
            MaxLevelValue = 100;
            Level = 1;
        }
    }
    private void Start() 
    {
        if (Level == 0) Level = 1;
    }
    void ProgressSlider()
    {
        if (proydeno == true)
        {
            progressSlider.value = (float)gmscript.Score;
            progressSlider.maxValue = (float)MaxLevelValue;

            if (gmscript.Score >= MaxLevelValue)
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
        if (gmscript.Score >= 5000000000000 && proydeno == true)
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

#if UNITY_ANDROID && !UNITY_EDITOR
  private void OnApplicationPause (bool pause) {
    if (pause) {
      SavedData data = new SavedData (proydeno, Level, MaxLevelValue);
      Save(data);
    }
      else Awake ();
  }
#else
    private void OnApplicationQuit()
    {
        SavedData data = new SavedData(proydeno, Level, MaxLevelValue);
        Save(data);
    }
#endif
    private void Save(object Obj)
    {
        MySave.SaveFileBinary(Obj, fileName);
    }

    [Serializable]
    private class SavedData
    {
        public SavedData(bool _proydeno, int _level, float maxLevelValue)
        {
            proydeno = _proydeno;
            level = _level;
            this.maxLevelValue = maxLevelValue;
        }
        public bool proydeno;
        public int level;
        public float maxLevelValue;
    }
}