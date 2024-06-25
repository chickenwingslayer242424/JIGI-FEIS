using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    static float moveSpeed = 3.5f, moveAccuracy = 0.15f; //wenn frames geskipt werden, wird dieser mit moveaccuracy korriegiert
    public static List<ItemData> collectedItems = new List<ItemData>();  //wurde von Int auf ItemData umgestellt, wenn es zu Problemen führt zurück
    public RectTransform nameTag, hintBox; //rect transform weil es das einzige ist was wir brauchen und nutzen

    public Image blockingImage; //Szene wird blockiert und es kann nichts angeklickt werden
    public GameObject[] localScenes;
    public int activeLocalScene = 0; // sicherstellen, dass dies öffentlich ist
    public Transform[] playerStartPos;

    public GameObject equipmentCanvas;
    public Image[] equipmentSlot, equipmentImages;
    public Sprite emtyItemSlotSprite;
    public Color selectedItemColor;
<<<<<<< Updated upstream
   public int selectedCanvasSlotID = 0, selectedItemID;
    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)  //myObject ist der player //point ist der gespeicherte punkt vom object
=======
    public int selectedCanvasSlotID = 0, selectedItemID;
>>>>>>> Stashed changes

    public CameraFollow cameraFollow; // Hinzufügen einer Referenz zu CameraFollow

    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)  //myObject ist der Spieler //point ist der gespeicherte Punkt vom Objekt
    {
        Vector2 positionDifference = point - (Vector2)myObject.position;
        while (positionDifference.magnitude > moveAccuracy)
        {
            myObject.Translate(moveSpeed * positionDifference.normalized * Time.deltaTime);
            positionDifference = point - (Vector2)myObject.position;
            yield return null; //warte einen Frame
        }

        myObject.position = point;
        if (myObject == FindObjectOfType<ClickManager>().player) //myObject ist Spieler
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
<<<<<<< Updated upstream
        //speichert änderungen und hört auf wenn keine items vorhanden sind oder das letzte item entfernt wurde
        if (equipmentCanvasID >= collectedItems.Count || equipmentCanvasID < 0) 
=======
        //speichert Änderungen und hört auf wenn keine Items vorhanden sind oder das letzte Item entfernt wurde
        if (equipmentCanvasID >= collectedItems.Count || equipmentCanvasID < 0)
>>>>>>> Stashed changes
        {
            //keine Items ausgewählt
            selectedItemID = -1;
            selectedCanvasSlotID = 0;
            return;
        }

        equipmentSlot[equipmentCanvasID].color = selectedItemColor; //später die Farbe setzen

        //Änderungen der IDs werden hier gespeichert
        selectedCanvasSlotID = equipmentCanvasID;
        selectedItemID = collectedItems[selectedCanvasSlotID].itemID;
<<<<<<< Updated upstream
        

=======
>>>>>>> Stashed changes
    }

    public void ShowItemName(int equipmentCanvasID)
    {

    }

    public void UpdateEquipmentCanvas()
    {
        //wie viele Items wir haben und wann wir stoppen
        int itemsAmount = collectedItems.Count, itemSlotAmount = equipmentSlot.Length;
        for (int i = 0; i < itemSlotAmount; i++)
        {
<<<<<<< Updated upstream
            //wählt zwischen 2 items aus eine ohne Image und das andere mit Image
            if (i < itemsAmount &&collectedItems[i].itemSlotSprite != null)
=======
            //wählt zwischen 2 Items aus eine ohne Bild und das andere mit Bild
            if (i < itemsAmount && collectedItems[i].itemSlotSprite != null)
>>>>>>> Stashed changes
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
        hintBox.localPosition = new Vector2(item.nameTagSize.x, -0.5f); //erstmal stehen lassen für flipped kommt in Zukunft
    }

    public void CheckSpecialConditions(ItemData item)
    {
        //wenn Item ID == etwas, gehe zu Szene 1
        switch (item.itemID)
        {
            case -11:
                //gehe zu Szene 1
                StartCoroutine(ChangeScene(0, 0));
                break;
            case -12:
<<<<<<< Updated upstream
                //go to scene 2
                StartCoroutine(ChangeScene(1, 0));
                break;
            case -32:
                //win game
                StartCoroutine(ChangeScene(2, 1));
=======
                //von der Bar zur Hochzeitskapelle
                StartCoroutine(ChangeScene(1, 0));
                break;
            case -13:
                //von der Bar zum VIP Club 
                StartCoroutine(ChangeScene(2, 0));
                break;
            case -14:
                //vom VIP Club zur Bar 
                StartCoroutine(ChangeScene(2, 0));
                break;
            case -32:
                //Spiel gewinnen
                StartCoroutine(ChangeScene(3, 1));
>>>>>>> Stashed changes
                break;
        }
    }

    public IEnumerator ChangeScene(int sceneNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        blockingImage.enabled = true;
        Color c = blockingImage.color;
        //Szene wird geblockt und es kann nichts geklickt werden
        while (blockingImage.color.a < 1)
        {
            yield return null;
            c.a += Time.deltaTime; //Szene wird in einer Sekunde schwarz
            blockingImage.color = c;
        }
        //alte Szene verstecken
        localScenes[activeLocalScene].SetActive(false);
        //neue Szene zeigen
        localScenes[sceneNumber].SetActive(true);
        //aktuelle Szene speichern
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

        // neue Szene wird gezeigt, klicken wird wieder aktiviert
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
