using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : NPC
{
    public override void Interact()
    {
        Debug.Log("granny Interact aufgerufen");

        if (GameManager.isOmaDefeated)
        {
            // Dialog, wenn isOmaDefeated wahr ist
            initialDialogLines = new string[] { "NOOOOOOOOOO-", "YOU", "THAT SLOT MACHINE WAS MY WHOLE LIFE!" };
            playerQuestions1 = new string[] { "Madame you played en-" };
            npcResponses1 = new string[] { " DON'T INTERRUPT ME!", "YOU TOOK EVERYTHING FROM ME!" };
        }
        else
        {
            // Ursprünglicher Dialog
            initialDialogLines = new string[] { "Who dares to interrupt my slot machine time?!", "I don't have much time left to live, and I want to spend it with my lucky slot machine." };
            playerQuestions1 = new string[] { "Well, I am lo-" };
            npcResponses1 = new string[] { "Well, drowning your sorrows in a bottle won't bring back the dead, will it?", "Watch out for the cable!" };
        }

        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("Granny Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Dialog mit Granny beendet");
    }

}
