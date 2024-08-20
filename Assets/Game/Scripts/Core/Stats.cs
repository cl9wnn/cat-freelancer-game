using UnityEngine;
using UnityEngine.UI;
using YG;
public class Stats : MonoBehaviour, ISaveLoad
{
    public Text[] stringNames;
    public Text[] statsText;
    public Text totalTimeText;
    private float totalPlayTime = 0f;

    public void Save()
    {
        ref var data = ref YandexGame.savesData.statsData;

        if (data == null)
        {
            data = new StatsData(totalPlayTime);
            return;
        }

        data.totalPlayTime = totalPlayTime;
    }
    public void Load()
    {
        var data = YandexGame.savesData.statsData;

        if (data == null) return;

        totalPlayTime = data.totalPlayTime;
    }

    private void Update()
    {
        if (Application.isFocused)
        {
            totalPlayTime += Time.deltaTime;
            
            totalTimeText.text = FormatTime(totalPlayTime);
        }
    }
    private string FormatTime(float totalTimeInSeconds)
    {
        int totalSeconds = (int)totalTimeInSeconds;
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        if (hours > 0)  
        {
            return $"{hours} {LanguageSystem.lng.time[0]} {minutes} {LanguageSystem.lng.time[2]}";
        }
        else if (minutes > 0)
        {
            return $"{minutes} {LanguageSystem.lng.time[2]} {seconds} {LanguageSystem.lng.time[7]}";
        }
        else
        {
            return $"{seconds} {LanguageSystem.lng.time[7]}";
        }
    }   

    public void ChangeLanguage()
    {
        for (int i = 0; i < stringNames.Length; i++)
        {
            stringNames[i].text = LanguageSystem.lng.statsString[i];
            Debug.Log(LanguageSystem.lng.statsString[i]);
        }
        
        for (int i = 0; i < 3; i++)
        {
            statsText[i].text = LanguageSystem.lng.statsText[i];
        }
    }
}
