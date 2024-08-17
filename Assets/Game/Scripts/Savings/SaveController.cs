using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class SaveController : MonoBehaviour
{
    private List<ISaveLoad> _saves;

    private bool _iSaveAll;

    private void Start()
    {
        _saves = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID).OfType<ISaveLoad>().ToList();
    }

    private void SaveAll()
    {
        if (Application.isEditor) Debug.Log("I save all");

        foreach (var save in _saves)
        {
            save.Save();
        }

        YandexGame.SaveProgress();
    }

    public void DeleteAll()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();

        _iSaveAll = true;
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
