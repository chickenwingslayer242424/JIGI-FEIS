using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tänzer1 : NPC
{
    public override void Interact()
    {
        Debug.Log("tänzer Interact aufgerufen");

        // Setze die Dialogzeilen für den KeySpawner
        initialDialogLines = new string[] { "Ready to see some moves that'll make your jaw drop?"};
        
        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("Singer Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Singer mit Granny beendet");
    }
}