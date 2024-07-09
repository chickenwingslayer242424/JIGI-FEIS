using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singer : NPC
{
    public override void Interact()
    {
        Debug.Log("Singer Interact aufgerufen");

        // Setze die Dialogzeilen für den KeySpawner
        initialDialogLines = new string[] { "NoW yOu....ehm... home aloOoO-, and...ehm..I...ehm bAck my sTUff"};
        playerQuestions1 = new string[] {"I guess if she knew the right lyrics, the party would be going."};
        npcResponses1 = new string[] { "- OoOold BotTle from...ehm...FiRst NiGhT we droOoOowned"};

        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("Singer Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Singer mit Granny beendet");
    }
}