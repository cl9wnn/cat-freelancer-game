using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TabletAnimation : MonoBehaviour
{
    [SerializeField] private Image _background;

    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private Button _smallTabletButton;
    [SerializeField] private Vector3 _targetScale = new Vector3(1.5f, 1.5f, 1.5f);

    private RectTransform tablet;
    private Vector3 _initPosition;

    private Color _initSmallTabletColor;

    private void Awake()
    {
        tablet = GetComponent<RectTransform>();

        if (_smallTabletButton == null) return;

        _initPosition = tablet.position;
        _initSmallTabletColor = _smallTabletButton.image.color;

        _smallTabletButton.interactable = true;
    }

    private void Start()
    {
        _smallTabletButton?.onClick.AddListener(Show);
        DisablePanel();
    }

    public void Show()
    {
        EnablePanel();
        tablet.localScale = Vector3.zero;
        tablet.position = _smallTabletButton.transform.position;

        _smallTabletButton.interactable = false;
        _smallTabletButton.image.color = Color.black;

        Vector3 targetScreenPosition = new Vector3(Screen.width / 3, Screen.height / 2, 0);

        Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(targetScreenPosition);
        targetWorldPosition.z = tablet.position.z; 

        Sequence sequence = DOTween.Sequence();
        sequence.Append(tablet.DOMove(targetWorldPosition, _duration).SetEase(Ease.OutBack));
        sequence.Join(tablet.DOScale(_targetScale, _duration).SetEase(Ease.OutBack));
    }

    public void Hide()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(tablet.DOScale(Vector3.zero, _duration).SetEase(Ease.InBack));
        sequence.Join(tablet.DOMove(_smallTabletButton.transform.position, _duration).SetEase(Ease.InBack));
        sequence.OnComplete(() =>
        {
            DisablePanel();
            tablet.position = _initPosition;

            _smallTabletButton.interactable = true;
            _smallTabletButton.image.color = _initSmallTabletColor;
        });
    }

    public void DisablePanel()
    {
        _background.enabled = false;
        tablet.GetComponent<Image>().enabled = false;

        foreach (Transform child in tablet.transform)
        {
            child.gameObject.SetActive(false);
        }

    }
    public void EnablePanel()
    {
        _background.enabled = true; 
        tablet.GetComponent<Image>().enabled = true;

        foreach (Transform child in tablet.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
