using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource AudioClick;
    public AudioSource AudioSpin;
    public AudioSource AudioSwap;
    public AudioSource AudioCollect;
    public AudioSource AudioOpenWindow;
    public AudioSource AudioOnButton;
    public AudioSource AudioOnMouseClick;

    public void ClickAudio()
    {
        AudioClick.Play();
    }
    public void ClickSpin()
    {
       AudioSpin.Play();
    }
    public void Swap()
    {
      AudioSwap.Play();
    }
    public void Collect()
    {
        AudioCollect.Play();
    }
    public void OpenWindow()
    {
        AudioOpenWindow.Play();
    }

    public void OnButtonOpenScene()
    {
        AudioOnButton.Play();
    }
    public void OnMouseClick()
    {
        AudioOnMouseClick.Play();
    }
}
