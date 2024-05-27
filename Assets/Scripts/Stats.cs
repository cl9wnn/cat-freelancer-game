using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class Stats : MonoBehaviour
{
    public Text[] stringNames;
    public Text[] statsText;
    public Text totalTimeText;
    private float totalPlayTime = 0f;
    private float sessionStartTime;
    private string fileName;

    private void Awake()
    {
        fileName = "Stats.BIN";
        if (File.Exists(Application.persistentDataPath + "/Saves/" + fileName))
        {
            SavedData data = MyLoad.LoadFileBinary<SavedData>(fileName);
            totalPlayTime = data.totalPlayTime;
        }
    }
    void Start()
    {
        sessionStartTime = Time.time;
    }
    void Update()
    {
        if ((int)totalPlayTime < 60)
        {
            totalTimeText.text = (int)totalPlayTime + LanguageSystem.lng.time[7];
        }
        else if ((int)totalPlayTime < 3600)
        {
            totalTimeText.text = (int)(totalPlayTime / 60) + LanguageSystem.lng.time[2];
        }
        else totalTimeText.text = (int)(totalPlayTime / 3600) + LanguageSystem.lng.time[0];

        if (Application.isFocused)
        {
            float sessionTime = Time.time - sessionStartTime;
            totalPlayTime += sessionTime;
            sessionStartTime = Time.time; 
        }
    }
    public void ChangeLanguage()
    {
        for (int i = 0; i < stringNames.Length; i++)
        {
            stringNames[i].text = LanguageSystem.lng.statsString[i];
        }
        
        for (int i = 0; i < 3; i++)
        {
            statsText[i].text = LanguageSystem.lng.statsText[i];
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause (bool pause) {
        if (pause)
        {
            SavedData data = new SavedData (totalPlayTime);
           Save(data);
        } else {
            Awake ();
        }
    }
#else
    private void OnApplicationQuit()
    {
        SavedData data = new SavedData(totalPlayTime);
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
        public SavedData(float TotalPlayTime)
        {
            totalPlayTime = TotalPlayTime;
        }
        public float totalPlayTime;
    }
}
