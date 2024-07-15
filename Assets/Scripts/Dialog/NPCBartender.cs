using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
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
    public GameObject Shaker1;
    public GameObject Shaker2;
    public GameObject KnockDrink1;
    public GameObject KnockDrink2;
    public Transform goToPoint; // Neuer Go To Point für den NPC
    // Neue Variable für die Blickrichtung
    public bool flipPlayerSpriteRight;



    public virtual void Interact()
    {
        Debug.Log("Interact aufgerufen");

        if (!hasSpokenToPlayer)
        {
            initialDialogLines = new string[] { "You look very stressed.", "Let me guess, you're looking for someone?" }; //spielt diesesn dialog nur 1x ab
            playerQuestions1 = new string[] { "Yes, I am looking for my love. Did you see them?" };
            npcResponses1 = new string[] { "You should grab one of my very special drinks. They will guide you the right way." };
            hasSpokenToPlayer = true;
            KnockDrink1.SetActive(true);
            KnockDrink2.SetActive(true);
        }
        else if (!hasReceivedItem && requiredItemID != 0)
        {
            GameManager gameManager = GameManager.Instance;
            ItemData item = ItemData.Instance;
            Debug.Log("Required Item ID: " + requiredItemID + ", Selected Item ID: " + (gameManager.selectedItem != null ? gameManager.selectedItem.itemID.ToString() : "null"));

            if (gameManager.selectedItemID == 99) //checkt ob das item ausgewählt wurde, wenn ja spielt folgendes ab //funktioniert
            {
                gameManager.RemoveItemForNPC();
                Debug.Log("Required item is selected.");
                Shaker1.SetActive(true);
                Shaker2.SetActive(true);
                //Shaker aktivieren

                // gameManager.RemoveCollectedItem(requiredItemID); // Entferne oder kommentiere diesen Aufruf
                hasReceivedItem = true;


                initialDialogLines = new string[] { "Oh my, you scared me, Bertha!", "Where were you?","Thank you, total stranger I've never seen before.", "Here, now you can make special drinks just like me.", "You can't take it with you, just put in two ingredients and I can mix it up for ya." };
                playerQuestions1 = new string[] { "Do you know how to get the key to the wedding chapel?" };
                npcResponses1 = new string[] { "Well, I know that the chapel keys are kept by a gang member at the VIP-Club." };


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