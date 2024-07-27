using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeBar : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;
    public Slider slider;
    public float volume;
    public void SetVolume(float saveValueOfSlider)
    {
        myAudioMixer.SetFloat("MasterVolume", Mathf.Log10(saveValueOfSlider) * 20);
    }
}