using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button.ButtonClickedEvent onClick;

    private bool isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed)
        {
            onClick.Invoke();
        }
        isPressed = false;
    }
}