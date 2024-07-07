using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public int itemID, requiredItemID; //Item wird in zukunft danach benannt nach nummerierung
    public int requiredItemID2;
    public Transform goToPoint; //pos von gameobjekten wird hier platziert
    public GameObject[] objectsToRemove;
    public GameObject hideUI; //damit der spieler nicht 2x auf denselben UI drückt
    public Sprite itemSlotSprite;
   
    
     //item reinziehen, was nach dem anklicken verschwinden soll
     public string objectName;
     public Vector2 nameTagSize = new Vector2(3,0.65f);
       public string hintMessage;
     public Vector2 hintBoxSize = new Vector2(3,0.65f);
  
    public void HideItem()
    {
        gameObject.SetActive(false);
    }
}