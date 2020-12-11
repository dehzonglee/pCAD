using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsPointerOnCanvas;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        IsPointerOnCanvas = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        IsPointerOnCanvas = false;
    }
}
