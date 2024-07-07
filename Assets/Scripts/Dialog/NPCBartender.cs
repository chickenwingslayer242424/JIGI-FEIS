using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string[] initialDialogLines;
    public string[] playerQuestions1;
    public string[] npcResponses1;

    public string[] playerQuestions2;
    public string[] npcResponses2;
    public string[] playerQuestions3;
    public string[] npcResponses3;

    public int requiredItemID; // Verwende Item-ID anstelle des Items
    public ItemData rewardItem;

    public bool hasReceivedItem = false;
    public Sprite npcSprite;
    public Sprite pcImage;
    public string npcName;
    public GameObject objectToSpawn;
    private bool hasSpokenToPlayer = false;
    public ItemData requestedItem;




    public virtual void Interact()
    {
        Debug.Log("Interact aufgerufen");
        
        if (!hasSpokenToPlayer)
        {
            initialDialogLines = new string[] { "You look very stressed.", "Let me guess, you're looking for someone?" }; //spielt diesesn dialog nur 1x ab
            playerQuestions1 = new string[] { "Yes, I am looking for my love. Did you see them?" };
            npcResponses1 = new string[] { "You should grab one of my very special drinks. They will guide you the right way." };
            hasSpokenToPlayer = true;
        }
        else if (!hasReceivedItem && requiredItemID != 0) 
        {
            GameManager gameManager = GameManager.Instance;
            Debug.Log("Required Item ID: " + requiredItemID + ", Selected Item ID: " + (gameManager.selectedItem != null ? gameManager.selectedItem.itemID.ToString() : "null"));
            
           if (gameManager.selectedItemID == 99) //checkt ob das item ausgewählt wurde, wenn ja spielt folgendes ab //funktioniert
            {
                Debug.Log("Required item is selected.");
                // gameManager.RemoveCollectedItem(requiredItemID); // Entferne oder kommentiere diesen Aufruf
                hasReceivedItem = true;
                
                //GameManager.collectedItems.Add(rewardItem); das item soll auf der bar sein, kann aber erst benutz werden, wenn die schlange gegeben wurde

                initialDialogLines = new string[] { "Wow, du super Hecht", "Hier mein super Shaker" };
                playerQuestions1 = new string[] { "Ich bin eine Frau..." };
                npcResponses1 = new string[] { "..Abstand" };
                
                
                
            }




            else //dialog spielt ab, wenn man kein item hat und schon mit ihm geredet hat
            {
                Debug.Log("Required item is not selected.");
                initialDialogLines = new string[] { "O nourrrrrrr, I can’t do this anymore.", "My poor snakey Bertha…", "...she’s gone...", "I can’t find her.", "If someone brings Bertha back to me, I would let them use my special cocktail shaker." };
                playerQuestions1 = new string[] { "Hmmmmm" };
                npcResponses1 = new string[] { "She always escapes me when she sees food....." };
            }
        }
        
        Debug.Log("NPC Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
        FindObjectOfType<DialogManager>().StartDialog(this);
    }

    public virtual void OnItemReceived()
    {
        Debug.Log("Item erhalten!");
    }

    public virtual void OnDialogEnd()
    {
        Debug.Log("Dialog beendet");
    }
}