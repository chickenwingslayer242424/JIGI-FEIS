using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : NPC
{
    public GameObject Schlüssel1;
    public GameObject Schlüssel2;
    private GameManager gameManager;
    public bool ReceiveKnock = false;
    private bool hasDroppedKey = false; // Variable hinzugefügt

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

    }

    public override void Interact()
    {
         if (hasDroppedKey) // Überprüfen, ob der Schlüssel bereits gedroppt wurde
        {
            Debug.Log("Dieser NPC ist nicht mehr interaktiv.");
            initialDialogLines = new string[] { "ehhhh" };
            playerQuestions1 = new string[] { "i think hes knocked out good" };
            npcResponses1 = new string[] { "öhhhhh.......hihiihih" };

            FindObjectOfType<DialogManager>().StartDialog(this);
            return;
        }
        Debug.Log("Keyspawner Interact aufgerufen");
        Debug.Log("Überprüfe, ob das Item ausgewählt ist");
        //GameManager gameManager = GameManager.Instance;
        // ItemData item = ItemData.Instance;
        ItemData item = ItemData.Instance;



        Debug.Log("Required Item ID: " + requiredItemID + ", Selected Item ID: " + (gameManager.selectedItem != null ? gameManager.selectedItem.itemID.ToString() : "null"));

        if (gameManager.selectedItemID == 19 || ReceiveKnock) //checkt ob das item ausgewählt wurde, wenn ja spielt folgendes ab //funktioniert
        {
            if (ReceiveKnock == false)
            {
                gameManager.RemoveItemForNPC();
            }

            initialDialogLines = new string[] { "Thats a good drink. Than-" };
            playerQuestions1 = new string[] { "looks like he dropped something." };
            npcResponses1 = new string[] { "öhhhhh........hihiihih" };

            //schlüssel aktivieren
            if (Schlüssel1 != null)
            {
                Schlüssel1.SetActive(true);
            }


            if (Schlüssel2 != null)
            {
                Schlüssel2.SetActive(true);
            }
            ReceiveKnock = true;
            hasDroppedKey = true;

        }
        else
        {
            initialDialogLines = new string[] { "Well, well, what do we have here?", "Another lost soul looking for something 'special'?", "I can show you my 'special' moves tonight." };
            playerQuestions1 = new string[] { "Ehm..ew", "I am looking for my love.", "Did you see them?" };
            npcResponses1 = new string[] { "You must be one of those who think that just because you had one night with the Boss, you can convince him to release his hostages and become a better person.", "Tough luck, the Boss is getting married today.", "And guess what? I've got the keys.", "But keeping an eye out to make sure everything goes smoothly while looking for some flesh for tonight is making me thirsty." };
            Debug.Log("Item 19 ist nicht ausgewählt");
        }

        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("KeySpawner Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Dialog mit KeySpawner beendet");
    }
}