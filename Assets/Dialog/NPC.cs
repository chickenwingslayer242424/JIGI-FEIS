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
                dialogLines = new string[] { "Ich brauche das Item Erdbeere, bevor ich dir weiterhelfen kann." }; // Setze die Dialogzeilen des NPCs
                pcDialogLines = new string[] { "halts maul" }; // Setze die Dialogzeilen des PCs
            }
        }
        else
        {
            Debug.Log("NPC hat das benötigte Item bereits erhalten oder benötigt kein Item"); // Debug-Ausgabe für den Zustand des NPCs
            dialogLines = new string[] { "Danke, dass du mir das gebracht hast!", "jetzt kann ich Mario Kart spielen und Smoothie trinken.", "Yippiiiiiiiiiiiiiii." }; // Setze die Dialogzeilen des NPCs
            pcDialogLines = new string[] { "Heißt du giang? oder was!" }; // Setze die Dialogzeilen des PCs
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
