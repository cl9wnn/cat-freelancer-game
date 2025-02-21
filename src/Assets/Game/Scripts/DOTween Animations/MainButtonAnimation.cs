using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainButtonAnimation : MonoBehaviour
{
    private Button _mainClickObject;
 
    private Vector3 _initialScale;

    void Start()
    {
        _initialScale = transform.localScale;

        _mainClickObject = GetComponent<Button>();

        _mainClickObject.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _mainClickObject.gameObject.transform.DOScale(_initialScale * 1.02f, 0.1f)
            .OnComplete(() => _mainClickObject.gameObject.transform.DOScale(_initialScale, 0.1f));
    }
}
