using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNPC : NPC
{
    public ItemData rewardItemForOmaDefeat; // Belohnungs-Item
    
    public override void Interact()
    {
        Debug.Log("Interact aufgerufen");
        base.Interact();

        if (GameManager.isOmaDefeated)
        {
            initialDialogLines = new string[] { "Thank you!", "At last, the lady's oppression has come to an end.", "Here... be careful in there. I can’t go there by myself anymore.", "There's a really disgusting creature on the loose. It's horrible!" };
            playerQuestions1 = new string[] { "Aight but do you know how I can find my lover?" };
            npcResponses1 = new string[] { "Hm, one of the gangsters is in the wedding chapel marrying his chick", "Maybe he can help you, but you can only get into the chapel with a key.", "Because it's a special event.", "I think the barkeeper has the keys." };
            playerQuestions2 = new string[] { "They’re probably in the basement with the gangsters." };
            npcResponses2 = new string[] { "You'll need to find a way to get past them. Maybe someone in the area knows a secret passage." };

            if (!hasReceivedItem)
            {
                hasReceivedItem = true;
                GameManager.collectedItems.Add(rewardItemForOmaDefeat);
                Debug.Log("Item hinzugefügt: " + rewardItemForOmaDefeat.objectName);
                FindObjectOfType<GameManager>().UpdateEquipmentCanvas();
            }
        }
        else
        {
            GameManager.hasSpokenToCasinoDealer = true;

            if (!hasReceivedItem && requiredItemID != 0)
            {
                if (GameManager.Instance.IsSelectedItem(requiredItemID))
                {
                    GameManager.collectedItems.Remove(GameManager.Instance.selectedItem);
                    hasReceivedItem = true;
                    GameManager.collectedItems.Add(rewardItem);

                    initialDialogLines = new string[] { "Thank you!", "At last, the lady's oppression has come to an end.", "Here... be careful in there. I can’t go there by myself anymore.", "There's a really disgusting creature on the loose. It's horrible!" };
                    playerQuestions1 = new string[] { "Aight but do you know how I can find my lover?" };
                    npcResponses1 = new string[] { "Hm, one of the gangsters is in the wedding chapel marrying his chick", "Maybe he can help you, but you can only get into the chapel with a key.", "Because it's a special event.", "I think the barkeeper has the keys." };
                    playerQuestions2 = new string[] { "They’re probably in the basement with the gangsters." };
                    npcResponses2 = new string[] { "You'll need to find a way to get past them. Maybe someone in the area knows a secret passage." };
                    FindObjectOfType<GameManager>().UpdateEquipmentCanvas();
                }
            }
            else
            {
                initialDialogLines = new string[] { "W-Welcome to the casino. C-Can I offer you the usual?" };
                playerQuestions1 = new string[] { "I'm looking for my love. Have you seen them?" };
                npcResponses1 = new string[] { "S-Sorry, I haven’t seen anyone particular", "Maybe someone in the VIP area could help you out?" };
                playerQuestions2 = new string[] { "Even if I would know how to get there...", "...V-I-P." ,"The security blocks the entrance." };
                npcResponses2 = new string[] { "Um... there was a big incident in the VIP area today. Three employees quit.", "I still have their uniforms I-I can give you one if you help me with something.","P-Please?"};
                playerQuestions3 = new string[] { "What can I do for you?" };
                npcResponses3 = new string[] { "There's a granny who always wins at the same slot machine, she scamming me and I can't beat her.", "If you sabotage her winning streak, I'll give you one of the uniforms.","You could sneek your way around security with them!" };
            }
        }

        FindObjectOfType<DialogManager>().StartDialog(this);
    }
}
