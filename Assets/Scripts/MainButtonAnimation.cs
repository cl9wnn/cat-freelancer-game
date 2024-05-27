using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainButtonAnimation : MonoBehaviour
{
    public GameObject MainClickObj;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    public void OnClick()
    {
        StartCoroutine(ButtonAnimationCoroutine());
    }

    IEnumerator ButtonAnimationCoroutine()
    {
        // Увеличиваем размер кнопки
        MainClickObj.transform.localScale = initialScale * 1.02f;

        
        yield return new WaitForSeconds(0.1f);

        // Возвращаем размер кнопки к исходному
        MainClickObj.transform.localScale = initialScale;
    }
}
