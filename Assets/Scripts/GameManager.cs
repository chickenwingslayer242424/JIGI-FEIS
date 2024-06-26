using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;

public class GameManager : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float moveAccuracy = 0.15f;
    public static List<ItemData> collectedItems = new List<ItemData>();
    public RectTransform nameTag, hintBox;
    public Image blockingImage;
    public GameObject[] localScenes;
    int activeLocalScene = 0;
    public Transform[] playerStartPos;
    public GameObject equipmentCanvas;
    public Image[] equipmentSlot, equipmentImages;
    public Sprite emtyItemSlotSprite;
    public Color selectedItemColor;
    public int selectedCanvasSlotID = 0, selectedItemID;
    public CameraFollow cameraFollow; // 添加对 CameraFollow 的引用

    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)
    {
        Vector2 positionDifference = point - (Vector2)myObject.position;
        while (positionDifference.magnitude > moveAccuracy)
        {
            myObject.Translate(moveSpeed * positionDifference.normalized * Time.deltaTime);
            positionDifference = point - (Vector2)myObject.position;
            yield return null; //wait one frame
        }

        myObject.position = point;
        if (myObject == FindObjectOfType<ClickManager>().player)
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

        if (equipmentCanvasID >= collectedItems.Count || equipmentCanvasID < 0)
        {
            selectedItemID = -1;
            selectedCanvasSlotID = 0;
            return;
        }

        equipmentSlot[equipmentCanvasID].color = selectedItemColor;
        selectedCanvasSlotID = equipmentCanvasID;
        selectedItemID = collectedItems[selectedCanvasSlotID].itemID;
    }

    public void ShowItemName(int equipmentCanvasID)
    {
        // Implement this method if needed
    }

    public void UpdateEquipmentCanvas()
    {
        int itemsAmount = collectedItems.Count, itemSlotAmount = equipmentSlot.Length;
        for (int i = 0; i < itemSlotAmount; i++)
        {
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
        hintBox.localPosition = new Vector2(item.nameTagSize.x, -0.5f);
    }

    public void CheckSpecialConditions(ItemData item)
    {
        switch (item.itemID)
        {
            case -11:
                StartCoroutine(ChangeScene(localScenes[0], 0));
                break;
            case -12:
                StartCoroutine(ChangeScene(localScenes[1], 0));
                break;
            case -13:
                StartCoroutine(ChangeScene(localScenes[2], 0));
                break;
            case -32:
                StartCoroutine(ChangeScene(localScenes[3], 1));
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
}
