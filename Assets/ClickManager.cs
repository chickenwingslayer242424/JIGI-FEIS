using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public bool isMoving;
    public Transform player;
    GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void GoToItem(ItemData item) //in item hitbox platziert und dann, die hitbox reinziehen
    {   
        if (!isMoving)
        {
           //update hintbox
        gameManager.UpdateHintBox(null);
        //start moving player
        StartCoroutine(gameManager.MoveToPoint(player, item.goToPoint.position)); //gameManager spricht die class "GameManager" an um MoveToPoint zu holen
         isMoving = true; 
        TryGettingItem(item); //wird sofot in die liste gepackt, weil
       
        //wird dann abgespielt, wenn der spieler sich nicht mehr bewegt, heißt isMoving = false  
        
        }
       
    }




    private void TryGettingItem(ItemData item)
    {
        bool canGetItem = item.requiredItemID == -1 || gameManager.selectedItemID == item.requiredItemID;//überprüft ob das ITEM eine requiredItemID zugewiesen bekommen hat, in dem fall -1, wenn nicht, dann kann das Item nicht aufgehoben werden
        if (canGetItem)
        {

            GameManager.collectedItems.Add(item); // item wird aufgehoben, checkt was für eine "itemID" das item hat, dann wird das in die liste eingespeichert
            Debug.Log("Item Collected");

        }
        StartCoroutine(UpdateSceneAfterAction(item, canGetItem)); //wenn das item aufgehoben wurde, werden die items zerstört
    }
    private IEnumerator UpdateSceneAfterAction(ItemData item, bool canGetItem) //ist eine Couroutine, diese alleine macht noch nichts
    {
        while (isMoving)
            yield return new WaitForSeconds(0.05f); //macht nichts, bis es "isMoving" am ziel ankommt
        if (canGetItem)
        {   
            foreach (GameObject g in item.objectsToRemove) //geht jedes item in der liste durch, wenn das item gefunden wurde, wird es zerstört
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
