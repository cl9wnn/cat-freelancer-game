using System.Collections;
using UnityEngine;

public class SoundFade : MonoBehaviour
{
    private AudioSource audioSource;
    private float startingVolume;
    private float fadeSpeed = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startingVolume = audioSource.volume;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }
    
}