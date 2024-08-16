using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;


public class VolumeSettings : MonoBehaviour, ISaveLoad
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _volumeSlider;

    [SerializeField] private Image _switchButtonImage;

    private const string MIXER_VOLUME = "Master";
    private const string MIXER_MUSIC = "MusicVolume";

    public Sprite[] onSprites; // Массив спрайтов включения для каждого языка
    public Sprite[] offSprites; // Массив спрайтов выключения для каждого языка
    private int currentLanguageIndex; // Текущий индекс языка

    private float _value;
    private bool _mute;
    public bool Mute
    {
        get => _mute;
        set
        {
            _mute = value;
            _mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(_mute ? _value = 0.0001f : _value = 1.0f) * 20);
            UpdateButtonSprite();

        }
    }
    private void UpdateButtonSprite()
    {
        _switchButtonImage.sprite = _mute ? offSprites[currentLanguageIndex] : onSprites[currentLanguageIndex];
    }

    public void UpdateLanguageSprites(int languageIndex)
    {
        currentLanguageIndex = languageIndex;
        UpdateButtonSprite();
    }



    public void Awake()
    {
        _volumeSlider.onValueChanged.AddListener(SetVolume);
        _switchButtonImage.GetComponent<Button>().onClick.AddListener(() => Mute = !Mute);

        if (YandexGame.SDKEnabled)
            Load();
    }

    public void Save()
    {
        ref var data = ref YandexGame.savesData.volumeData;
        
        if (data == null)
        {
            data = new VolumeData(_value);
            return;
        }

        data.maserVolume = _value;
    }
    public void Load()
    {
        var data = YandexGame.savesData.volumeData;

        float value;

        if (data == null)
            value = 1.0f;
        else
            value = data.maserVolume;
        
        _value = value;
        _volumeSlider.SetValueWithoutNotify(value);
        _mixer.SetFloat(MIXER_VOLUME, Mathf.Log10(value) * 20);
    }

    public void SetVolume(float value)
    {
        _value = value;
        _mixer.SetFloat(MIXER_VOLUME, Mathf.Log10(value) * 20);
        
        Save();      
    }
}