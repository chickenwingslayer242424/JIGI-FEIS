using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 difference = Vector2.zero;
    public GameObject targetButton; // Referenz auf das Ziel-UI-Element

    public void OnPointerDown(PointerEventData eventData)
    {
        difference = (Vector2)transform.position - (Vector2)eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector2)eventData.position + difference;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(targetButton.GetComponent<RectTransform>(), Input.mousePosition))
        {
            // Benachrichtigen des GameManagers
            GameManager.Instance.OnDropOnTarget();
        }
    }
}
