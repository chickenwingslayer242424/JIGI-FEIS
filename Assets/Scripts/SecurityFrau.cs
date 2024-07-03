using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityFrau : NPC
{
    public GameObject objectToDeactivate; // Hinzugefügt: Referenz auf das zu deaktivierende GameObject

    public override void Interact()
    {
        Debug.Log("SecurityFrau Interact aufgerufen");

        // Überprüfe, ob der Spieler das benötigte Item hat
        if (GameManager.collectedItems.Contains(requiredItem))
        {
            // Wenn der Spieler das benötigte Item hat, setze die Dialogzeilen entsprechend
            initialDialogLines = new string[] { "Natürlich darfst du durch, Kollege." };
            playerQuestions1 = new string[] { "Danke!" };
            npcResponses1 = new string[] { "Pass auf dich auf." };

            // Deaktiviere das GameObject
            if (objectToDeactivate != null)
            {
                objectToDeactivate.SetActive(false);
                Debug.Log("Object " + objectToDeactivate.name + " wurde deaktiviert.");
            }
        }
        else
        {
            // Wenn der Spieler das benötigte Item nicht hat, setze die Standard-Dialogzeilen
            initialDialogLines = new string[] { "Hallo, ich bin die SecurityFrau.", "Du kommst nicht durch" };
            playerQuestions1 = new string[] { "Ja, ich suche jemanden. Haben Sie sie gesehen?" };
            npcResponses1 = new string[] { "Piss dich, ich habe niemanden gesehen." };
        }

        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("SecurityFrau Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Dialog mit SecurityFrau beendet");
    }
}
