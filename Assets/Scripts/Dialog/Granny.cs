using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : NPC
{
    public override void Interact()
    {
        Debug.Log("granny Interact aufgerufen");

        // Setze die Dialogzeilen für den KeySpawner
        initialDialogLines = new string[] { "Who dares to interrupt my slot machine time?!",  "I don't have much time left to live, and I want to spend it with my lucky slot machine." };
        playerQuestions1 = new string[] { "well, I am lo-" };
        npcResponses1 = new string[] { "Well, drowning your sorrows in a bottle won't bring back the dead, will it?", "Watch out for the cable!"};

        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("Granny Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Dialog mit Granny beendet");
    }
}