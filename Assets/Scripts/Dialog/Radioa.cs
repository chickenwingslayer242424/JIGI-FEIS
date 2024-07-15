using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radioa : NPC
{
    public override void Interact()
    {
        Debug.Log("Mafioso Interact aufgerufen");

        // Setze die Dialogzeilen für den KeySpawner
        initialDialogLines = new string[] { "This new top hit.", "Listen closely to the lyrics, they'll be stuck in your head.", "You don't mess with the dancers..." , "...they click on the drunk ones... ", "...then on the champagne bottles... ", "...then on the poles.... " };
        playerQuestions1 = new string[] { "Hmm, that sounds like the song the singer is trying to sing."};
        npcResponses1 = new string[] { "You don't mess with the dancers..." , "...they click on the drunk ones... ", "...then on the champagne bottles... ", "...then on the poles.... "};

        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("Mafioso Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Dialog mit Mafioso beendet");
    }
}
