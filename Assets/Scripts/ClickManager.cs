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

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        previousPosition = player.position;
    }

    //Dialog
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
        //Dialog
        if (DialogManager.isDialogActive) return;
        if (!isMoving)
        {
            gameManager.UpdateHintBox(null);
            isMoving = true;
            StartCoroutine(gameManager.MoveToPoint(player, item.goToPoint.position));
            TryGettingItem(item);
        }
    }

    private void TryGettingItem(ItemData item)
    {
        bool canGetItem = item.requiredItemID == -1 || gameManager.selectedItemID == item.requiredItemID;
        if (canGetItem)
        {
            GameManager.collectedItems.Add(item);
            Debug.Log("Item Collected");
        }
        StartCoroutine(UpdateSceneAfterAction(item, canGetItem));
    }

    private IEnumerator UpdateSceneAfterAction(ItemData item, bool canGetItem)
    {
        while (isMoving)
            yield return new WaitForSeconds(0.05f);
        if (canGetItem)
        {
            foreach (GameObject g in item.objectsToRemove)
                Destroy(g);
            gameManager.UpdateEquipmentCanvas();
        }
        else
        {
            gameManager.UpdateHintBox(item);
            gameManager.CheckSpecialConditions(item);
        }
        yield return null;
    }

    public void GoToGround(Vector3 point)
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(gameManager.MoveToPoint(player, point));
        }
    }

    //Dialog
    private void CheckForNPCInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (DialogManager.isDialogActive) return;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                NPC npc = hit.collider.GetComponent<NPC>();
                if (npc != null)
                {
                    InteractWithNPC(npc);
                }
            }
        }
    }
}