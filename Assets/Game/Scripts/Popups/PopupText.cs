using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening;

public class PopupText : MonoBehaviour
{
    [Header("Popup Text Settings")]
    [SerializeField] private RectTransform _popupTextPrefab;
    [SerializeField] private Canvas _canvas; 
    [SerializeField] private float _animationDuration = 1f; 
    [SerializeField] private float _moveDistance = 50f; 
    [SerializeField] private Vector3 _startScale = new Vector3(1f, 1f, 1f); 
    [SerializeField] private Vector3 _endScale = new Vector3(1.5f, 1.5f, 1.5f); 
    [SerializeField] private float _delayBeforeFade = 0.1f;
    [SerializeField] private float _fadeDuration;

    [SerializeField] private GameObject _particles;
    private Camera Camera;

    private void Awake()
    {
        Camera = Camera.main;
    }
    public void ShowPopupText(string text, Vector3 position, Color textColor)
    {
        RectTransform popupTextInstance = Instantiate(_popupTextPrefab, _canvas.transform);

        var textComponent = popupTextInstance.GetComponent<Text>(); 

        if (textComponent != null)
        {
            textComponent.text = text;
            textComponent.color = textColor;
        }

        popupTextInstance.anchoredPosition = position;
        popupTextInstance.localScale = _startScale;

        float duration = _animationDuration;

        Color startColor = textComponent.color;
        Color endColor = startColor;
        endColor.a = 0f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(popupTextInstance.DOScale(_endScale, duration / 2).SetEase(Ease.Linear));

        sequence.Join(popupTextInstance.DOAnchorPosY(position.y + _moveDistance, duration).SetEase(Ease.Linear));
        sequence.Insert(_delayBeforeFade, textComponent.DOColor(endColor, _fadeDuration));

        sequence.OnComplete(() => Destroy(popupTextInstance.gameObject));

        sequence.Play();
    }

    public void ShowClickPopupText()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 point);

        var hasBoosted = GameSingleton.Instance.Boost.IsBoostActive;

        var text = "+" + StringMethods.FormatMoney(GameSingleton.Instance.Game.ScoreIncrease * (hasBoosted ? 3 : 1));

        var color = hasBoosted ? new Color32(255, 215, 0, 255) : new Color32(255, 255, 255, 255);

        ShowPopupText(text, point, color);

        if (GameSingleton.Instance.Boost.IsBoostActive)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Задаем дистанцию от камеры до точки в мире, где будут спавниться частицы
            Vector3 worldPosition = this.Camera.ScreenToWorldPoint(mousePosition);
            worldPosition.z = -5;
            Destroy(Instantiate(_particles, worldPosition, Quaternion.identity), 2f);
        }
    }
}
