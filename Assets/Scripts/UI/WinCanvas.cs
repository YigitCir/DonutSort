using System;
using TMPro;
using UI.Canvas;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WinCanvas : SingletonCanvas<WinCanvas>
    {
        protected static event Action<bool> SetActiveWinMessage;
        public static void InvokeSetActiveWinMessage(bool obj)
        {
            SetActiveWinMessage?.Invoke(obj);
        }
    
        public CanvasGroup CanvasGroup;
        public TextMeshProUGUI winText;
        public Button nextLevelButton;

        protected virtual void Start()
        {
            SetActiveWinMessage += OnSetActiveWinMessage;
            nextLevelButton.onClick.AddListener(OnButtonPressed);
            winText.text = "You win!";
            SetDefaultState();
        }
    
        protected virtual void OnDestroy()
        {
            SetActiveWinMessage -= OnSetActiveWinMessage;
            nextLevelButton.onClick.RemoveListener(OnButtonPressed);
        }

        protected virtual void SetDefaultState()
        {
            winText.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(false);
        }

        protected virtual void OnSetActiveWinMessage(bool isShow)
        {
            winText.gameObject.SetActive(isShow);
            nextLevelButton.gameObject.SetActive(isShow);
        }

        protected virtual void OnButtonPressed()
        {
        
        }
    
    }
}