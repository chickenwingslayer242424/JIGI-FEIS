using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoss : NPC
{
    public override void Interact()
    {
        Debug.Log("Mafioso Interact aufgerufen");

        // Setze die Dialogzeilen für den KeySpawner
        initialDialogLines = new string[] { "Argh.", "That damn El Bis went berserk.", "First he yanked out  all the cables from the power box.",  "Because he didn't want anyone to see him! he's supposed to help us marry." , "I’ve got hostages causing trouble in the basement, and I need to get down there asap." };
        playerQuestions1 = new string[] { "Hmm, I might be able to take care of it.", "But you have to take me to your Basement, I want to take a look."};
        npcResponses1 = new string[] { "Fine, but you better not screw this up.", "I need to marry my beautiful gem Cherry.", "Do this right, and there might be something extra in it for you."};

        // Starte den Dialog direkt ohne die Interact-Methode der Basisklasse aufzurufen
        FindObjectOfType<DialogManager>().StartDialog(this);

        Debug.Log("Mafioso Sprite gesetzt: " + (npcSprite != null ? npcSprite.name : "null"));
    }

    public override void OnDialogEnd()
    {
        Debug.Log("Dialog mit Mafioso beendet");
    }
}
