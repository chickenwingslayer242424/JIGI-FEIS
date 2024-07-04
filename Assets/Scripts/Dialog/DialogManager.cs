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
    private Queue<string> playerQuestions; // Dialogsätze des Spielers
    private Queue<string> npcResponses; // Dialogsätze des NPCs
    private bool isNpcSpeaking = true; // Flag, um zu verfolgen, wer spricht

    private NPC currentNpc; // Variable zum Speichern des aktuellen NPCs 
    public GameObject npcNamePanel; // Panel für das Namensschild des NPCs
    public GameObject pcNamePanel; // Panel für das Namensschild des PCs
    public TextMeshProUGUI npcNameText; // Text innerhalb des NPC Namensschildes
    public TextMeshProUGUI pcNameText; // Text innerhalb des PC Namensschildes

    public static bool isDialogActive; // Diese Eigenschaft wurde hinzugefügt
    private bool hasDrinkSpawned = false; // Variable zum Überprüfen, ob der Drink gespawnt wurde

    private int currentPlayerQuestionSet = 1; // Neue Variable zum Verfolgen des aktuellen Fragensatzes
    private bool hasTalkedToBarkeeper = false;

    void Start()
    {
        dialogPanel.SetActive(false); // Dialogfeld ausblenden

        npcSentences = new Queue<string>(); // Initialisiere die NPC-Dialog-Warteschlange
        playerQuestions = new Queue<string>(); // Initialisiere die Spieler-Dialog-Warteschlange
        npcResponses = new Queue<string>(); // Initialisiere die NPC-Antwort-Warteschlange
        nextButton.onClick.AddListener(DisplayNextSentence); // Button mit der Methode verknüpfen
    }

    public void StartDialog(NPC npc)
    {
        currentNpc = npc; // Speichert den übergebenen NPC in der aktuellen Instanz
        dialogPanel.SetActive(true); // Dialogfeld anzeigen
        npcSentences.Clear(); // Leert die NPC-Dialog-Warteschlange
        playerQuestions.Clear(); // Leert die Spieler-Dialog-Warteschlange
        npcResponses.Clear(); // Leert die NPC-Antwort-Warteschlange

        // Initialisiere die NPC-Sätze
        foreach (string sentence in currentNpc.initialDialogLines)
        {
            npcSentences.Enqueue(sentence);
        }

        // Initialisiere die Spielerfragen und NPC-Antworten für den ersten Satz
        LoadQuestionSet(1);

        if (npc is NPC)
        {
            hasTalkedToBarkeeper = true;
        }

        isNpcSpeaking = true; // Starte mit NPC-Dialog
        npcImage.sprite = npc.npcSprite; // Setze das NPC-Image
        pcImage.sprite = npc.pcImage; // Setze das PC-Image (kann optional sein, je nach deiner Implementierung)

        npcNameText.text = npc.npcName; // Setze den NPC-Namen
        pcNameText.text = "sexy chick"; // Setze den Namen des Spielers (kann je nach Bedarf variieren)

        isDialogActive = true; // Setze den Dialogstatus auf aktiv
        hasDrinkSpawned = false; // Zurücksetzen der Drink-Spawn-Variable
        DisplayNextSentence(); // Zeige die nächste Dialogzeile an
    }

    public void DisplayNextSentence()
    {
        if (isNpcSpeaking)
        {
            if (npcSentences.Count == 0)
            {
                SwitchToPlayerQuestion();
                return;
            }

            string sentence = npcSentences.Dequeue(); // Nächste NPC-Dialogzeile aus der Warteschlange
            StopAllCoroutines(); // Stoppe alle laufenden Coroutinen
            StartCoroutine(TypeSentence(sentence)); // Starte die Coroutine zum schrittweisen Anzeigen des Satzes
            npcImage.gameObject.SetActive(true); // Zeige NPC-Image an
            pcImage.gameObject.SetActive(false); // Verstecke PC-Image

            npcNamePanel.SetActive(true); // Aktiviere das NPC Namensschild
            pcNamePanel.SetActive(false); // Deaktiviere das PC Namensschild

            Debug.Log("NPC Sprite gesetzt: " + (npcImage.sprite != null ? npcImage.sprite.name : "null")); // Debug-Ausgabe
        }
        else if (playerQuestions.Count > 0)
        {
            string question = playerQuestions.Dequeue(); // Nächste Spieler-Dialogzeile aus der Warteschlange
            StopAllCoroutines(); // Stoppe alle laufenden Coroutinen
            StartCoroutine(TypeSentence(question)); // Starte die Coroutine zum schrittweisen Anzeigen des Satzes
            npcImage.gameObject.SetActive(false); // Verstecke NPC-Image
            pcImage.gameObject.SetActive(true); // Zeige PC-Image an

            npcNamePanel.SetActive(false); // Deaktiviere das NPC Namensschild
            pcNamePanel.SetActive(true); // Aktiviere das PC Namensschild
        }
        else if (npcResponses.Count > 0)
        {
            string response = npcResponses.Dequeue(); // Nächste NPC-Antwortzeile aus der Warteschlange
            StopAllCoroutines(); // Stoppe alle laufenden Coroutinen
            StartCoroutine(TypeSentence(response)); // Starte die Coroutine zum schrittweisen Anzeigen des Satzes
            npcImage.gameObject.SetActive(true); // Zeige NPC-Image an
            pcImage.gameObject.SetActive(false); // Verstecke PC-Image

            npcNamePanel.SetActive(true); // Aktiviere das NPC Namensschild
            pcNamePanel.SetActive(false); // Deaktiviere das PC Namensschild

            if (npcResponses.Count == 0)
            {
                LoadNextQuestionSet();
            }
        }
        else
        {
            EndDialog(); // Beende den Dialog
        }
    }

    void SwitchToPlayerQuestion()
    {
        isNpcSpeaking = false;
        DisplayNextSentence(); // Zeige die nächste Spieler-Dialogzeile an
    }

    void LoadNextQuestionSet()
    {
        currentPlayerQuestionSet++;
        LoadQuestionSet(currentPlayerQuestionSet);
    }

    void LoadQuestionSet(int questionSet)
    {
        playerQuestions.Clear();
        npcResponses.Clear();

        switch (questionSet)
        {
            case 1:
                if (currentNpc.playerQuestions1 != null && currentNpc.npcResponses1 != null)
                {
                    foreach (string question in currentNpc.playerQuestions1)
                    {
                        playerQuestions.Enqueue(question);
                    }
                    foreach (string response in currentNpc.npcResponses1)
                    {
                        npcResponses.Enqueue(response);
                    }
                }
                break;
            case 2:
                if (currentNpc.playerQuestions2 != null && currentNpc.npcResponses2 != null)
                {
                    foreach (string question in currentNpc.playerQuestions2)
                    {
                        playerQuestions.Enqueue(question);
                    }
                    foreach (string response in currentNpc.npcResponses2)
                    {
                        npcResponses.Enqueue(response);
                    }
                }
                break;
            case 3:
                if (currentNpc.playerQuestions3 != null && currentNpc.npcResponses3 != null)
                {
                    foreach (string question in currentNpc.playerQuestions3)
                    {
                        playerQuestions.Enqueue(question);
                    }
                    foreach (string response in currentNpc.npcResponses3)
                    {
                        npcResponses.Enqueue(response);
                    }
                }
                break;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = ""; // Leere den Text
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter; // Füge Buchstabe für Buchstabe hinzu
            yield return null; // Warte einen Frame
        }
    }

    public void EndDialog()
    {
        dialogPanel.SetActive(false); // Dialogfeld ausblenden
        isDialogActive = false; // Setze den Dialogstatus auf inaktiv
    }

    void Update()
    {
        if (hasTalkedToBarkeeper)
        {
            // Rufe die Methode IsSelectedItem auf und übergebe das Item
            if (GameManager.Instance.IsSelectedItem(currentNpc.requiredItem) && !hasDrinkSpawned)
            {
                // Code zum Spawnen des Drinks
                Debug.Log("Drink wird gespawnt");
                hasDrinkSpawned = true; // Setze die Variable auf true
            }
        }
    }
}
