using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameEventAnimation : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private float _colorAlpha = 1.0f;

    [Header("Game Event")]
    [SerializeField] private Image _eventImage;
    [SerializeField] private Text _eventText;
    [SerializeField] private Button _takeButton;
    [SerializeField] private Text _takeText;

    [Header("Coin")]
    [SerializeField] private Image _coinImage;
    [SerializeField] private Text _currentCoinsText;
    [SerializeField] private Text _earnedCoinsText;
    [SerializeField] private float _buttonsAndCoinsFadeDuration = 1.5f;
    
    [Header("Hide Duration")]
    [SerializeField] private float _hideDuration = 0.5f;
    
    private void Awake()
    {
        _backgroundImage.gameObject.SetActive(false);
        _takeButton.image.enabled = false;
        _takeButton.interactable = false;

        _takeButton.onClick.AddListener(GameSingleton.Instance.SoundManager.CreateSound().WithSoundData(SoundEffect.CLICK_SHOP_BUTTON).Play);
    }

    public void ShowPanel()
    {
        Sequence animationSequence = DOTween.Sequence();

        animationSequence.Append(AnimateBackground())
                         .Append(AnimateEvent())
                         .Append(AnimateButtonsAndCoins());
    }

    public void HidePanel()
    {
        Sequence hideSequence = DOTween.Sequence();

        hideSequence.AppendInterval(0.5f);

        hideSequence.Append(_currentCoinsText.DOFade(0, _hideDuration));
        hideSequence.Join(_earnedCoinsText.DOFade(0, _hideDuration));
        hideSequence.Join(_takeText.DOFade(0, _hideDuration));

        hideSequence.Append(_takeButton.transform.DOScale(Vector3.zero, _hideDuration).SetEase(Ease.InBack));
        hideSequence.Join(_coinImage.transform.DOScale(Vector3.zero, _hideDuration).SetEase(Ease.InBack));

        hideSequence.Append(_eventText.DOFade(0, _hideDuration));
        hideSequence.Join(_eventImage.transform.DOScale(Vector3.zero, _hideDuration).SetEase(Ease.InBack));

        hideSequence.Append(_backgroundImage.DOFade(0, _hideDuration));

        hideSequence.OnComplete(() =>
        {
            _takeButton.image.enabled = false;
            _backgroundImage.gameObject.SetActive(false);
        });
    }

    private Sequence AnimateBackground()
    {
        _backgroundImage.gameObject.SetActive(true);
        _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 0);

        return DOTween.Sequence()
            .Append(_backgroundImage.DOFade(_colorAlpha, _fadeDuration))
            .SetEase(Ease.InOutQuad);
    }

    private Sequence AnimateEvent()
    {
        _eventImage.color = new Color(_eventImage.color.r, _eventImage.color.g, _eventImage.color.b, 0);
        _eventText.color = new Color(_eventText.color.r, _eventText.color.g, _eventText.color.b, 0);
        return DOTween.Sequence()
            .Append(_eventImage.DOFade(1, _fadeDuration))
            .Join(_eventText.DOFade(1, _fadeDuration))
            .SetEase(Ease.InOutQuad);
    }

    private Sequence AnimateButtonsAndCoins()
    {
        _takeButton.image.enabled = true;
        _takeButton.interactable = true;

        _takeButton.image.color = new Color(_takeButton.image.color.r, _takeButton.image.color.g, _takeButton.image.color.b, 0);
        _takeText.color = new Color(_takeText.color.r, _takeText.color.g, _takeText.color.b, 0);
        _coinImage.color = new Color(_coinImage.color.r, _coinImage.color.g, _coinImage.color.b, 0);
        _currentCoinsText.color = new Color(_currentCoinsText.color.r, _currentCoinsText.color.g, _currentCoinsText.color.b, 0);
        _earnedCoinsText.color = new Color(_earnedCoinsText.color.r, _earnedCoinsText.color.g, _earnedCoinsText.color.b, 0);

        return DOTween.Sequence()
            .Append(_takeText.DOFade(1, _buttonsAndCoinsFadeDuration))
            .Join(_coinImage.DOFade(1, _buttonsAndCoinsFadeDuration))
            .Join(_currentCoinsText.DOFade(1, _buttonsAndCoinsFadeDuration))
            .Join(_earnedCoinsText.DOFade(1, _buttonsAndCoinsFadeDuration))
            .SetEase(Ease.InOutQuad);
    }
}
