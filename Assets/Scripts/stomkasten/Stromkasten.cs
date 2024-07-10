using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameHandler : MonoBehaviour
{
    public GameObject miniGameCanvas; // Referenz auf das Minispiel-Canvas
    public List<GameObject> draggableObjects; // Liste der draggable Objekte
    public List<GameObject> targetButtons; // Liste der Ziel-Buttons

    private Vector2 difference = Vector2.zero;
    private bool isDragging = false;
    private GameObject selectedObject;

    public static bool isMiniGameActive { get; private set; } // Eigenschaft, die den Zustand des Minispiels speichert

    void Update()
    {
        // Abbrechen, wenn das Minispiel aktiv ist
        if (isMiniGameActive) return;

        // Erfassen der Mausposition
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Setze z auf 0, da es ein 2D-Spiel ist

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button pressed");

            // Überprüfen, ob die linke Maustaste gedrückt wurde und ein Objekt getroffen wurde
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Hit collider: " + hit.collider.gameObject.name);

                if (draggableObjects.Contains(hit.collider.gameObject))
                {
                    Debug.Log("Draggable object hit");
                    isDragging = true;
                    selectedObject = hit.collider.gameObject;
                    difference = (Vector2)mousePosition - (Vector2)selectedObject.transform.position;
                }
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Debug.Log("Dragging object");
            // Aktualisieren der Position des ausgewählten Objekts
            selectedObject.transform.position = (Vector2)mousePosition - difference;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Debug.Log("Left mouse button released");
            isDragging = false;
            selectedObject = null;

            // Überprüfen, ob auf einem gültigen Ziel abgelegt wurde
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && targetButtons.Contains(hit.collider.gameObject))
            {
                Debug.Log("Dropped on target button");
                // Benachrichtigen des GameManagers
                GameManager.Instance.OnDropOnTarget();
            }
        }
    }

    // Methode zum Starten des Minispiels
    public void StartMiniGame()
    {
        isMiniGameActive = true;
        miniGameCanvas.SetActive(true);
    }

    // Methode zum Beenden des Minispiels
    public void ExitMiniGame()
    {
        if (miniGameCanvas != null)
        {
            miniGameCanvas.SetActive(false);
            isMiniGameActive = false; // Setzt das Minispiel auf inaktiv
            GameManager.Instance.CloseMiniGame();
        }
    }
}