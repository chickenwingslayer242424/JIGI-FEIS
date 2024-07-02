using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string[] initialDialogLines; // Array der Dialogzeilen für den NPC
    public string playerQuestion; // Dialogzeile für die Frage des Spielers
    public string npcResponse; // Antwort des NPCs auf die Frage des Spielers
    public ItemData requiredItem; // Das benötigte Item, um den Dialog zu starten
    public ItemData rewardItem; // Das Item, das der NPC nach Erfüllung der Bedingung gibt

    protected bool hasReceivedItem = false; // Ändere von private zu protected
    public Sprite npcSprite; // Das Bild des NPCs
    public Sprite pcImage; // Das Bild des Player Characters (PC)
    public string npcName; // Neues Feld für den NPC-Namen

    public GameObject objectToSpawn; // Das Objekt, das gespawnt werden soll
    private bool hasSpokenToPlayer = false; // Variable, um zu verfolgen, ob der NPC bereits mit dem Spieler gesprochen hat

    public virtual void Interact() // Die Methode als virtual markieren
    {
        Debug.Log("Interact aufgerufen"); // Debug-Ausgabe für Interaktion

        if (!hasSpokenToPlayer)
        {
            // Spezielle Begrüßung beim ersten Gespräch
            initialDialogLines = new string[] { "You look very stressed.", "Let me guess, you're looking for someone?" };
            playerQuestion = "Yes, I am looking for my love. Did you see them?";
            npcResponse = "You should grab one of my very special drinks. They will guide you the right way.";
            hasSpokenToPlayer = true;
        }
        else if (!hasReceivedItem && requiredItem != null) // Wenn der NPC das benötigte Item nicht erhalten hat und ein Item benötigt wird
        {
            if (GameManager.collectedItems.Contains(requiredItem)) // Wenn das benötigte Item gesammelt wurde
            {
                GameManager.collectedItems.Remove(requiredItem); // Entferne das Item aus der Sammlung
                hasReceivedItem = true; // Setze den Zustand auf erhalten

                GameManager.collectedItems.Add(rewardItem); // Füge das Belohnungs-Item zur Sammlung hinzu
                Debug.Log("Belohnungs-Item hinzugefügt: " + rewardItem.name); // Debug-Ausgabe für das Belohnungs-Item

                initialDialogLines = new string[] { "Wow, du super Hecht!", "Hier mein super Shaker." }; // Setze die Dialogzeilen des NPCs
                playerQuestion = "Ich bin eine Frau...";
                npcResponse = "..Abstand";

                // Aktiviere das Objekt
                ActivateObjectToSpawn();
            }
            else
            {
                initialDialogLines = new string[] { "Oh no, I can’t do this anymore.", "My poor snakey Bertha…", "...she’s gone...", "I can’t find her.", "If someone brings Bertha back to me, I would let them use my special cocktail shaker." }; // Setze die Dialogzeilen des NPCs
                playerQuestion = "Hmmmmm";
                npcResponse = "She always escapes me when she sees food.....";
            }
        }
        else
        {
            Debug.Log("NPC hat das benötigte Item bereits erhalten oder benötigt kein Item"); // Debug-Ausgabe für den Zustand des NPCs
            initialDialogLines = new string[] { "Oh my, you scared me Bertha!! Where were you?", "Thank you, total stranger, that I’ve never seen before....", "Here, now you can make special drinks just like me. You can’t take it with you, just bring me 2 ingredients and I can mix it up for ya." }; // Setze die Dialogzeilen des NPCs
            playerQuestion = "Do you know, how to get the key to the wedding chapel?";
            npcResponse = "Well, I know that the chapel keys are kept by the security in the entrance.";
        }

        Debug.Log("NPC Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null")); // Debug-Ausgabe für das NPC-Bild

        FindObjectOfType<DialogManager>().StartDialog(this); // Starte den Dialog mit dem DialogManager
    }

public virtual void OnDialogEnd() // Die Methode als virtual markieren
{
    // Zusätzliche Logik nach dem Dialog (falls nötig)
    Debug.Log("Dialog beendet"); // Debug-Ausgabe für das Dialogende
    
    // Aktiviere das Objekt, wenn es noch nicht aktiviert wurde
    if (!hasReceivedItem && requiredItem != null && objectToSpawn != null)
    {
        ActivateObjectToSpawn();
    }
}

private void ActivateObjectToSpawn()
{
    if (objectToSpawn != null)
    {
        objectToSpawn.SetActive(true);
        Debug.Log("Objekt aktiviert: " + objectToSpawn.name);
    }
    else
    {
        Debug.Log("Kein Objekt zum Aktivieren gefunden.");
    }
}
}
