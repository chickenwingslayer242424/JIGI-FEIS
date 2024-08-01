using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityFrau : NPC
{
    public GameObject objectToDeactivate; // Hinzugefügt: Referenz auf das zu deaktivierende GameObject
    public GameObject objectToActivate;
    public override void Interact()
    {
        Debug.Log("SecurityFrau Interact aufgerufen");
        GameManager gameManager = GameManager.Instance;
        // Überprüfe, ob der Spieler das benötigte Item hat
        if (gameManager.selectedItemID == 456)
        {
            // Wenn der Spieler das benötigte Item hat, setze die Dialogzeilen entsprechend
            gameManager.RemoveItemForNPC();
            initialDialogLines = new string[] { "Thanks sis, finally.", "Sorry for beeing so rough.",  "You know how crowded it can get here sometimes." };
            playerQuestions1 = new string[] { "Have you seen my love?" };
            npcResponses1 = new string[] { "Oh, sweetie.","I haven't seen mine like you for quite some time, and that won't change." };

            // Deaktiviere das GameObject
            if (objectToDeactivate != null)
            {
                objectToDeactivate.SetActive(false);
                objectToActivate.SetActive(true);

            }
        } 
        else
        {
            // Wenn der Spieler das benötigte Item nicht hat, setze die Standard-Dialogzeilen
            initialDialogLines = new string[] { " Nuh-uh- sister!", "You're not allowed in here.", "GEEZ-", "BECAUSE YOU DISTRACTED ME I MADE A HOLE INTO MY UNIFORM!","Now leave before I stab you with my nails.",  "They are long for a reason." };
            playerQuestions1 = new string[] { "Hold on!", "I can get you a new uniform." };
            npcResponses1 = new string[] { "Just hurry up.", "I've had enough of you." };
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
