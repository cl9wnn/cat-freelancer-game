using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAnimation : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private RectTransform _panel;
    [SerializeField] private Vector3 _hiddenPanelPosition;
    [SerializeField] private float _transitionDuration;

    [Header("Buttons")]
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        HidePanel();

        if (_openButton != null)
            _openButton.onClick.AddListener(ShowPanel);
        
        if (_closeButton != null)
            _closeButton.onClick.AddListener(HidePanel);
    }
    public void ShowPanel()
    {
        _panel.DOKill();
        _panel.gameObject.SetActive(true);
        _panel.DOAnchorPos(Vector3.zero, _transitionDuration).SetEase(Ease.OutQuad);
    }
    public void HidePanel()
    {
        _panel.DOAnchorPos(_hiddenPanelPosition, _transitionDuration)
                     .SetEase(Ease.InQuad)
                     .OnComplete(() => _panel.gameObject.SetActive(false));
    }
}
