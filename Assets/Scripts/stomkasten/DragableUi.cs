using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 difference = Vector2.zero;
    private Vector2 originalPosition; // Speichert die Ausgangsposition des Objekts
    public GameObject targetButton; // Referenz auf das Ziel-UI-Element

    public void OnPointerDown(PointerEventData eventData)
    {
        originalPosition = transform.position; // Speichert die Ausgangsposition
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
            // Setzt den draggable Button in die Mitte des targetButtons
            transform.position = targetButton.GetComponent<RectTransform>().position;
            
            // Benachrichtigen des GameManagers
            GameManager.Instance.OnDropOnTarget();
        }
        else
        {
            // Zurück zur Ausgangsposition
            Debug.Log("Zurück zur Ausgangsposition");
            transform.position = originalPosition;
        }
    }
}