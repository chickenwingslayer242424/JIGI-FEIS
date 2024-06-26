using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNPC : NPC
{
    public override void Interact()
    {
        Debug.Log("Interact aufgerufen");

        if (!hasReceivedItem && requiredItem != null)
        {
            if (GameManager.collectedItems.Contains(requiredItem))
            {
                GameManager.collectedItems.Remove(requiredItem);
                hasReceivedItem = true;
                GameManager.collectedItems.Add(rewardItem);

                initialDialogLines = new string[] { "Danke für das Item.", "Hier ist deine Belohnung." };
                playerQuestion = "Was nun?";
                npcResponse = "Geh zur Kapelle.";
            }
            else
            {
                initialDialogLines = new string[] { "Ich benötige ein spezielles Item." };
                playerQuestion = "Welches Item?";
                npcResponse = "Finde es heraus.";
            }
        }
        else
        {
            initialDialogLines = new string[] { "I heard that a beast escaped from the wedding chapel." , "Horrible. It's horrible. I can't go in there myself." };
            playerQuestion= "Hmmmm..maybe I should check it out.";
            npcResponse = "NO!?!";
        }

        Debug.Log("NPC Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));

        FindObjectOfType<DialogManager>().StartDialog(this);
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Dialog beendet");
    }
}
