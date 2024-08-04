using Plugins.Audio.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NavigationButton : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite _buttonActiveSprite;
    [SerializeField] private Sprite _buttonInactiveSprite;

    public Button Button { get; private set; }
    
    private bool _active;
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

        Button.onClick.AddListener(GameSingleton.Instance.SoundManager.CreateSound()
                                                                      .WithSoundData(SoundEffect.CLICK_SHOP_BUTTON)
                                                                      .Play);
        IsActive = false;
    }

    private void SetSprite()
    {
        Button.image.sprite = _active ? _buttonActiveSprite : _buttonInactiveSprite;
    }
}
