using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StarAnimation : MonoBehaviour
{
    public float animationDuration = 1f;
    public Color startColor = new Color(1, 1, 1, 0); 
    public Color endColor = Color.white;
    public float delayBeforeStart;

    [ContextMenu("Rotate")]
    public void AnimateStar()
    {
        var image = GetComponent<Image>();

        if (image != null)
        {
            image.color = startColor;
            AnimateStar(image);
        }
    }

    private void AnimateStar(Image image)
    {
        Sequence starSequence = DOTween.Sequence();

        starSequence.AppendInterval(delayBeforeStart);
        starSequence.Append(transform.DOScale(1.5f, animationDuration * 0.5f).SetEase(Ease.OutBack));
        starSequence.Join(image.DOColor(endColor, animationDuration * 0.5f));
        starSequence.Append(transform.DOScale(1f, animationDuration * 0.5f).SetEase(Ease.InBack));
        starSequence.Join(transform.DORotate(new Vector3(0, 0, 360 + image.transform.eulerAngles.z), animationDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutSine));
        starSequence.Play();
    }
}
