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
    [SerializeField] private Button _closeButton;

    private void Awake()
    {       
        HideSettingsPanel();

        _openButton.onClick.AddListener(ShowSettingsPanel);
        _openButton.onClick.AddListener(LeftRotateButton);
       
        _closeButton.onClick.AddListener(HideSettingsPanel);
        _closeButton.onClick.AddListener(RightRotateButton);

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
        _settingsPanel.DOAnchorPos(Vector3.zero, _transitionDuration).SetEase(Ease.OutQuad);
    }

    private void HideSettingsPanel()
    {
        _settingsPanel.DOAnchorPos(_hiddenPanelPosition, _transitionDuration)
                     .SetEase(Ease.InQuad)
                     .OnComplete(() => _settingsPanel.gameObject.SetActive(false));
    }
}
