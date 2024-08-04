using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _volumeSlider;

    [SerializeField] private Image _switchButtonImage;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;

    private const string MIXER_VOLUME = "Master";
    private const string MIXER_MUSIC = "MusicVolume";

    private bool _mute;
    public bool Mute
    {
        get => _mute;
        set
        {
            _mute = value;
            _mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(_mute ? 0.0001f : 1.0f) * 20);
            _switchButtonImage.sprite = _mute ? _offSprite : _onSprite;
        }
    }

    public void Awake()
    {
        _volumeSlider.onValueChanged.AddListener(SetVolume);
        _switchButtonImage.GetComponent<Button>().onClick.AddListener(() => Mute = !Mute);

        if (YandexGame.SDKEnabled)
            Load();
    }
    private void OnEnable()
    {
        SaveManager.OnLoadEvent += Load;
    }
    private void OnDisable()
    {
        SaveManager.OnLoadEvent -= Load;
    }
    
    private void Load()
    {
        var data = YandexGame.savesData.volumeData;

        float value;

        if (data == null)
            value = 1.0f;
        else
            value = data.maserVolume;

        _volumeSlider.SetValueWithoutNotify(value);
        _mixer.SetFloat(MIXER_VOLUME, Mathf.Log10(value) * 20);
    }

    public void SetVolume(float value)
    {
        _mixer.SetFloat(MIXER_VOLUME, Mathf.Log10(value) * 20);

        YandexGame.savesData.volumeData = new VolumeData(value);        
    }
}