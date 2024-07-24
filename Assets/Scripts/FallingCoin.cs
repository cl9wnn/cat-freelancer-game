using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class FallingCoin : MonoBehaviour
{
    [Tooltip("Время, через которое объект будет уничтожен.")]
    [SerializeField] private float lifetime = 8f;

    [Tooltip("Продолжительность анимации падения.")]
    [SerializeField] private float fallDuration = 8f;

    [Tooltip("Расстояние падения объекта.")]
    [SerializeField] private float fallPositionY = -5.5f;

    private float _speed;
    private Vector2 _initPosition;
    private float _distance;

    private Game _game;
    private SpawnDown _spawnDown;
    private RectTransform _rectTransform;

    public float Speed
    {
        get => _speed;
        set
        {
            fallDuration = _distance / value;
        
            _speed = value;
        }
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

        var targetPosition = _initPosition + new Vector2(0, -5.5f);
        _distance = Vector2.Distance(_initPosition, targetPosition);
        _initPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    void Start()
    {
        _game = GameSingleton.Instance.Game;
        _spawnDown = GameSingleton.Instance.SpawnDown;
        _rectTransform = GetComponent<RectTransform>();

        if (_rectTransform != null)
        {
            _rectTransform.DOMoveY(fallPositionY, fallDuration).SetEase(Ease.Linear)
                .OnKill(() => Destroy(gameObject));
        }
        else
        {
            Debug.LogError("RectTransform component is missing.");
        }
    }

    public void OnClick()
    {
        if (_game != null)
        {
            _game.Clicks++;
        }

        if (_spawnDown != null)
        {
            _spawnDown.CaughtCoin.Play();
        }

        if (_rectTransform != null)
        {
            _rectTransform.DOKill();
        }

        Destroy(gameObject);
    }
}
