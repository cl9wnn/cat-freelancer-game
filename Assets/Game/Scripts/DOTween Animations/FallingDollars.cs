using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FallingDollars : MonoBehaviour
{
    [SerializeField] private float fallDuration = 2.5f; 
    [SerializeField] private float fallPositionY = 1f; 
    [SerializeField] private float swayDuration = 0.2f; 
    [SerializeField] private float swayStrength = 5f; 

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
            fallAndSwaySequence.Insert(1.5f, rectTransform.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.Linear));


            fallAndSwaySequence.Play();
        }
    }
}
