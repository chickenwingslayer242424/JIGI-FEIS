using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string[] dialogLines; // Array der Dialogzeilen für den NPC
    public ItemData requiredItem; // Das benötigte Item, um den Dialog zu starten
    public ItemData rewardItem; // Das Item, das der NPC nach Erfüllung der Bedingung gibt

    private bool hasReceivedItem = false; // Gibt an, ob der NPC das benötigte Item erhalten hat
    public Sprite npcSprite; // Das Bild des NPCs
    public Sprite pcImage; // Das Bild des Player Characters (PC)
    public string[] pcDialogLines; // Array der Dialogzeilen für den Player Character (PC)

    // Neues Feld für den NPC-Namen
    public string npcName;

    public void Interact()
    {
        Debug.Log("Interact aufgerufen"); // Debug-Ausgabe für Interaktion

        if (!hasReceivedItem && requiredItem != null) // Wenn der NPC das benötigte Item nicht erhalten hat und ein Item benötigt wird
        {
            if (GameManager.collectedItems.Contains(requiredItem)) // Wenn das benötigte Item gesammelt wurde
            {
                GameManager.collectedItems.Remove(requiredItem); // Entferne das Item aus der Sammlung
                hasReceivedItem = true; // Setze den Zustand auf erhalten

                GameManager.collectedItems.Add(rewardItem); // Füge das Belohnungs-Item zur Sammlung hinzu

                dialogLines = new string[] { "Wow du super hecht", "hier mein super shaker" }; // Setze die Dialogzeilen des NPCs
                pcDialogLines = new string[] { "ich bin eine Frau...", "..Abstand" }; // Setze die Dialogzeilen des PCs
            }
            else
            {
                dialogLines = new string[] { "O nourrrrrrr, I can’t do this anymore.", " My poor snakey Bertha…", "...she’s gone...", "I can’t find her.",  "If someone brings Bertha back to me, I would let them use my special cocktail shaker." }; // Setze die Dialogzeilen des NPCs
                pcDialogLines = new string[] { "hmmmmm" }; // Setze die Dialogzeilen des PCs
            }
        }
        else
        {
            Debug.Log("NPC hat das benötigte Item bereits erhalten oder benötigt kein Item"); // Debug-Ausgabe für den Zustand des NPCs
            dialogLines = new string[] { "Oh my, you scared me Bertha!!Where were you?", "Thank you, total stranger, that I’ve never seen before....", " Here, now you can make special drinks just like me. You can’t take it with you, just bring me 2 ingredients and I can mix it up for ya."  }; // Setze die Dialogzeilen des NPCs
            pcDialogLines = new string[] { "Do you know, how to get the key to the wedding chapel? " }; // Setze die Dialogzeilen des PCs
            dialogLines = new string[] { "Well, I knaurr that the chapel keys are kept by the security in the entrance." };
        }

        Debug.Log("NPC Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null")); // Debug-Ausgabe für das NPC-Bild

        FindObjectOfType<DialogManager>().StartDialog(this); // Starte den Dialog mit dem DialogManager
    }

    public void OnDialogEnd()
    {
        // Zusätzliche Logik nach dem Dialog (falls nötig)
        Debug.Log("Dialog beendet"); // Debug-Ausgabe für das Dialogende
    }
}
