using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropMoneyPanel : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private Vector3 hiddenPanelPosition;
    [SerializeField] private float transitionDuration;

    [Header("General")]
    [SerializeField] private Vector3 replyTargetPosition;
    [SerializeField] private Button actionButton;
    [SerializeField] private Text actionButtonText;

    [Header("First Launch")]
    [SerializeField] private Image background;
    [SerializeField] private Image firstMessageImage;
    [SerializeField] private RectTransform firstReplyRect;

    [Header("Subsequent Launch")]
    [SerializeField] private Text collectedCoinsText;
    [SerializeField] private Text levelText;
    [SerializeField] private CountdownTimer getReadyTimer;
    [SerializeField] private Image levelImage;

    [Header("End Launch")]
    [SerializeField] private Button endActionButton;
    [SerializeField] private Image endMessageImage;
    [SerializeField] private RectTransform endReplyRect;

    public float CountdownDuration
    {
        get => getReadyTimer.CountdownDuration;
        set => getReadyTimer.CountdownDuration = value;
    }

    public void HandleFirstLaunch()
    {
        ShowPanel();

        firstMessageImage.gameObject.SetActive(true);
        firstReplyRect.gameObject.SetActive(true);
        actionButton.gameObject.SetActive(true);
        actionButton.onClick.AddListener(HideFirstLaunchElements);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(firstMessageImage.DOFade(1f, 0.5f).SetEase(Ease.Linear));
        AppendTextFadeInSequence(sequence, firstMessageImage.transform, 0.5f);
      
        sequence.AppendInterval(1.5f);
      
        sequence.Append(firstReplyRect.GetComponent<Image>().DOFade(1.0f, 0.5f));
        AppendTextFadeInSequence(sequence, firstReplyRect, 0.25f);
        sequence.Join(firstReplyRect.DOAnchorPos(replyTargetPosition, 0.5f).SetEase(Ease.InOutQuad));
      
        sequence.AppendInterval(1f);
       
        sequence.Append(actionButtonText.DOFade(1f, 0.5f).SetEase(Ease.Linear));
    }
    public void HandleEndLaunch()
    {
        ShowPanel();

        endMessageImage.gameObject.SetActive(true);
        endReplyRect.gameObject.SetActive(true);
        endActionButton.gameObject.SetActive(true);
        endActionButton.onClick.AddListener(HideEndLaunchElements);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(endMessageImage.DOFade(1f, 0.5f).SetEase(Ease.Linear));
        AppendTextFadeInSequence(sequence, endMessageImage.transform, 0.5f);

        sequence.AppendInterval(1.5f);

        sequence.Append(endReplyRect.GetComponent<Image>().DOFade(1.0f, 0.25f));
        AppendTextFadeInSequence(sequence, endReplyRect.transform, 0.5f);
        sequence.Append(endReplyRect.DOAnchorPos(replyTargetPosition, 0.5f).SetEase(Ease.InOutQuad));
   
        sequence.AppendInterval(1f);

        sequence.Append(actionButtonText.DOFade(1f, 0.5f).SetEase(Ease.Linear));
    }

    private void AppendTextFadeInSequence(Sequence sequence, Transform parent, float duration)
    {
        foreach (var textComponent in parent.GetComponentsInChildren<Text>())
        {
            sequence.Join(textComponent.DOFade(1f, duration).SetEase(Ease.Linear));
        }
    }

    public IEnumerator HandleSubsequentLaunch()
    {
        ShowPanel();

        yield return new WaitForSeconds(0.5f);

        collectedCoinsText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        getReadyTimer.gameObject.SetActive(true);
        levelImage.gameObject.SetActive(true);

        getReadyTimer.transform.localScale = Vector3.one;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(collectedCoinsText.DOFade(1.0f, 0.5f).SetEase(Ease.Linear));
        sequence.Join(levelText.DOFade(1.0f, 0.5f).SetEase(Ease.Linear));
        sequence.Join(levelImage.DOFade(1.0f, 0.5f).SetEase(Ease.Linear));


        yield return levelImage.DOFade(1, 1);

        yield return new WaitForSeconds(0.5f);

        getReadyTimer.CountdownDuration = 3f;

        yield return new WaitForSeconds(2f);

        GameSingleton.Instance.SoundManager.CreateSound()
                                           .WithSoundData(SoundEffect.COUNTDOWN)
                                           .Play();

        yield return getReadyTimer.StartCountdown();

        levelText.gameObject.SetActive(false);
        getReadyTimer.gameObject.SetActive(false);
        levelImage.gameObject.SetActive(false);
    }

    private void HideFirstLaunchElements()
    {
        HideElements(firstMessageImage, firstReplyRect, HideFirstLaunchCleanup);
    }
    private void HideEndLaunchElements()
    {
        HideElements(endMessageImage, endReplyRect, HideEndLaunchCleanup);
    }

    private void HideElements(Image messageImage, RectTransform replyRect, TweenCallback onComplete)
    {
        Sequence hideSequence = DOTween.Sequence();

        hideSequence.Append(messageImage.DOFade(0, 0.5f).SetEase(Ease.InBack));
        AppendTextFadeOutSequence(hideSequence, messageImage.transform, 0.5f);

        hideSequence.Join(replyRect.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.InBack));
        AppendTextFadeOutSequence(hideSequence, replyRect, 0.5f);

        AppendTextFadeOutSequence(hideSequence, replyRect, 0.5f);
        hideSequence.Join(actionButtonText.DOFade(0f, 0.5f).SetEase(Ease.InBack));
        hideSequence.OnComplete(onComplete);
    }

    private void AppendTextFadeOutSequence(Sequence sequence, Transform parent, float duration)
    {
        foreach (var textComponent in parent.GetComponentsInChildren<Text>())
        {
            sequence.Join(textComponent.DOFade(0f, duration).SetEase(Ease.InBack));
        }
    }

    private void HideFirstLaunchCleanup()
    {
        firstReplyRect.gameObject.SetActive(false);
        firstMessageImage.gameObject.SetActive(false);
        actionButton.onClick.RemoveListener(HideFirstLaunchElements);
        actionButton.gameObject.SetActive(false);
    }
    private void HideEndLaunchCleanup()
    {
        endReplyRect.gameObject.SetActive(false);
        endMessageImage.gameObject.SetActive(false);
        endActionButton.onClick.RemoveListener(HideEndLaunchElements);
        endActionButton.gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        panelRectTransform.DOKill();
        panelRectTransform.gameObject.SetActive(true);
        panelRectTransform.DOAnchorPos(Vector3.zero, transitionDuration).SetEase(Ease.OutQuad);
        background.DOFade(0.95f, 1f).SetEase(Ease.Linear);
    }
    public void HidePanel()
    {
        background.DOFade(0f, 0.5f).SetEase(Ease.Linear);
        panelRectTransform.DOAnchorPos(hiddenPanelPosition, transitionDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => panelRectTransform.gameObject.SetActive(false));
    }
}
