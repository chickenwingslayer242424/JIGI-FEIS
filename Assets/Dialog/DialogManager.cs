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
    public Image npc0Image; // Referenz auf das Bildobjekt für den NPC
    public Image pcImage; // Referenz auf das Bildobjekt für den Player-Charakter
    public Button nextButton; // Der Button für die nächste Dialogzeile

    private Queue<string> sentences; // Warteschlange für die Dialogsätze
    public static bool isDialogActive = false; // Zustand des Dialogs //Neuer Kommentar
    private Queue<string> npcSentences; // Dialogsätze des NPCs
    private Queue<string> pcSentences; // Dialogsätze des Player Characters (PC)
    private bool isNpcSpeaking = true; // Flag, um zu verfolgen, wer spricht

    private NPC currentNpc; // Variable zum Speichern des aktuellen NPCs 
    public GameObject npcNamePanel; // Panel für das Namensschild des NPCs
    public GameObject pcNamePanel; // Panel für das Namensschild des PCs
    public TextMeshProUGUI npcNameText; // Text innerhalb des NPC Namensschildes
    public TextMeshProUGUI pcNameText; // Text innerhalb des PC Namensschildes


    void Start()
    {
        // Dialogpanel zu Beginn deaktivieren
        dialogPanel.SetActive(false); // Dialogfeld ausblenden

        npcSentences = new Queue<string>(); // Initialisiere die NPC-Dialog-Warteschlange
        pcSentences = new Queue<string>(); // Initialisiere die PC-Dialog-Warteschlange
        nextButton.onClick.AddListener(DisplayNextSentence); // Button mit der Methode verknüpfen
    }

    public void StartDialog(NPC npc) // Diese Methode erwartet nur den NPC-Parameter
    {
        currentNpc = npc; // Speichert den übergebenen NPC in der aktuellen Instanz //Neuer Kommentar
        dialogPanel.SetActive(true); // Dialogfeld anzeigen
        npcSentences.Clear(); // Leert die NPC-Dialog-Warteschlange
        pcSentences.Clear(); // Leert die PC-Dialog-Warteschlange

        foreach (string sentence in npc.dialogLines)
        {
            npcSentences.Enqueue(sentence); // Füge die NPC-Dialogzeilen zur Warteschlange hinzu
        }

        foreach (string sentence in npc.pcDialogLines)
        {
            pcSentences.Enqueue(sentence); // Füge die PC-Dialogzeilen zur Warteschlange hinzu
        }

        isNpcSpeaking = true; // Starte mit NPC-Dialog
        npcImage.sprite = npc.npcSprite; // Setze das NPC-Image
        pcImage.sprite = npc.pcImage; // Setze das PC-Image (kann optional sein, je nach deiner Implementierung)
        
        // Aktualisiere Namensschilder je nach Sprecher
        npcNameText.text = npc.npcName; // Setze den NPC-Namen
        pcNameText.text = "sexy chick"; // Setze den Namen des Spielers (kann je nach Bedarf variieren)
        
        DisplayNextSentence(); // Zeige die nächste Dialogzeile an
    }

    public void DisplayNextSentence()
    {
        if (isNpcSpeaking)
        {
            if (npcSentences.Count == 0)
            {
                SwitchToPCDialog(); // Wechsel zu PC-Dialog
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
        else
        {
            if (pcSentences.Count == 0)
            {
                EndDialog(); // Beende den Dialog
                return;
            }

            string sentence = pcSentences.Dequeue(); // Nächste PC-Dialogzeile aus der Warteschlange
            StopAllCoroutines(); // Stoppe alle laufenden Coroutinen
            StartCoroutine(TypeSentence(sentence)); // Starte die Coroutine zum schrittweisen Anzeigen des Satzes
             // Zeige PC-Image an, verstecke NPC-Image
            pcImage.gameObject.SetActive(true);
            npcImage.gameObject.SetActive(false);

            // Aktiviere das PC Namensschild und deaktiviere das NPC Namensschild
            pcNamePanel.SetActive(true);
            npcNamePanel.SetActive(false);

            Debug.Log("PC Image sichtbar: " + pcImage.gameObject.activeSelf); // Debug-Ausgabe
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = ""; // Setze den Dialogtext zurück
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter; // Füge den nächsten Buchstaben zum Dialogtext hinzu
            yield return null; // Warte einen Frame
        }
    }

    void SwitchToPCDialog()
    {
        isNpcSpeaking = false; // Starte mit PC-Dialog
        DisplayNextSentence(); // Zeige die nächste Dialogzeile an
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false); // Dialogfeld ausblenden
        isDialogActive = false; // Dialogzustand deaktivieren //Neuer Kommentar
        // Weitere Logik nach dem Dialogende, falls nötig
    }
}
