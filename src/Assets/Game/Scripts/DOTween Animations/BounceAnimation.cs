using UnityEngine;
using DG.Tweening;

public class BounceAnimation : MonoBehaviour
{
    [SerializeField] private float _jumpHeight = 60; 
    [SerializeField] private float _jumpDuration = 0.6f;
    [SerializeField] private float _delayAfterAnimation = 1.5f;

    private RectTransform _rectTransform;

    private Vector3 _startPosition;

    private Sequence _sequence;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startPosition = _rectTransform.anchoredPosition;
    }

    public void StartAnimation()
    {
        _sequence?.Kill();
        _rectTransform.anchoredPosition = _startPosition;

        _sequence = DOTween.Sequence();

        _sequence.Append(_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + _jumpHeight, _jumpDuration / 2)
            .SetEase(Ease.OutQuad)); 
        _sequence.Append(_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y, _jumpDuration / 2)
            .SetEase(Ease.InQuad)); 

        _sequence.AppendInterval(_delayAfterAnimation);

        _sequence.SetLoops(-1, LoopType.Restart);
    }

    public void StopAnimation()
    {
        _sequence?.Kill();
        _rectTransform.anchoredPosition = _startPosition;
    }
}
