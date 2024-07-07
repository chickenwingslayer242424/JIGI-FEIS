using UnityEngine;

public class MiniGameHandler : MonoBehaviour
{
    private GameObject selectedObject;
    public GameObject miniGameCanvas; // Referenz auf das Minispiel-Canvas

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.transform.parent == transform)
            {
                if (selectedObject == null)
                {
                    // Objekt auswählen
                    selectedObject = hit.collider.gameObject;
                    selectedObject.transform.SetParent(transform); // Damit es sich relativ zum Canvas bewegt
                }
                else
                {
                    // Objekt ablegen
                    selectedObject.transform.SetParent(null);
                    selectedObject = null;
                }
            }
        }

        if (selectedObject != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10.0f; // Setzen Sie dies auf den gewünschten Z-Wert
            selectedObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }

    // Methode zum Beenden des Minispiels
    public void ExitMiniGame()
    {
        if (miniGameCanvas != null)
        {
            miniGameCanvas.SetActive(false); // Minispiel-Canvas deaktivieren
            GameManager.Instance.EndMiniGame(); // Minispiel im GameManager beenden
        }
    }
}
