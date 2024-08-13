using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelAnimation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private int _rotationCount = 3;
    [SerializeField] private float _rotationDuration = 0.5f;

    [Header("Panel Settings")]
    [SerializeField] private RectTransform _settingsPanel;
    [SerializeField] private Vector3 _hiddenPanelPosition;
    [SerializeField] private float _transitionDuration;

    [Header("Buttons")]
    [SerializeField] private Button _openButton;
    [SerializeField] private Button[] _closeButtons;

    [Header("Device")]
    [SerializeField] private bool IsMobile;

    private Vector3 _initPosition;

    private void Awake()
    {       
        _settingsPanel.gameObject.SetActive(false);

        HideSettingsPanel();

        _openButton.enabled = true;

        _openButton.onClick.AddListener(ShowSettingsPanel);
        _openButton.onClick.AddListener(LeftRotateButton);
       
        foreach (var button in _closeButtons)
        {
            button.onClick.AddListener(HideSettingsPanel);
            button.onClick.AddListener(RightRotateButton);
        }

        _initPosition = _settingsPanel.position;
    }

    private void LeftRotateButton()
    {
        transform.DORotate(Vector3.forward * 360f * _rotationCount, _rotationDuration * _rotationCount, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear);
    }

    private void RightRotateButton()
    {
        transform.DORotate(-Vector3.forward * 360f * _rotationCount, _rotationDuration * _rotationCount, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear);
    }

    private void ShowSettingsPanel()
    {
        _settingsPanel.DOKill();
        _settingsPanel.gameObject.SetActive(true);

        if (IsMobile)
        {
             _settingsPanel.DOAnchorPos(Vector3.zero, _transitionDuration).SetEase(Ease.OutQuad);
            return;
        }

        _settingsPanel.localScale = Vector3.zero;
        _settingsPanel.position = _openButton.transform.position;

        _openButton.enabled = false;

        Vector3 targetScreenPosition = new Vector3(Screen.width / 3, Screen.height / 2, 0);

        Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(targetScreenPosition);
        targetWorldPosition.z = _settingsPanel.position.z;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_settingsPanel.DOMove(targetWorldPosition, _transitionDuration).SetEase(Ease.OutBack));
        sequence.Join(_settingsPanel.DOScale(Vector3.one, _transitionDuration).SetEase(Ease.OutBack));
    }

    private void HideSettingsPanel()
    {
        if (!IsMobile)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_settingsPanel.DOScale(Vector3.zero, _transitionDuration).SetEase(Ease.InBack));
            sequence.Join(_settingsPanel.DOMove(_openButton.transform.position, _transitionDuration).SetEase(Ease.InBack));
            sequence.OnComplete(() =>
            {
                _settingsPanel.gameObject.SetActive(false);
                _settingsPanel.position = _initPosition;

                _openButton.enabled = true;
            });
        }
        else
        {
            _settingsPanel.DOAnchorPos(_hiddenPanelPosition, _transitionDuration)
                         .SetEase(Ease.InQuad)
                         .OnComplete(() => _settingsPanel.gameObject.SetActive(false));
        }
    }
}
