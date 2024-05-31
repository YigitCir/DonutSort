using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    public CommandManager commandManager;
    public Button undoButton;

    void Start()
    {
        if (commandManager == null)
        {
            Debug.LogError("CommandManager is not assigned in UndoButton.");
            return;
        }

        if (undoButton == null)
        {
            Debug.LogError("UndoButton is not assigned in UndoButton.");
            return;
        }

        undoButton.onClick.RemoveAllListeners();
        undoButton.onClick.AddListener(commandManager.UndoCommand);
    }
}