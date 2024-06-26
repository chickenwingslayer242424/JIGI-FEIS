using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public bool isMoving;
    public Transform player;
    GameManager gameManager;
    private Vector3 previousPosition; // Speichert die vorherige Position des Spielers
    private bool facingRight = true; // Speichert die aktuelle Richtung, in die der Spieler schaut

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        previousPosition = player.position; // Initialisiert die vorherige Position
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 mousePosWorld2D = new Vector2(mousePosWorld.x, mousePosWorld.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePosWorld2D, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                // Spieler hat auf den Boden geklickt, beginnen Sie mit der Bewegung
                isMoving = true;
                StartCoroutine(gameManager.MoveToPoint(player, hit.point));
            }
        }

        // Überprüfen, ob der Spieler sich bewegt hat
        if (isMoving)
        {
            Vector3 currentPosition = player.position; // Aktuelle Position des Spielers
            if (currentPosition.x > previousPosition.x && !facingRight)
            {
                // Spieler bewegt sich nach rechts und schaut nach links
                Flip();
            }
            else if (currentPosition.x < previousPosition.x && facingRight)
            {
                // Spieler bewegt sich nach links und schaut nach rechts
                Flip();
            }
            previousPosition = currentPosition; // Aktualisiert die vorherige Position
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = player.localScale;
        scale.x *= -1; // Flips the player horizontally
        player.localScale = scale;
    }

    public void GoToItem(ItemData item) // in item hitbox platziert und dann, die hitbox reinziehen
    {
        if (!isMoving)
        {
            // update hintbox
            gameManager.UpdateHintBox(null);
            // start moving player
            isMoving = true;  // so eine scheiße, bitte hier lassen!!!
            StartCoroutine(gameManager.MoveToPoint(player, item.goToPoint.position)); // gameManager spricht die class "GameManager" an um MoveToPoint zu holen

            TryGettingItem(item); // wird sofort in die liste gepackt, weil
            // wird dann abgespielt, wenn der spieler sich nicht mehr bewegt, heißt isMoving = false  
        }
    }

    private void TryGettingItem(ItemData item)
    {
        bool canGetItem = item.requiredItemID == -1 || gameManager.selectedItemID == item.requiredItemID; // überprüft ob das ITEM eine requiredItemID zugewiesen bekommen hat, in dem fall -1, wenn nicht, dann kann das Item nicht aufgehoben werden
        if (canGetItem)
        {
            GameManager.collectedItems.Add(item); // item wird aufgehoben, checkt was für eine "itemID" das item hat, dann wird das in die liste eingespeichert
            Debug.Log("Item Collected");
        }
        StartCoroutine(UpdateSceneAfterAction(item, canGetItem)); // wenn das item aufgehoben wurde, werden die items zerstört
    }

    private IEnumerator UpdateSceneAfterAction(ItemData item, bool canGetItem) // ist eine Couroutine, diese alleine macht noch nichts
    {
        while (isMoving)
            yield return new WaitForSeconds(0.05f); // macht nichts, bis es "isMoving" am ziel ankommt
        if (canGetItem)
        {
            foreach (GameObject g in item.objectsToRemove) // geht jedes item in der liste durch, wenn das item gefunden wurde, wird es zerstört
                Destroy(g);
            gameManager.UpdateEquipmentCanvas();
        }
        else
        {
            gameManager.UpdateHintBox(item); // wenn das item nicht aufgehoben werden kann, wird eine hintbox displayed die dem spieler sagt was er braucht
            gameManager.CheckSpecialConditions(item);
        }

        yield return null;
    }
}
