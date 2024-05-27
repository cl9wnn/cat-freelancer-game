using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class BttnAnimation : MonoBehaviour
{
    private Button button;
    private Vector3 originalScale;

    private void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;

        if (button != null)
        {
            button.onClick.AddListener(AnimateButton);
        }
    }

    private void AnimateButton()
    {
        StartCoroutine(AnimateButtonCoroutine());
    }

    private IEnumerator AnimateButtonCoroutine()
    {
        // Уменьшение кнопки
        Vector3 targetScale = originalScale * 0.98f;
        float animationTime = 0.12f; // Длительность анимации

        float elapsedTime = 0f;
        while (elapsedTime < animationTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        // Восстановление размера кнопки
        yield return new WaitForSeconds(0.1f); // Пауза перед восстановлением размера
        elapsedTime = 0f;
        while (elapsedTime < animationTime)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
    

