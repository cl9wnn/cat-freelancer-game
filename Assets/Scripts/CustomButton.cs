using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CustomButton : Button
{
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (IsPressed())
        {
            onClick.Invoke();
        }

        base.OnPointerUp(eventData);
    }
}