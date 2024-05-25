using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMessage : MonoBehaviour
{
    public TextMeshProUGUI winText;
    public Button nextLevelButton;

    void Start()
    {
        winText.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
    }

    public void ShowWinMessage()
    {
        winText.gameObject.SetActive(true);
        winText.text = "You win!";
        nextLevelButton.gameObject.SetActive(true);
    }
}