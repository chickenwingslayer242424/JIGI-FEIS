using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel; // Das Dialogfeld-UI-Element
    public TextMeshProUGUI dialogText; // Der Text-UI-Komponente für den Dialog
    public Image npcImage; // Referenz auf das Bildobjekt für den NPC
    public Image pcImage; // Referenz auf das Bildobjekt für den Player-Charakter
    public Button nextButton; // Der Button für die nächste Dialogzeile

    private Queue<string> npcSentences; // Dialogsätze des NPCs
    private string playerQuestion; // Dialogzeile des Players
    private string npcResponse; // Antwort des NPCs auf die Frage des Spielers
    private bool isNpcSpeaking = true; // Flag, um zu verfolgen, wer spricht

    private NPC currentNpc; // Variable zum Speichern des aktuellen NPCs 
    public GameObject npcNamePanel; // Panel für das Namensschild des NPCs
    public GameObject pcNamePanel; // Panel für das Namensschild des PCs
    public TextMeshProUGUI npcNameText; // Text innerhalb des NPC Namensschildes
    public TextMeshProUGUI pcNameText; // Text innerhalb des PC Namensschildes

    public static bool isDialogActive; // Diese Eigenschaft wurde hinzugefügt

    void Start()
    {
        // Dialogpanel zu Beginn deaktivieren
        dialogPanel.SetActive(false); // Dialogfeld ausblenden

        npcSentences = new Queue<string>(); // Initialisiere die NPC-Dialog-Warteschlange
        nextButton.onClick.AddListener(DisplayNextSentence); // Button mit der Methode verknüpfen
    }

    public void StartDialog(NPC npc) // Diese Methode erwartet nur den NPC-Parameter
    {
        currentNpc = npc; // Speichert den übergebenen NPC in der aktuellen Instanz
        dialogPanel.SetActive(true); // Dialogfeld anzeigen
        npcSentences.Clear(); // Leert die NPC-Dialog-Warteschlange

        foreach (string sentence in npc.initialDialogLines)
        {
            npcSentences.Enqueue(sentence); // Füge die NPC-Dialogzeilen zur Warteschlange hinzu
        }

        playerQuestion = npc.playerQuestion; // Setze die Frage des Spielers
        npcResponse = npc.npcResponse; // Setze die Antwort des NPCs auf die Frage

        isNpcSpeaking = true; // Starte mit NPC-Dialog
        npcImage.sprite = npc.npcSprite; // Setze das NPC-Image
        pcImage.sprite = npc.pcImage; // Setze das PC-Image (kann optional sein, je nach deiner Implementierung)

        // Aktualisiere Namensschilder je nach Sprecher
        npcNameText.text = npc.npcName; // Setze den NPC-Namen
        pcNameText.text = "sexy chick"; // Setze den Namen des Spielers (kann je nach Bedarf variieren)
        
        isDialogActive = true; // Setze den Dialogstatus auf aktiv
        DisplayNextSentence(); // Zeige die nächste Dialogzeile an
    }

    public void DisplayNextSentence()
    {
        if (isNpcSpeaking)
        {
            if (npcSentences.Count == 0)
            {
                SwitchToPlayerQuestion(); // Wechsel zur Spielerfrage
                return;
            }

            string sentence = npcSentences.Dequeue(); // Nächste NPC-Dialogzeile aus der Warteschlange
            StopAllCoroutines(); // Stoppe alle laufenden Coroutinen
            StartCoroutine(TypeSentence(sentence)); // Starte die Coroutine zum schrittweisen Anzeigen des Satzes
            // Zeige NPC-Image an, verstecke PC-Image
            npcImage.gameObject.SetActive(true);
            pcImage.gameObject.SetActive(false);

            // Aktiviere das NPC Namensschild und deaktiviere das PC Namensschild
            npcNamePanel.SetActive(true);
            pcNamePanel.SetActive(false);

            Debug.Log("NPC Sprite gesetzt: " + (npcImage.sprite != null ? npcImage.sprite.name : "null")); // Debug-Ausgabe
        }
        else if (playerQuestion != null)
        {
            StopAllCoroutines(); // Stoppe alle laufenden Coroutinen
            StartCoroutine(TypeSentence(playerQuestion)); // Starte die Coroutine zum schrittweisen Anzeigen der Frage
            playerQuestion = null; // Leere die Spielerfrage nach dem Anzeigen
            // Zeige PC-Image an, verstecke NPC-Image
            pcImage.gameObject.SetActive(true);
            npcImage.gameObject.SetActive(false);

            // Aktiviere das PC Namensschild und deaktiviere das NPC Namensschild
            pcNamePanel.SetActive(true);
            npcNamePanel.SetActive(false);

            Debug.Log("PC Image sichtbar: " + pcImage.gameObject.activeSelf); // Debug-Ausgabe
        }
        else
        {
            if (npcResponse != null)
            {
                StopAllCoroutines(); // Stoppe alle laufenden Coroutinen
                StartCoroutine(TypeSentence(npcResponse)); // Starte die Coroutine zum schrittweisen Anzeigen der Antwort
                npcResponse = null; // Leere die NPC-Antwort nach dem Anzeigen
                // Zeige NPC-Image an, verstecke PC-Image
                npcImage.gameObject.SetActive(true);
                pcImage.gameObject.SetActive(false);

                // Aktiviere das NPC Namensschild und deaktiviere das PC Namensschild
                npcNamePanel.SetActive(true);
                pcNamePanel.SetActive(false);

                Debug.Log("NPC Sprite gesetzt: " + (npcImage.sprite != null ? npcImage.sprite.name : "null")); // Debug-Ausgabe
            }
            else
            {
                EndDialog(); // Beende den Dialog
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = ""; // Setze den Dialogtext auf leer
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter; // Füge Buchstaben nacheinander hinzu
            yield return null; // Warte einen Frame
        }
    }

    void SwitchToPlayerQuestion()
    {
        isNpcSpeaking = false; // Wechsle zu Spielerfrage
        DisplayNextSentence(); // Zeige die Spielerfrage an
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false); // Blende das Dialogfeld aus
        isNpcSpeaking = true; // Setze den Sprecher auf NPC zurück
        isDialogActive = false; // Setze den Dialogstatus auf inaktiv
        currentNpc.OnDialogEnd(); // Rufe die Methode OnDialogEnd des aktuellen NPCs auf
    }
}