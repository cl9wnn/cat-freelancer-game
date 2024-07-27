using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AlbumShake : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;

    [SerializeField] private float _shakeDuration = 1.0f;
    [SerializeField] private float _shakeStrength = 10f;
    [SerializeField] private int _shakeVibrato = 5;
    [SerializeField] private float _shakeRandomness = 90.0f;

    [SerializeField] private float _bounceDuration = 0.5f;
    [SerializeField] private float _bounceStrength = 0.2f;

    [SerializeField] private float _rotationDuration = 0.5f;
    [SerializeField] private float _rotationAngle = 15.0f;

    [SerializeField] private Ease _shakeEase = Ease.InOutBounce;

    private Vector2 _initialPosition;
    private Quaternion _initialRotation;
    private Vector3 _initialScale;

    private Sequence _albumSequence;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _initialPosition = _rectTransform.anchoredPosition;
        _initialRotation = _rectTransform.rotation;
        _initialScale = _rectTransform.localScale;

        GetComponent<Button>().onClick.AddListener(StopAnimation);

        StopAnimation();
    }

    private void Start()
    {
        GameSingleton.Instance.Achievements.OnAchievementComplete += ShakeAndBounce;
    }

    [ContextMenu("Shake")]
    public void ShakeAndBounce()
    {
        Debug.Log(323224);
        StopAnimation();

        _particles.gameObject.SetActive(true);

        _albumSequence = DOTween.Sequence();

        _albumSequence.Append(_rectTransform.DOShakeAnchorPos(_shakeDuration, _shakeStrength, _shakeVibrato, _shakeRandomness)
                                    .SetEase(_shakeEase));

        _albumSequence.Join(_rectTransform.DORotate(new Vector3(0, 0, _rotationAngle), _rotationDuration)
                                    .SetEase(_shakeEase)
                                    .SetLoops(4, LoopType.Yoyo));

        _albumSequence.Join(_rectTransform.DOScale(new Vector3(1 + _bounceStrength, 1 + _bounceStrength, 1), _bounceDuration)
                                    .SetEase(_shakeEase)
                                    .SetLoops(4, LoopType.Yoyo));
    }

    public void StopAnimation()
    {
        _albumSequence?.Kill();

        _rectTransform.anchoredPosition = _initialPosition;
        _rectTransform.rotation = _initialRotation;
        _rectTransform.localScale = _initialScale;

        _particles.gameObject.SetActive(false);
    }
}
