using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tänzer2 : NPC
{
    public override void Interact()
    {
        Debug.Log("tänzer Interact aufgerufen");

        // Setze die Dialogzeilen für den KeySpawner
        initialDialogLines = new string[] {"I hope you've got your dollars ready."};
        
        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("Singer Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Singer mit Granny beendet");
    }
}