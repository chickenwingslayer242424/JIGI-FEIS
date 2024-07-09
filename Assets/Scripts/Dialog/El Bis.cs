using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElBis : NPC
{
    public override void Interact()
    {
        Debug.Log("El Bis Interact aufgerufen");

        // Setze die Dialogzeilen für den KeySpawner
        initialDialogLines = new string[] { "I can't.", "I just can't." };
        playerQuestions1 = new string[] { "OH MY!!!","El Bis, my love and I are big fans of yours!","We've always watched your performances at the club together.", "Why don't you start moving your hips?",  };
        npcResponses1 = new string[] { "I can't.","I have a lot of holes in my pants, so I can't put on a show.", "I need a new pair of pants to cover my two round cheeks."};

        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("El Bis Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Dialog mit El Bis beendet");
    }
}
