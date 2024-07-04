using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public bool isMoving;
    public Transform player;
    private GameManager gameManager;
    private Vector3 previousPosition;
    private bool facingRight = true;
    private const int steckerItemID = 123; // ID für das Item "Stecker"

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        previousPosition = player.position;
    }

    // Dialog
    public void InteractWithNPC(NPC npc)
    {
        npc.Interact();
        Debug.Log("Interacting with NPC: " + npc.name);
    }

    private void Update()
    {
        // Abbrechen, wenn der Dialog aktiv ist
        if (DialogManager.isDialogActive) return;
        if (isMoving)
        {
            Vector3 currentPosition = player.position;
            if (currentPosition.x > previousPosition.x && !facingRight)
            {
                Flip();
            }
            else if (currentPosition.x < previousPosition.x && facingRight)
            {
                Flip();
            }
            previousPosition = currentPosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                GoToGround(hit.point);
            }
        }

        // Prüfen, ob auf einen NPC geklickt wurde
        CheckForNPCInteraction();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = player.localScale;
        scale.x *= -1;
        player.localScale = scale;
    }

    public void GoToItem(ItemData item)
    {
        // Dialog
        if (DialogManager.isDialogActive) return; // Unterbricht, wenn ein Dialog aktiv ist
        if (!isMoving) // Wenn der Spieler sich nicht bewegt
        {
            gameManager.UpdateHintBox(null); // Aktualisiert die Hinweiskiste
            isMoving = true; // Setzt den Bewegungsstatus auf wahr
            StartCoroutine(MoveAndTryGettingItem(item)); // Startet die Coroutine zum Bewegen und Holen des Items
        }
    }

    private IEnumerator MoveAndTryGettingItem(ItemData item)
    {
        yield return StartCoroutine(gameManager.MoveToPoint(player, item.goToPoint.position)); // Bewegt den Spieler zum Zielpunkt des Items
        TryGettingItem(item); // Versucht, das Item zu holen
        isMoving = false; // Setzt den Bewegungsstatus auf falsch
    }

    public void TryGettingItem(ItemData item)
    {
        if (item.itemID == steckerItemID && !GameManager.hasSpokenToCasinoDealer)
        {
            Debug.Log("Du musst zuerst mit dem Casino-Dealer sprechen.");
            return; // Unterbricht die Methode, wenn die Bedingung nicht erfüllt ist
        }

        bool canGetItem = item.requiredItemID == -1 || gameManager.selectedItemID == item.requiredItemID;
        if (canGetItem)
        {
            GameManager.collectedItems.Add(item); // Item zur Liste der gesammelten Items hinzufügen
            Debug.Log("Item Collected");
        }

        StartCoroutine(UpdateSceneAfterAction(item, canGetItem));

        if (item.itemID == steckerItemID)
        {
            GameManager.isOmaDefeated = true; // Setze die Variable, dass die Oma besiegt wurde
            Debug.Log("Oma wurde besiegt");
        }
    }

    private IEnumerator UpdateSceneAfterAction(ItemData item, bool canGetItem)
    {
        while (isMoving) // Solange sich der Spieler bewegt
            yield return new WaitForSeconds(0.05f); // Warte 0,05 Sekunden

        if (canGetItem) // Wenn das Item geholt werden kann
        {
            foreach (GameObject obj in item.objectsToRemove) // Für jedes zu entfernende Objekt des Items
            {
                obj.SetActive(false); // Setzt das GameObject auf inaktiv
                Destroy(obj); // Entfernt das GameObject
            }
            gameManager.UpdateEquipmentCanvas(); // Aktualisiert das Ausrüstungs-Canvas
        }
        else
        {
            gameManager.UpdateHintBox(item); // Aktualisiert die Hinweiskiste mit dem Item
            gameManager.CheckSpecialConditions(item); // Überprüft spezielle Bedingungen des Items
        }
    }

    public void GoToGround(Vector3 point)
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(gameManager.MoveToPoint(player, point));
        }
    }

    // Dialog
    private void CheckForNPCInteraction()
    {
        if (Input.GetMouseButtonDown(0)) // Wenn die linke Maustaste gedrückt wird
        {
            if (DialogManager.isDialogActive) return; // Unterbricht, wenn ein Dialog aktiv ist
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); // Raycast an der Mausposition
            if (hit.collider != null) // Wenn der Raycast etwas trifft
            {
                NPC npc = hit.collider.GetComponent<NPC>(); // Holt das NPC-Component des getroffenen Objekts
                if (npc != null) // Wenn das getroffene Objekt ein NPC ist
                {
                    InteractWithNPC(npc); // Interagiert mit dem NPC
                }

                // Prüfen, ob auf ein Item geklickt wurde
                ItemData item = hit.collider.GetComponent<ItemData>(); // Holt das ItemData-Component des getroffenen Objekts
                if (item != null) // Wenn das getroffene Objekt ein Item ist
                {
                    GoToItem(item); // Gehe zu dem Item
                }
            }
        }
    }
}
