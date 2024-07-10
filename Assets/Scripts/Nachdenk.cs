using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NachdenkManager : MonoBehaviour
{
    public static NachdenkManager Instance;

    public GameObject nachdenkCanvas;
    public TextMeshProUGUI nachdenkText;
    public Button exitButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (nachdenkCanvas != null)
        {
            nachdenkCanvas.SetActive(false);
        }

        exitButton.onClick.AddListener(CloseNachdenkCanvas);
    }

    public void ShowNachdenkCanvas(string text)
    {
        if (nachdenkCanvas != null)
        {
            nachdenkText.text = text;
            nachdenkCanvas.SetActive(true);
            GameManager.isPopupActive = true;
        }
    }

    public void CloseNachdenkCanvas()
    {
        if (nachdenkCanvas != null)
        {
            nachdenkCanvas.SetActive(false);
            GameManager.isPopupActive = false;
        }
    }
}
