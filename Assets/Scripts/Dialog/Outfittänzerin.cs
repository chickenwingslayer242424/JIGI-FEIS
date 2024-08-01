using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitTänzerin : NPC
{
    public override void Interact()
    {
        Debug.Log("tänzer Interact aufgerufen");

        // Setze die Dialogzeilen für den KeySpawner
        initialDialogLines = new string[] {"Ugh, it's so frustrating.",  "I can't even start my dancing routine properly because our new singer is horrible.", "The missing lyrics are throwing off my entire performance." };
        playerQuestions1 = new string[] {"Maybe you could find something to help you fill in the missing lyrics." ,"That might get the music flowing right." };
        npcResponses1 = new string[] { "If you can make that happen, honey", "I'll tear these bell-bottoms right off and give them to you as a souvenir.", "Deal?"};
        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("Singer Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Singer mit Granny beendet");
    }
}