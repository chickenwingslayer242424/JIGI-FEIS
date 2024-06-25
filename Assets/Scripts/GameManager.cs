using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
//game manager in clickmanager reinpacken
public class GameManager : MonoBehaviour
{
    static float moveSpeed = 3.5f, moveAccuracy = 0.15f; //wenn frames geskipt werden, wird dieser mit moveaccuracy korriegiert
    public static List<ItemData> collectedItems = new List<ItemData>();  //wurde von Int auf ItemData umgestellt, wenn es zu problemen führt zurück
    public RectTransform nameTag, hintBox; //rect transform weil es das einzige ist was wir brauchen und nutzen

    public Image blockingImage; //scene wird blockiert un es kann nichts angeklickt werden
    public GameObject[] localScenes;
    int activeLocalScene = 0;
    public Transform[] playerStartPos;

    public GameObject equipmentCanvas;
    public Image[] equipmentSlot, equipmentImages;
    public Sprite emtyItemSlotSprite;
    public Color selectedItemColor;
    public int selectedCanvasSlotID = 0, selectedItemID;
    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)  //myObject ist der player //point ist der gespeicherte punkt vom object

    {


        Vector2 positionDifference = point - (Vector2)myObject.position;
        while (positionDifference.magnitude > moveAccuracy)
        {
            myObject.Translate(moveSpeed * positionDifference.normalized * Time.deltaTime);
            positionDifference = point - (Vector2)myObject.position;
            yield return null; //wait one frame
        }

        myObject.position = point;
        if (myObject == FindObjectOfType<ClickManager>().player)//myObject ist spieler, 
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
        //speichert änderungen und hört auf wenn keine items vorhanden sind oder das letzte item entfernt wurde
        if (equipmentCanvasID >= collectedItems.Count || equipmentCanvasID < 0)
        {
            //keine items selected
            selectedItemID = -1;
            selectedCanvasSlotID = 0;
            return;
        }

        equipmentSlot[equipmentCanvasID].color = selectedItemColor; //später die farbe setzen

        //änderungen der IDs werden hier gespeichert
        selectedCanvasSlotID = equipmentCanvasID;
        selectedItemID = collectedItems[selectedCanvasSlotID].itemID;


    }
    public void ShowItemName(int equipmentCanvasID)
    {

    }
    public void UpdateEquipmentCanvas()
    {
        //wie viele items wir haben und wann wir stoppen
        int itemsAmount = collectedItems.Count, itemSlotAmount = equipmentSlot.Length;
        for (int i = 0; i < itemSlotAmount; i++)
        {
            //wählt zwischen 2 items aus eine ohne Image und das andere mit Image
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
        hintBox.localPosition = new Vector2(item.nameTagSize.x, -0.5f); //erstmal stehen lassen für flipped kommt in zukunft
    }

    public void CheckSpecialConditions(ItemData item)
    {
        //wenn item ID == etwas, gehe zu scene 1
        switch (item.itemID)
        {
            case -11:
                //go to scene 1
                StartCoroutine(ChangeScene(0, 0));
                break;
            case -12:
                //go to scene 2
                StartCoroutine(ChangeScene(1, 0));
                break;
            case -13:
                //go to scene 2
                StartCoroutine(ChangeScene(2, 0));
                break;
            case -32:
                //win game
                StartCoroutine(ChangeScene(3, 1));
                break;
        }

    }
    public IEnumerator ChangeScene(int sceneNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        blockingImage.enabled = true;
        Color c = blockingImage.color;
        //scene wird geblockt und es kann nichts geklickt werden
        while (blockingImage.color.a < 1)
        {
            yield return null;
            c.a += Time.deltaTime; //scene wird in einer sekunde schwarz
            blockingImage.color = c;

        }
        //alte scene verstecken
        localScenes[activeLocalScene].SetActive(false);
        //neue scene zeigen
        localScenes[sceneNumber].SetActive(true);
        //aktuelle scene speichern
        activeLocalScene = sceneNumber;
        //spieler teleportieren
        FindObjectOfType<ClickManager>().player.position = playerStartPos[sceneNumber].position;
        UpdateHintBox(null);



        //neue scene wird gezeigt, klicken wird wieder aktiviert
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
