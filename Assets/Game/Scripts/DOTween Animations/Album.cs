using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Album : MonoBehaviour
{
    [Header("Album Panel")]
    [SerializeField] private RectTransform _panel;
    [SerializeField] private float _transitionDuration = 1f; 

    [SerializeField] private float _leftDistance;
    [SerializeField] private float _rightDistance;

    [Header("Buttons")]
    [SerializeField] private Button _openButton;
    [SerializeField] private Button[] _closeButtons;


    private void Awake()
    {
        _openButton.onClick.AddListener(OpenAlbum);

        for (int i = 0; i < _closeButtons.Length; i++)
        {
            _closeButtons[i].onClick.AddListener(CloseAlbum);
        }
        
        _panel.gameObject.SetActive(false);
    }

    public void OpenAlbum()
    {
        _panel.gameObject.SetActive(true);

        GameSingleton.Instance.SoundManager.CreateSound()
                                           .WithSoundData(SoundEffect.OPEN_ACHIEVEMENTS_BOOK)
                                           .Play();

        SelectLeftSide();
    }

    public void SelectRightSide()
    {
        Vector3 targetPosition = new Vector2(_rightDistance, 0);

        _panel.DOAnchorPos(targetPosition, _transitionDuration).SetEase(Ease.Linear);
    }

    public void SelectLeftSide()
    {
        Vector3 targetPosition = new Vector2(_leftDistance, 0);

        _panel.DOAnchorPos(targetPosition, _transitionDuration).SetEase(Ease.Linear);
    }

    public void CloseAlbum()
    {
        Vector3 targetPosition = Vector3.zero;

        GameSingleton.Instance.SoundManager.CreateSound()
                                           .WithSoundData(SoundEffect.CLOSE_ACHIEVEMENT_BOOK)
                                           .Play();

        _panel.DOAnchorPos(targetPosition, _transitionDuration)
           .SetEase(Ease.Linear)
           .OnComplete(() => {
               _panel.gameObject.SetActive(false);
           });
    }
}
