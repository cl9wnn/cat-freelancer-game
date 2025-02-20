using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class BootstrapScenes : MonoBehaviour
{
    [SerializeField] private string _mobileSceneName;
    [SerializeField] private string _pcSceneName;

    private void Start()
    {
        if (YandexGame.SDKEnabled)
            LoadScene();
        else
            YandexGame.GetDataEvent += LoadScene;
    }

    public void LoadScene()
    {
        Debug.Log(YandexGame.EnvironmentData.deviceType);
        if (YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet)
            SceneManager.LoadScene(_mobileSceneName);
        else
            SceneManager.LoadScene(_pcSceneName);
    }


}
