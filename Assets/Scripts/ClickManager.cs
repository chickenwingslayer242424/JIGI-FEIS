using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public bool isMoving;
    public Transform player;
    public PolygonCollider2D floorCollider; // Referenz zum PolygonCollider des Bodens
    GameManager gameManager;
    private Vector3 previousPosition;
    private bool facingRight = true;
    private Camera mainCamera;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        previousPosition = player.position;
        mainCamera = Camera.main;

        // Initialisiere den floorCollider
        if (floorCollider == null)
        {
            floorCollider = GameObject.FindGameObjectWithTag("Ground").GetComponent<PolygonCollider2D>();
            if (floorCollider == null)
            {
                Debug.LogError("PolygonCollider2D not found on the floor object!");
            }
        }
    }

    private void Update()
    {
        HandleMovement();
        CheckPlayerDirection();
    }

    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Vector3 targetPosition = GetMouseWorldPosition();
            if (floorCollider.OverlapPoint(targetPosition))
            {
                isMoving = true;
                StartCoroutine(MoveToPosition(targetPosition));
            }
            else
            {
                Vector2 closestPoint = FindClosestPointOnPolygon(targetPosition);
                isMoving = true;
                StartCoroutine(MoveToPosition(closestPoint));
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private Vector2 FindClosestPointOnPolygon(Vector3 targetPosition)
    {
        Vector2 closestPoint = floorCollider.ClosestPoint(targetPosition);
        return closestPoint;
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while ((targetPosition - player.position).sqrMagnitude > 0.01f)
        {
            player.position = Vector3.MoveTowards(player.position, targetPosition, 5f * Time.deltaTime);
            yield return null;
        }
        player.position = targetPosition;
        isMoving = false;
    }

    private void CheckPlayerDirection()
    {
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
}
