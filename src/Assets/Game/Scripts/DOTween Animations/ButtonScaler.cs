using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScaler : MonoBehaviour
{
    private Button button;
    private Vector3 originalScale;
    private float animationTime = 0.12f; 
    private float pauseTime = 0.1f; 

    private void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;

        if (button != null)
        {
            button.onClick.AddListener(AnimateButton);
        }
    }

    private void AnimateButton()
    {
        Vector3 targetScale = originalScale * 0.98f;

        transform.DOScale(targetScale, animationTime)
                 .OnComplete(() =>
                     transform.DOScale(originalScale, animationTime)
                              .SetDelay(pauseTime));
    }
}
