using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FingerAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _pointer;

    [SerializeField] private float _distance;
    [SerializeField] private Vector2 _diretion = new Vector2(-0.788f , -0.615f);
    [SerializeField] private float _duration;

    [SerializeField] private float _lifetime;

    void Start()
    {
        var endPosition = _pointer.anchoredPosition + _diretion * _distance;
        
        _pointer.DOAnchorPos(endPosition, _duration)
            .SetEase(Ease.InOutSine) 
            .SetLoops((int)(_lifetime / _duration), LoopType.Yoyo)
            .OnComplete(() => Destroy(gameObject)); 
    }
}
