using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    static float moveSpeed = 3.5f, moveAccuracy = 0.15f; // Wenn frames geskipt werden, wird dieser mit moveaccuracy korrigiert
    public static List<ItemData> collectedItems = new List<ItemData>();  // Wurde von Int auf ItemData umgestellt, wenn es zu Problemen führt zurück
    public RectTransform nameTag, hintBox; // Rect transform weil es das einzige ist was wir brauchen und nutzen

    public Image blockingImage; // Szene wird blockiert und es kann nichts angeklickt werden
    public GameObject[] localScenes;
    public int activeLocalScene = 0; // Sicherstellen, dass dies öffentlich ist
    public Transform[] playerStartPos;

    public GameObject equipmentCanvas;
    public Image[] equipmentSlot, equipmentImages;
    public Sprite emtyItemSlotSprite;
    public Color selectedItemColor;
    public int selectedCanvasSlotID = 0, selectedItemID;

    public CameraFollow cameraFollow; // Hinzufügen einer Referenz zu CameraFollow

    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)  // myObject ist der Spieler // point ist der gespeicherte Punkt vom Objekt
    {
        Vector2 positionDifference = point - (Vector2)myObject.position;
        while (positionDifference.magnitude > moveAccuracy)
        {
            myObject.Translate(moveSpeed * positionDifference.normalized * Time.deltaTime);
            positionDifference = point - (Vector2)myObject.position;
            yield return null; // Warte einen Frame
        }

        myObject.position = point;
        if (myObject == FindObjectOfType<ClickManager>().player) // myObject ist Spieler
        {
            FindObjectOfType<ClickManager>().isMoving = false;
        }
        yield return null;
    }

    public void SelectItem(int equipmentCanvasID)
    {
        Color c = Color.white;
        c.a = 0;
        equipmentSlot[selectedCanvasSlotID].color = c;
        // Speichert Änderungen und hört auf wenn keine Items vorhanden sind oder das letzte Item entfernt wurde
        if (equipmentCanvasID >= collectedItems.Count || equipmentCanvasID < 0)
        {
            // Keine Items ausgewählt
            selectedItemID = -1;
            selectedCanvasSlotID = 0;
            return;
        }

        equipmentSlot[equipmentCanvasID].color = selectedItemColor; // Später die Farbe setzen

        // Änderungen der IDs werden hier gespeichert
        selectedCanvasSlotID = equipmentCanvasID;
        selectedItemID = collectedItems[selectedCanvasSlotID].itemID;
    }

    public void ShowItemName(int equipmentCanvasID)
    {

    }

    public void UpdateEquipmentCanvas()
    {
        // Wie viele Items wir haben und wann wir stoppen
        int itemsAmount = collectedItems.Count, itemSlotAmount = equipmentSlot.Length;
        for (int i = 0; i < itemSlotAmount; i++)
        {
            // Wählt zwischen 2 Items aus eine ohne Bild und das andere mit Bild
            if (i < itemsAmount && collectedItems[i].itemSlotSprite != null)
            {
                equipmentImages[i].sprite = collectedItems[i].itemSlotSprite;
            }
            else
            {
                equipmentImages[i].sprite = emtyItemSlotSprite;
            }
        }
        if (itemsAmount == 0)
        {
            SelectItem(-1);
        }
        else if (itemsAmount == 1)
        {
            SelectItem(0);
        }
    }

    public void UpdateNameTag(ItemData item)
    {
        nameTag.GetComponentInChildren<TextMeshProUGUI>().text = item.objectName;
        nameTag.sizeDelta = item.nameTagSize;
        nameTag.localPosition = new Vector2(item.nameTagSize.x, -0.5f);
    }

    public void UpdateHintBox(ItemData item)
    {
        if (item == null)
        {
            hintBox.gameObject.SetActive(false);
            return;
        }
        hintBox.gameObject.SetActive(true);

        hintBox.GetComponentInChildren<TextMeshProUGUI>().text = item.hintMessage;
        hintBox.sizeDelta = item.hintBoxSize;
        hintBox.localPosition = new Vector2(item.nameTagSize.x, -0.5f); // Erstmal stehen lassen für flipped kommt in Zukunft
    }

    public void CheckSpecialConditions(ItemData item)
    {
        // Wenn Item ID == etwas, gehe zu Szene 1
        switch (item.itemID)
        {
            case -11:
                // Gehe zu Szene 1
                StartCoroutine(ChangeScene(0, 0));
                break;
            case -12:
                // Von der Bar zur Hochzeitskapelle
                StartCoroutine(ChangeScene(1, 0));
                break;
            case -13:
                // Von der Bar zum VIP Club 
                StartCoroutine(ChangeScene(2, 0));
                break;
            case -14:
                // Vom VIP Club zur Bar 
                StartCoroutine(ChangeScene(2, 0));
                break;
            case -32:
                // Spiel gewinnen
                StartCoroutine(ChangeScene(3, 1));
                break;
        }
    }

    public IEnumerator ChangeScene(int sceneNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        blockingImage.enabled = true;
        Color c = blockingImage.color;
        // Szene wird geblockt und es kann nichts geklickt werden
        while (blockingImage.color.a < 1)
        {
            yield return null;
            c.a += Time.deltaTime; // Szene wird in einer Sekunde schwarz
            blockingImage.color = c;
        }
        // Alte Szene verstecken
        localScenes[activeLocalScene].SetActive(false);
        // Neue Szene zeigen
        localScenes[sceneNumber].SetActive(true);
        // Aktuelle Szene speichern
        activeLocalScene = sceneNumber;
        // Spielerposition zurücksetzen
        Transform playerTransform = FindObjectOfType<ClickManager>().player;
        playerTransform.position = playerStartPos[sceneNumber].position;

        // Kamera zurücksetzen
        if (cameraFollow != null)
        {
            cameraFollow.ResetCameraPosition();
        }

        UpdateHintBox(null);

        // Neue Szene wird gezeigt, klicken wird wieder aktiviert
        while (blockingImage.color.a > 0)
        {
            yield return null;
            c.a -= Time.deltaTime;
            blockingImage.color = c;
        }
        blockingImage.enabled = false;
        yield return null;
    }
}
