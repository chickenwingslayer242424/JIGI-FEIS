using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float moveSpeed = 3.5f; // Bewegungsgeschwindigkeit des Spielers
    public float moveAccuracy = 0.15f; // Genauigkeit der Bewegung
    public static List<ItemData> collectedItems = new List<ItemData>(); // Liste der gesammelten Items
    public RectTransform nameTag, hintBox; // UI-Elemente für Namensschilder und Hinweisbox
    public Image blockingImage; // Bild zum Blockieren des Bildschirms (z.B. für Übergänge)
    public GameObject[] localScenes; // Array der lokalen Szenen
    int activeLocalScene = 0; // Index der aktiven lokalen Szene
    public Transform[] playerStartPos; // Startpositionen des Spielers in den Szenen
    public GameObject equipmentCanvas; // UI-Canvas für die Ausrüstung
    public Image[] equipmentSlot, equipmentImages; // UI-Elemente für die Ausrüstungsslots und -bilder
    public Sprite emtyItemSlotSprite; // Sprite für leere Itemslots
    public Color selectedItemColor; // Farbe für das ausgewählte Item
    public int selectedCanvasSlotID = 0, selectedItemID; // IDs für den ausgewählten Slot und das ausgewählte Item
    public CameraFollow cameraFollow; // Referenz auf das CameraFollow-Skript
    public static bool hasSpokenToCasinoDealer = false; // Flag, ob mit dem Casino-Dealer gesprochen wurde
    public static bool isOmaDefeated = false; // Flag, ob die Oma besiegt wurde
    public static GameManager Instance; // Singleton-Instanz des GameManagers
    public ItemData selectedItem; // Das aktuell ausgewählte Item
     public GameObject ActiveInvObjectA;// Hinzugefügt, um den ausgewählten Artikel zu speichern
    public GameObject ActiveInvObjectB;

    void Update()
    {
        // Desired number to check for
        int desiredNumber = 99;

        // Check for desired number in collectedItems
        foreach (ItemData item in collectedItems)
        {
            if (item.itemID == desiredNumber)
            {

                if (ActiveInvObjectA != null)
                {
                    ActiveInvObjectA.SetActive(true);
                }

                // Check if ActiveInvObjectB is not null before setting it active
                if (ActiveInvObjectB != null)
                {
                    ActiveInvObjectB.SetActive(true);
                }


            }
        }
    }



    private void Awake()
    {
        if (Instance == null) // Überprüfe, ob die Singleton-Instanz null ist
        {
            Instance = this; // Setze die Singleton-Instanz auf diese Instanz
            DontDestroyOnLoad(gameObject); // Verhindere, dass der GameManager beim Szenenwechsel zerstört wird
        }
        else
        {
            Destroy(gameObject); // Zerstöre Duplikate des GameManagers
        }
    }

    public void SelectItem(int equipmentCanvasID)
    {
        Color c = Color.white; // Setze die Farbe auf weiß
        c.a = 0; // Setze die Alpha-Komponente auf 0
        equipmentSlot[selectedCanvasSlotID].color = c; // Setze die Farbe des zuvor ausgewählten Slots

        if (equipmentCanvasID >= collectedItems.Count || equipmentCanvasID < 0) // Überprüfe, ob die übergebene ID gültig ist
        {
            selectedItemID = -1; // Setze die ausgewählte Item-ID auf -1 (kein Item)
            selectedCanvasSlotID = 0; // Setze die ausgewählte Slot-ID auf 0
            return; // Beende die Methode
        }

        equipmentSlot[equipmentCanvasID].color = selectedItemColor; // Setze die Farbe des neuen ausgewählten Slots
        selectedCanvasSlotID = equipmentCanvasID; // Aktualisiere die ausgewählte Slot-ID
        selectedItemID = collectedItems[selectedCanvasSlotID].itemID; // Aktualisiere die ausgewählte Item-ID
        selectedItem = collectedItems[selectedCanvasSlotID]; // Speichere das ausgewählte Item
    }

    public bool IsSelectedItem(int itemID)
    {
        return selectedItem != null && selectedItem.itemID == itemID; // Überprüfe, ob das ausgewählte Item die übergebene Item-ID hat
    }

    // Hier ist die fehlende Methode hinzugefügt
    public void RemoveCollectedItem(int itemID)
    {
        ItemData itemToRemove = collectedItems.Find(item => item.itemID == itemID);
        if (itemToRemove != null)
        {
            collectedItems.Remove(itemToRemove);
            UpdateEquipmentCanvas();
        }
    }

    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)
    {
        Vector2 positionDifference = point - (Vector2)myObject.position; // Berechne den Unterschied zwischen Ziel- und aktueller Position
        while (positionDifference.magnitude > moveAccuracy) // Schleife, bis die Position genau genug ist
        {
            myObject.Translate(moveSpeed * positionDifference.normalized * Time.deltaTime); // Bewege das Objekt in Richtung des Ziels
            positionDifference = point - (Vector2)myObject.position; // Aktualisiere den Positionsunterschied
            yield return null; // Warte einen Frame
        }

        myObject.position = point; // Setze die Position des Objekts genau auf den Zielpunkt
        if (myObject == FindObjectOfType<ClickManager>().player) // Überprüfe, ob das bewegte Objekt der Spieler ist
        {
            FindObjectOfType<ClickManager>().isMoving = false; // Setze das Bewegungs-Flag auf false
        }
        yield return null; // Warte einen Frame
    }

    public void ShowItemName(int equipmentCanvasID)
    {
        // Implementiere diese Methode, falls erforderlich
    }

    public void UpdateEquipmentCanvas()
    {
        int itemsAmount = collectedItems.Count, itemSlotAmount = equipmentSlot.Length; // Anzahl der Items und Slots
        for (int i = 0; i < itemSlotAmount; i++) // Schleife über alle Slots
        {
            if (i < itemsAmount && collectedItems[i].itemSlotSprite != null) // Überprüfe, ob es ein Item mit einem Sprite gibt
            {
                equipmentImages[i].sprite = collectedItems[i].itemSlotSprite; // Setze das Sprite des Items im Slot
            }
            else
            {
                equipmentImages[i].sprite = emtyItemSlotSprite; // Setze das Sprite für einen leeren Slot
            }
        }

        if (itemsAmount == 0) // Überprüfe, ob keine Items vorhanden sind
        {
            SelectItem(-1); // Wähle kein Item aus
        }
        else if (itemsAmount == 1) // Überprüfe, ob genau ein Item vorhanden ist
        {
            SelectItem(0); // Wähle das erste Item aus
        }
    }

    public void UpdateNameTag(ItemData item)
    {
        nameTag.GetComponentInChildren<TextMeshProUGUI>().text = item.objectName; // Aktualisiere den Namen im Namensschild
        nameTag.sizeDelta = item.nameTagSize; // Setze die Größe des Namensschildes
        nameTag.localPosition = new Vector2(item.nameTagSize.x, -0.5f); // Setze die Position des Namensschildes
    }

    public void UpdateHintBox(ItemData item)
    {
        if (item == null) // Überprüfe, ob kein Item übergeben wurde
        {
            hintBox.gameObject.SetActive(false); // Verstecke die Hinweisbox
            return; // Beende die Methode
        }
        hintBox.gameObject.SetActive(true); // Zeige die Hinweisbox an
        hintBox.GetComponentInChildren<TextMeshProUGUI>().text = item.hintMessage; // Setze die Nachricht in der Hinweisbox
        hintBox.sizeDelta = item.hintBoxSize; // Setze die Größe der Hinweisbox
        hintBox.localPosition = new Vector2(item.nameTagSize.x, -0.5f); // Setze die Position der Hinweisbox
    }

    public void CheckSpecialConditions(ItemData item)
    {
        switch (item.itemID) // Überprüfe die Item-ID und führe spezielle Aktionen aus
        {
            case -11:
                StartCoroutine(ChangeScene(localScenes[0], 0)); // Szene wechseln
                break;
            case -12:
                StartCoroutine(ChangeScene(localScenes[1], 0)); // Szene wechseln
                break;
            case -13:
                StartCoroutine(ChangeScene(localScenes[2], 0)); // Szene wechseln
                break;
            case -32:
                StartCoroutine(ChangeScene(localScenes[3], 1)); // Szene wechseln
                break;
        }
    }

      public IEnumerator ChangeScene(GameObject newScene, float delay)
    {
        yield return new WaitForSeconds(delay);
        blockingImage.enabled = true;
        Color c = blockingImage.color;
        while (blockingImage.color.a < 1)
        {
            yield return null;
            c.a += Time.deltaTime;
            blockingImage.color = c;
        }

        localScenes[activeLocalScene].SetActive(false);
        newScene.SetActive(true);
        activeLocalScene = System.Array.IndexOf(localScenes, newScene);

        if (cameraFollow != null)
        {
            cameraFollow.enabled = true;
        }

        FindObjectOfType<ClickManager>().player.position = playerStartPos[activeLocalScene].position;
        UpdateHintBox(null);

        // 等待摄像机位置刷新
        yield return new WaitForEndOfFrame();

        // 启用或禁用 CameraFollow 脚本
        if (cameraFollow != null)
        {
            cameraFollow.enabled = (newScene.name != "Scene2");
        }

        FindObjectOfType<ClickManager>().player.position = playerStartPos[activeLocalScene].position;
        UpdateHintBox(null);

        while (blockingImage.color.a > 0)
        {
            yield return null;
            c.a -= Time.deltaTime;
            blockingImage.color = c;
        }
        blockingImage.enabled = false;
        yield return null;
    }

     // Methode zum Sammeln der Maus
    public void CollectMouse(Mover mouseMover)
    {
        Debug.Log("Mouse collected!");
        ItemData mouseItem = mouseMover.GetComponent<ItemData>();
        if (mouseItem != null)
        {
            if (!collectedItems.Contains(mouseItem)) // Überprüfen, ob die Maus bereits gesammelt wurde
            {
                collectedItems.Add(mouseItem);
                UpdateEquipmentCanvas();
                mouseItem.HideItem(); // Maus ausblenden
            }
            else
            {
                Debug.LogWarning("Mouse already collected!"); // Warnung, falls die Maus bereits gesammelt wurde
            }
        }
     }
}

