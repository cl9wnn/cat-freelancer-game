using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuAnimator : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] _menuButtons;

    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 1.0f;
    [SerializeField] private float _delayBetweenButtons = 0.2f;
    [SerializeField] private float _startYPosition = -1500f;

    [SerializeField] private Ease _easeType = Ease.OutBounce; 
    [SerializeField] private float startDelay = 0f;

    private void Start()
    {
        AnimateButtons();
    }
    [ContextMenu("OPPP")]
    private void AnimateButtons()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.PrependInterval(startDelay);

        foreach (var button in _menuButtons)
        {
            sequence.Append(AnimateButton(button));
        }
    }

    private Tween AnimateButton(Button button)
    {
        RectTransform buttonTransform = button.GetComponent<RectTransform>();
        Vector2 originalPosition = buttonTransform.anchoredPosition;
        buttonTransform.anchoredPosition = new Vector2(originalPosition.x, _startYPosition);

        return DOTween.Sequence().
            Append(buttonTransform.DOAnchorPosY(originalPosition.y, _animationDuration).SetEase(_easeType)).
            AppendInterval(_delayBetweenButtons);
    }
}
