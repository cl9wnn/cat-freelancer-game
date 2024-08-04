using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BoostAnimation : MonoBehaviour
{
    [Header("Colorize Boost Background")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private float _colorChangeDuration = 1f;
    [SerializeField] private Color[] _colors;

    [Header("x3 Boost Image")]
    [SerializeField] private RectTransform _x3BoostRect;
    [SerializeField] private float minScale = 10f;
    [SerializeField] private float maxScale = 20f;
    [SerializeField] private float _x3ScaleDuration;

    [SerializeField] private Text _boostCountdownText;

    private Sequence _colorSequence;
    private Tween _scaleTween;

    private void Start()
    {
        GameSingleton.Instance.Boost.BoostActivated += StartAnimation;
        GameSingleton.Instance.Boost.BoostDeactivated += StopAnimation;

        if (GameSingleton.Instance.Boost.IsBoostActive)
        {
            StartAnimation();
        }
        else
        {
            StopAnimation();
        }
    }

    public void StartAnimation()
    {
        StopAllAnimations();

        _backgroundImage.gameObject.SetActive(true);
        _x3BoostRect.gameObject.SetActive(true);
        _boostCountdownText.gameObject.SetActive(true);

        StartColorAnimation();
        StartScaleAnimation();
    }
    public void StopAnimation()
    {
        StopAllAnimations();

        _backgroundImage.gameObject.SetActive(false);
        _x3BoostRect.gameObject.SetActive(false);
        _boostCountdownText.gameObject.SetActive(false);
    }

    private void StopAllAnimations()
    {
        _colorSequence?.Kill();
        _scaleTween?.Kill();
    }

    private void StartColorAnimation()
    {
        _colorSequence = DOTween.Sequence();

        _backgroundImage.color = _colors[0];

        foreach (var color in _colors)
        {
            _colorSequence.Append(_backgroundImage.DOColor(color, _colorChangeDuration));
        }

        _colorSequence.SetLoops(-1, LoopType.Restart);
    }
    private void StartScaleAnimation()
    {
        _scaleTween = _x3BoostRect.DOScale(new Vector2(maxScale, maxScale), _x3ScaleDuration)
                                       .SetEase(Ease.InOutSine)
                                       .SetLoops(-1, LoopType.Yoyo);
    }
}
