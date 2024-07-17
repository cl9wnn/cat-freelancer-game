using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnim : MonoBehaviour
{
    [SerializeField] private Canvas _popupCanvas;

    private ObjectPool _pool;

    private Game _game;
    private Boost _boost;
    private Fortune _fortune;

    private Camera _camera;

    private GameObject spawnedPopupText;

    private void Start()
    {
        _pool = GameSingleton.Instance.Pool;

        _game = GameSingleton.Instance.Game;
        _boost = GameSingleton.Instance.Boost;
        _fortune = GameSingleton.Instance.Fortune;

        _camera = Camera.main;
    }

    public void OnClick()
    {
        if (!_boost.BoostOn && !_fortune.coffeeRewarded || _boost.BoostOn || _fortune.coffeeRewarded)
        {
            CreateTextPopup();
        }
    }

    private void CreateTextPopup()
    {
        var position = _camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;

        spawnedPopupText = _pool.Get();
        if (spawnedPopupText == null) return;

        spawnedPopupText.transform.SetParent(_popupCanvas.transform, false);
        spawnedPopupText.transform.position = position;
    }
}