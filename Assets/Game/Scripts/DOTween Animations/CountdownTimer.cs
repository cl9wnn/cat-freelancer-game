using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private Text _countdownText;  
    [SerializeField] private float _countdownDuration = 10f;

    public float CountdownDuration
    {
        get => _countdownDuration;
        set => _countdownDuration = value;
    }

    [ContextMenu("Play")]
    public void Play()
    {
        StartCoroutine(StartCountdown());
    }

    public IEnumerator StartCountdown()
    {
        float remainingTime = _countdownDuration;

        while (remainingTime > 0)
        {
            _countdownText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                _countdownText.transform.DOScale(1f, 0.5f).SetEase(Ease.InBack);
            });

            _countdownText.DOColor(Color.red, 0.5f).OnComplete(() =>
            {
                _countdownText.DOColor(Color.white, 0.5f);
            });

            _countdownText.transform.DORotate(new Vector3(0, 0, 15), 0.25f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);

            _countdownText.transform.DOShakePosition(0.5f, strength: new Vector3(10, 10, 0), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);

            _countdownText.text = Mathf.CeilToInt(remainingTime).ToString();

            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        _countdownText.transform.DOScale(2f, 0.5f).SetEase(Ease.OutElastic).OnComplete(() =>
        {
            _countdownText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack);
        });
        _countdownText.text = "Go!";
    }
}
