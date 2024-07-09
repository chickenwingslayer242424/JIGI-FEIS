using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mafiosa : NPC
{
    public override void Interact()
    {
        Debug.Log("Mafiosa Interact aufgerufen");

        // Setze die Dialogzeilen für den KeySpawner
        initialDialogLines = new string[] { "Hihi","Tonight is perfect, nothing else matters",  "I'm getting a new outfit after this, can you believe it?" };
        playerQuestions1 = new string[] { "Mhm, alright girl." };
        npcResponses1 = new string[] { "Everything is just... wonderful!", "Not even the darkest room will ruin my mood!"};

        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("Mafiosa Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Dialog mit Mafiosa beendet");
    }
}
