using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Language
{
    public string languageCode;
    public Sprite flagSprite;
}

public class SwitchLanguageButton : MonoBehaviour
{
    [SerializeField] private Language[] _languages;
    [SerializeField] private Image _flagImage;       
    private int currentLanguageIndex = 0;

    private void Start()
    {
        var languageCode = GameSingleton.Instance.LanguageSystem.LanguageCode;
        Debug.Log(languageCode);
        InitializeButton(languageCode);
    }

    private void InitializeButton(string savedLanguageCode)
    {
        for (int i = 0; i < _languages.Length; i++)
        {
            if (_languages[i].languageCode == savedLanguageCode)
            {
                currentLanguageIndex = i;
                _flagImage.sprite = _languages[i].flagSprite;
                return;
            }
        }

        currentLanguageIndex = 0;
        _flagImage.sprite = _languages[0].flagSprite;
    }

    public void OnSwitchLanguage()
    {
        currentLanguageIndex = (currentLanguageIndex + 1) % _languages.Length;

        _flagImage.sprite = _languages[currentLanguageIndex].flagSprite;

        UpdateLanguage(_languages[currentLanguageIndex].languageCode);
    }

    private void UpdateLanguage(string languageCode)
    {
        GameSingleton.Instance.LanguageSystem.SetLanguage(languageCode);
    }
}
