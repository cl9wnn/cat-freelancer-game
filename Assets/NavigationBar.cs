using DG.Tweening;
using UnityEngine;

public class NavigationBar : MonoBehaviour
{
    [Header("Center Navigation Button")]
    [SerializeField] private NavigationButton _mainCenterButton;

    [Header("Shop Navigation Buttons")]
    [SerializeField] private NavigationButton[] _navigationButtons;

    [Header("Panels + DOTween")]
    [SerializeField] private RectTransform[] _panels;

    [SerializeField] private float _transitionDuration = 0.2f;
    [SerializeField] private Vector3 _hiddenPanelPosition = new Vector3(0, -1850, 0);

    private void OnEnable()
    {
        GameSingleton.Instance.Fortune.WheelStartedSpinning += DisableAllButtons;
        GameSingleton.Instance.Fortune.WheelStoppedSpinning += EnableAllButtons;
    }
    private void OnDisable()
    {
        GameSingleton.Instance.Fortune.WheelStartedSpinning -= DisableAllButtons;
        GameSingleton.Instance.Fortune.WheelStoppedSpinning -= EnableAllButtons;
    }

    private void Start()
    {
        InitializeMainButton();
        InitializeNavigationButtons();

        CloseAllPanels();
        EnableAllButtons();
        _mainCenterButton.IsActive = true;
    }

    private void EnableAllButtons()
    {
        _mainCenterButton.Button.interactable = true; 

        foreach (var button in _navigationButtons)
        {
            button.Button.interactable = true;
        }
    }
    private void DisableAllButtons()
    {
        _mainCenterButton.Button.interactable = false; 
       
        foreach (var button in _navigationButtons)
        {
            button.Button.interactable = false;
        }
    }

    private void InitializeMainButton()
    {
        _mainCenterButton.Button.onClick.AddListener(ToggleMainButton);
    }

    private void InitializeNavigationButtons()
    {
        for (int i = 0; i < _navigationButtons.Length; i++)
        {
            int index = i;
            _navigationButtons[i].Button.onClick.AddListener(() => TogglePanel(index));
        }
    }

    private void ToggleMainButton()
    {
        _mainCenterButton.IsActive = !_mainCenterButton.IsActive;
        if (_mainCenterButton.IsActive)
        {
            CloseAllPanels();
        }
    }

    private void TogglePanel(int index)
    {
        if (index < 0 || index >= _navigationButtons.Length)
        {
            Debug.LogWarning("Invalid panel index");
            return;
        }

        bool isCurrentlyActive = _navigationButtons[index].IsActive;
        CloseAllPanels();

        if (!isCurrentlyActive)
        {
            OpenPanel(index);
        }
    }

    private void OpenPanel(int index)
    {
        _navigationButtons[index].IsActive = true;
        var panel = _panels[index];

        panel.DOKill();
        panel.gameObject.SetActive(true);
        panel.DOAnchorPos(Vector3.zero, _transitionDuration).SetEase(Ease.OutQuad);
    }

    private void CloseAllPanels()
    {
        foreach (var button in _navigationButtons)
        {
            button.IsActive = false;
        }

        foreach (var panel in _panels)
        {
            ClosePanel(panel);
        }

        _mainCenterButton.IsActive = false;
    }

    private void ClosePanel(RectTransform panel)
    {
        panel.DOAnchorPos(_hiddenPanelPosition, _transitionDuration)
             .SetEase(Ease.InQuad)
             .OnComplete(() => panel.gameObject.SetActive(false));
    }
}
