using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CoinButton : Button
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