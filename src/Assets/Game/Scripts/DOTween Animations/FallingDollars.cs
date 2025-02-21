using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FallingDollars : MonoBehaviour
{
    [SerializeField] private float fallDuration = 1.8f;
    [SerializeField] private float fallPositionY = 1f;
    [SerializeField] private float swayDuration = 0.2f;
    [SerializeField] private float swayStrength = 5f;
    [SerializeField] private float rotationStrength = 30f;
    [SerializeField] private float rotationDuration = 0.8f; 

    private void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Sequence fallAndSwaySequence = DOTween.Sequence();

            // �������� �������
            fallAndSwaySequence.Append(rectTransform.DOMoveY(fallPositionY, fallDuration)
                .SetEase(Ease.Linear));

            // �������� ������������
            fallAndSwaySequence.Insert(1.5f, rectTransform.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.Linear));

            // ������� ����� � ������
            fallAndSwaySequence.Join(rectTransform.DORotate(new Vector3(0, 0, rotationStrength), rotationDuration, RotateMode.LocalAxisAdd)
                               .SetEase(Ease.InOutSine)
                               .SetLoops(-1, LoopType.Yoyo));

            // �����������
            fallAndSwaySequence.Join(rectTransform.DOShakePosition(swayDuration, swayStrength, 10, 90, true, false)
                               .SetLoops(30, LoopType.Yoyo))
                               .OnKill(() => Destroy(gameObject));

            // ����������� ������ �����������
            rectTransform.DORotate(new Vector3(0, 0, rotationStrength), rotationDuration, RotateMode.LocalAxisAdd)
                         .SetEase(Ease.InOutSine)
                         .SetLoops(-1, LoopType.Yoyo)
                         .Play();

            fallAndSwaySequence.Play();
        }
    }
}