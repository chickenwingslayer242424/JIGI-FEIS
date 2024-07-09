using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 difference = Vector2.zero;

    public void OnPointerDown(PointerEventData eventData)
    {
        difference = (Vector2)transform.position - (Vector2)eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector2)eventData.position + difference;
    }
}
