using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

[DefaultExecutionOrder(1000)]
public class SaveController : MonoBehaviour
{
    private List<ISaveLoad> _saves;

    private bool _iSaveAll;

    private void Awake()
    {
        _saves = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID).OfType<ISaveLoad>().ToList();

        if (YandexGame.SDKEnabled)
            LoadAll();
    }

    private void OnEnable() => YandexGame.GetDataEvent += LoadAll;
    private void OnDisable() => YandexGame.GetDataEvent -= LoadAll;

    private void SaveAll()
    {
        if (Application.isEditor) Debug.Log("I save all");

        foreach (var save in _saves)
        {
            save.Save();
        }

        YandexGame.SaveProgress();
    }
    private void LoadAll()
    {
        if (Application.isEditor) Debug.Log("I load all");

        foreach (var save in _saves)
        {
            save.Load();
        }
    }
    public void DeleteAll()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();

        _iSaveAll = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && !_iSaveAll) SaveAll();
        else
        {
            _iSaveAll = false;
            return;
        }

        _iSaveAll = true;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause && !_iSaveAll) SaveAll();
        else
        {
            _iSaveAll = false;
            return;
        }

        _iSaveAll = true;
    }
    private void OnApplicationQuit()
    {
        if (_iSaveAll) return;

        SaveAll();
    }
}
