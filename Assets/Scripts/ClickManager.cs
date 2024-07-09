using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public bool isMoving;
    public Transform player;
    private GameManager gameManager;
    private Vector3 previousPosition;
    private bool facingRight = true;
    private const int steckerItemID = 123; // ID für das Item "Stecker"
    private const int stromkastenItemID = 666999;
    public static bool DrinkItem1 = false;
    public static bool DrinkItem2 = false;
    public GameObject KnockDrink1;// Hinzugefügt, um den ausgewählten Artikel zu speichern
    public GameObject KnockDrink2;
    public GameObject DeKnockDrink1;// Hinzugefügt, um den ausgewählten Artikel zu speichern
    public GameObject DeKnockDrink2;
    public GameObject DeKnockDrink3;
    public static bool check1 = true;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        previousPosition = player.position;
    }

    // Dialog
    public void InteractWithNPC(NPC npc)
    {
        if (MiniGameHandler.isMiniGameActive) return; // Abbrechen, wenn das Minispiel aktiv ist
        npc.Interact();
        Debug.Log("Interacting with NPC: " + npc.name);
    }

    private void Update()
    {
        CheckForDrink();
        // Abbrechen, wenn der Dialog oder das Minispiel aktiv ist
        if (DialogManager.isDialogActive || MiniGameHandler.isMiniGameActive) return;
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
            if (MiniGameHandler.isMiniGameActive) return; // Abbrechen, wenn das Minispiel aktiv ist
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Minispiel Trigger"))
                {
                    ItemData item = hit.collider.GetComponent<ItemData>();
                    if (item != null && item.itemID == 666999)
                    {
                        gameManager.StartMiniGame(); // Minispiel starten
                    }
                }
                else if (hit.collider.CompareTag("Ground"))
                {
                    GoToGround(hit.point);
                }
            }
        }

        // Prüfen, ob auf einen NPC geklickt wurde
        CheckForNPCInteraction();
    }

    public void CheckForDrink()
    {
        if (DrinkItem1 && DrinkItem2 && check1) // wenn beide drinks gegebn wurde,
        {
            KnockDrink1.SetActive(true); // in zukunft in eine foreach schleiche packen wenn mehrere items aktiviert werden!
            KnockDrink2.SetActive(true);
            DeKnockDrink1.SetActive(false); // in zukunft in eine foreach schleiche packen wenn mehrere items aktiviert werden!
            DeKnockDrink2.SetActive(false);
            DeKnockDrink3.SetActive(false);
            check1 = false;// damit es nur 1x abspielt
        }
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
        // Abbrechen, wenn der Dialog oder das Minispiel aktiv ist
        if (DialogManager.isDialogActive || MiniGameHandler.isMiniGameActive) return;
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
        if (gameManager.selectedItemID == item.requiredItemID && gameManager.selectedItemID == 16 && !DrinkItem1) //checkt ob rattenreste übergeben wurde wenn ja wird es gespeichert
        {
            Debug.Log("Zutat1 wurde gegeben");
            DrinkItem1 = true;
            gameManager.RemoveItemWhenUsed(item);
            return;
        }
        if (gameManager.selectedItemID == item.requiredItemID2 && gameManager.selectedItemID == 17 && !DrinkItem2) //checkt ob zigartten übergeben wurde wenn ja wird es gespeichert
        {
            Debug.Log("Zutat2 wurde gegeben");
            DrinkItem2 = true;
            gameManager.RemoveItemWhenUsed(item);
            return;
        }

        bool canGetItem = item.requiredItemID == -1 || gameManager.selectedItemID == item.requiredItemID;
        if (canGetItem && item.itemID != steckerItemID && item.itemID != stromkastenItemID) // Hier wird das Item mit der ID 123 nicht gesammelt
        {
            GameManager.collectedItems.Add(item); // Item zur Liste der gesammelten Items hinzufügen
            Debug.Log("Item Collected");
        }

        if (item.itemID == steckerItemID && !GameManager.hasSpokenToCasinoDealer)
        {
            Debug.Log("Du musst zuerst mit dem Casino-Dealer sprechen.");
            return; // Unterbricht die Methode, wenn die Bedingung nicht erfüllt ist
        }

        if (item.itemID == steckerItemID)
        {
            GameManager.isOmaDefeated = true; // Setze die Variable, dass die Oma besiegt wurde
            Debug.Log("Oma wurde besiegt");
        }
        gameManager.RemoveItemWhenUsed(item);

        StartCoroutine(UpdateSceneAfterAction(item, canGetItem));
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
            gameManager.UpdateEquipmentCanvas();// Aktualisiert das Ausrüstungs-Canvas
        }
        else
        {
            Debug.Log("kein item wurde benutzt");
            gameManager.UpdateHintBox(item); // Aktualisiert die Hinweiskiste mit dem Item
            gameManager.CheckSpecialConditions(item); // Überprüft spezielle Bedingungen des Items
        }
    }

    public void GoToGround(Vector3 point)
    {
        if (MiniGameHandler.isMiniGameActive) return; // Abbrechen, wenn das Minispiel aktiv ist
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
            if (DialogManager.isDialogActive || MiniGameHandler.isMiniGameActive) return; // Abbrechen, wenn der Dialog oder das Minispiel aktiv ist
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
