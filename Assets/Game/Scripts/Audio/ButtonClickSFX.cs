using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSFX : MonoBehaviour
{
    [SerializeField] private SoundEffect soundEffect;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayButtonSound);
    }

    private void PlayButtonSound()
    {
        GameSingleton.Instance.SoundManager.CreateSound()
                                           .WithSoundData(soundEffect)
                                           .Play();
    }
}
