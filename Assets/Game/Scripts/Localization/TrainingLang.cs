using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;


public class TrainingLang : MonoBehaviour
{
    public static lanng lng = new lanng();
    private string json;
    private string[] langArray = { "ru_RU", "en_US" };

    void Awake()
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

    void LangLoad()
    {
        string path = PlayerPrefs.GetString("Language");
        json = Resources.Load<TextAsset>(path).text;

        Debug.Log("Язык: " + PlayerPrefs.GetString("Language") + " " + (json == null ? " Не загружен" : "Загружен"));

        lng = JsonUtility.FromJson<lanng>(json);
    }

}

public class lanng
{
    public string[] training;
}

