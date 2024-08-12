using UnityEngine;
using DG.Tweening;

public class ClipboardShake : MonoBehaviour
{
   [SerializeField] private float _shakeInterval = 60f;
   [SerializeField] private float _shakeDuration = 0.5f;
   [SerializeField] private float _shakeStrength = 5f;
   [SerializeField] private float _swingDuration = 2f;
   [SerializeField] private float _swingAmount = 10f;

    private RectTransform _rectTransform;

    private void Awake() => _rectTransform = GetComponent<RectTransform>();
    private void Start() => StartShaking();
    private void StartShaking() => InvokeRepeating(nameof(ShakeAndSwing), 0f, _shakeInterval);
    private void ShakeAndSwing()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_rectTransform.DOShakeAnchorPos(_shakeDuration, _shakeStrength, 10, 90, true));

        sequence.Append(_rectTransform.DORotate(new Vector3(0, 0, _swingAmount), _swingDuration).SetEase(Ease.InOutSine));
        sequence.Append(_rectTransform.DORotate(new Vector3(0, 0, -_swingAmount), _swingDuration).SetEase(Ease.InOutSine));
        sequence.Append(_rectTransform.DORotate(new Vector3(0, 0, 2), _swingDuration).SetEase(Ease.InOutSine));
    }
}
