using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : NPC
{
    public override void Interact()
    {
        Debug.Log("Keyspawner Interact aufgerufen");
        Debug.Log("Überprüfe, ob das Item ausgewählt ist");
        GameManager gameManager = GameManager.Instance;
        Debug.Log("Required Item ID: " + requiredItemID + ", Selected Item ID: " + (gameManager.selectedItem != null ? gameManager.selectedItem.itemID.ToString() : "null"));
            
        if (gameManager.selectedItemID == 5) //checkt ob das item ausgewählt wurde, wenn ja spielt folgendes ab //funktioniert
        {
            initialDialogLines = new string[] { "Thats a good drink. Than-" };
            Debug.Log("Item 19 ist ausgewählt");
        }
        else 
        { 
            initialDialogLines = new string[] { "Well, well, what do we have here?", "Another lost soul looking for something 'special'?", "I can show you my 'special' moves tonight." };
            playerQuestions1 = new string[] { "Ehm..", "I am looking for my love.", "Did you see them?" };
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