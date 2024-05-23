using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMessage : MonoBehaviour
{
    public TextMeshProUGUI winText;

    void Start()
    {
        winText.gameObject.SetActive(false);
    }

    public void ShowWinMessage()
    {
        winText.gameObject.SetActive(true);
        winText.text = "You win!";
    }
}