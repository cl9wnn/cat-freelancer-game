using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NavigationButton : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite _buttonActiveSprite;
    [SerializeField] private Sprite _buttonInactiveSprite;

    [Header("State")]
    [SerializeField] private bool _active;

    [Header("SFX")]
    [SerializeField] private AudioSource _audio;

    public Button Button { get; private set; }
    public bool IsActive
    {
        get => _active;
        set
        {
            _active = value;
            SetSprite();
        }
    }

    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(_audio.Play);

        IsActive = _active;
    }

    private void SetSprite()
    {
        Button.image.sprite = _active ? _buttonActiveSprite : _buttonInactiveSprite;
    }
}
