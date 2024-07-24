using UnityEngine;
using DG.Tweening;

public class FallingDollars : MonoBehaviour
{
    [SerializeField] private float fallDuration = 2.5f; 
    [SerializeField] private float fallPositionY = 5.5f; 
    [SerializeField] private float swayDuration = 0.5f; 
    [SerializeField] private float swayStrength = 0.5f; 

    private void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Sequence fallAndSwaySequence = DOTween.Sequence();

            //fallAndSwaySequence.Append(rectTransform.DOShakePosition(swayDuration, swayStrength)
            //   .SetEase(Ease.InOutSine));
      
            fallAndSwaySequence.Append(rectTransform.DOMoveY(fallPositionY, fallDuration)
                .SetEase(Ease.Linear)
                .OnKill(() => Destroy(gameObject)));


            fallAndSwaySequence.Play();
        }
    }
}
