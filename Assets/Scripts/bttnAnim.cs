
using UnityEngine;
using UnityEngine.UI;
public class bttnAnim : MonoBehaviour
{

    public Image[] buttons; // Массив изображений кнопок
    public Sprite[] pushedSprites; // Массив спрайтов для активированных кнопок
    public Sprite[] notPushedSprites; // Массив спрайтов для выключенных кнопок
    public Image shopImg;
    private bool[] isPushed; // Массив состояний кнопок

    void Start()
    {
        // Инициализация массива состояний
        isPushed = new bool[buttons.Length];

        // Первая кнопка всегда включена
        isPushed[0] = true;
        buttons[0].sprite = pushedSprites[0];

        // Остальные кнопки выключены
        for (int i = 1; i < isPushed.Length; i++)
        {
            isPushed[i] = false;
            buttons[i].sprite = notPushedSprites[i];
        }
    }

    public void OpenButton(int buttonIndex)
    {
        // Инвертирование состояния кнопки
        isPushed[buttonIndex] = !isPushed[buttonIndex];

        // Установка спрайта в зависимости от состояния кнопки
        buttons[buttonIndex].sprite = isPushed[buttonIndex] ? pushedSprites[buttonIndex] : notPushedSprites[buttonIndex];

        // Сброс состояния других кнопок
        for (int i = 0; i < isPushed.Length; i++)
        {
            if (i != buttonIndex)
            {
                isPushed[i] = false;
                buttons[i].sprite = notPushedSprites[i];
            }
        }

        // Если кнопка была закрыта, установить спрайт активированной первой кнопки
        if (!isPushed[buttonIndex])
        {
            isPushed[0] = true;
            buttons[0].sprite = pushedSprites[0];
        }
    }
}
