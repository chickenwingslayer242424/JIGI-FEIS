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
    public static bool KnockDrink = false;
    public GameObject KnockDrink1;// Hinzugefügt, um den ausgewählten Artikel zu speichern
    public GameObject KnockDrink2;
    public GameObject DeKnockDrink1;// Hinzugefügt, um den ausgewählten Artikel zu speichern
    public GameObject DeKnockDrink2;
    public GameObject DeKnockDrink3;
    public static bool check1 = true;
    public Transform posterGoToPoint; // Hinzugefügt, um den festgelegten Go To Point vom poster zu speichern


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
        // Sprite-Flip basierend auf der Blickrichtung des NPCs
        if (npc.flipPlayerSpriteRight && !facingRight)
        {
            Flip();
        }
        else if (!npc.flipPlayerSpriteRight && facingRight)
        {
            Flip();
        }
    }

    private void Update()
    {
        CheckForDrink();
        // Abbrechen, wenn der Dialog oder das Minispiel aktiv ist
        if (DialogManager.isDialogActive || MiniGameHandler.isMiniGameActive || GameManager.isPopupActive) return;
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
            if (MiniGameHandler.isMiniGameActive || GameManager.isPopupActive || GameManager.isNachdenkenActive) return; // Abbrechen, wenn das Minispiel oder Popup aktiv ist
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
                else if (gameManager.GetLocalScenes()[1].activeSelf && gameManager.objectToHide.activeSelf)
                {
                    // Blockiere andere Klicks in localScene[1], solange objectToHide aktiv ist
                    return;
                }
                else if (hit.collider.CompareTag("Ground"))
                {
                    GoToGround(hit.point);
                }
                else if (hit.collider.CompareTag("poster"))
                {
                    ItemData item = hit.collider.GetComponent<ItemData>();
                    if (item != null && item.itemID == 777456)
                    {
                        GoToPoster(item); // Popup anzeigen
                    }
                }
            }
        }

        // Prüfen, ob auf einen NPC geklickt wurde
        CheckForNPCInteraction();
    }

    private void GoToPoster(ItemData item)
    {
        if (!isMoving && posterGoToPoint != null) // Überprüfen, ob der Spieler sich nicht bewegt und der Go To Point festgelegt ist
        {
            isMoving = true; // Setzt den Bewegungsstatus auf wahr
            StartCoroutine(MoveToPosterAndShowCanvas(item)); // Startet die Coroutine zum Bewegen und Anzeigen des Canvas
        }
    }

    private IEnumerator MoveToPosterAndShowCanvas(ItemData item)
    {
        yield return StartCoroutine(gameManager.MoveToPoint(player, posterGoToPoint.position)); // Bewegt den Spieler zum Go To Point
        isMoving = false; // Setzt den Bewegungsstatus auf falsch
        gameManager.ShowPosterPopup(); // Zeigt das Poster-Canvas an
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
        if (DialogManager.isDialogActive || MiniGameHandler.isMiniGameActive || GameManager.isPopupActive|| GameManager.isNachdenkenActive) return;
        if (!isMoving) // Wenn der Spieler sich nicht bewegt
        {
            gameManager.UpdateHintBox(null); // Aktualisiert die Hinweiskiste
            isMoving = true; // Setzt den Bewegungsstatus auf wahr
            StartCoroutine(MoveAndTryGettingItem(item)); // Startet die Coroutine zum Bewegen und Holen des Items
        }
    }

    private IEnumerator MoveAndTryGettingItem(ItemData item)
    {
        yield return StartCoroutine(gameManager.MoveToPoint(player, item.goToPoint.position)); //das hier nutzen um zum npc zuerst hinlaufen dann interagieren
        TryGettingItem(item); // Versucht, das Item zu holen
        isMoving = false; // Setzt den Bewegungsstatus auf falsch
    }

    public void TryGettingItem(ItemData item)
    {
       if (GameManager.isPopupActive) return; // Abbrechen, wenn ein Popup aktiv ist

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
        if(gameManager.selectedItemID == item.requiredItemID && gameManager.selectedItemID == -5)
        {
            gameManager.CheckForKey(item);
            return; //immer return nach special conditions, sonst versucht das nächste if ein item ins inv zu packen obwohl nichts ist.
        }

        bool canGetItem = item.requiredItemID == -1 || gameManager.selectedItemID == item.requiredItemID;
        if (canGetItem && item.itemID != steckerItemID && item.itemID != stromkastenItemID) // Hier wird das Item mit der ID 123 nicht gesammelt
        {
            GameManager.collectedItems.Add(item); // Item zur Liste der gesammelten Items hinzufügen
            Debug.Log("Item Collected");
        }

        else
            {
                Debug.Log("Item nicht gesammelt");
                if (item.showNachdenkCanvas) // Überprüfe, ob das Nachdenk-Canvas angezeigt werden soll
                    {
                        Debug.Log("Showing nachdenk canvas with message: " + item.nachdenkText);
                        NachdenkManager.Instance.ShowNachdenkCanvas(item.nachdenkText); // Zeigt das Nachdenk-Canvas mit der entsprechenden Nachricht
                    }
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
        if (MiniGameHandler.isMiniGameActive || GameManager.isPopupActive) return; // Abbrechen, wenn das Minispiel oder Popup aktiv ist
        if (!isMoving) // Wenn der Spieler sich nicht bewegt
        {
            gameManager.UpdateHintBox(null); // Aktualisiert die Hinweiskiste
            isMoving = true; // Setzt den Bewegungsstatus auf wahr
            StartCoroutine(MoveToGroundAndStop(point)); // Startet die Coroutine zum Bewegen zum Punkt und Anhalten
        }
    }

    private IEnumerator MoveToGroundAndStop(Vector3 point)
    {
        yield return StartCoroutine(gameManager.MoveToPoint(player, point)); //das hier nutzen um zum npc zuerst hinlaufen dann interagieren
        isMoving = false; // Setzt den Bewegungsstatus auf falsch
    }

     private void CheckForNPCInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("NPC"))
            {
                NPC npc = hit.collider.GetComponent<NPC>();
                if (npc != null)
                {
                    // Bewege den Spieler zum Go To Point des NPCs, bevor interagiert wird
                    if (npc.goToPoint != null)
                    {
                        GoToGround(npc.goToPoint.position);
                        StartCoroutine(WaitAndInteractWithNPC(npc));
                    }
                    else
                    {
                        InteractWithNPC(npc); // Interagiere direkt, wenn kein Go To Point vorhanden ist
                    }
                }
            }
        }
    }

    private IEnumerator WaitAndInteractWithNPC(NPC npc)
    {
        while (isMoving) // Warte, bis der Spieler sich nicht mehr bewegt
        {
            yield return null;
        }
        InteractWithNPC(npc); // Interagiere mit dem NPC, wenn der Spieler angekommen ist
    }
}
