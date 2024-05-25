using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public WinConditionChecker winConditionChecker;
    public float moveDuration = 1f;
    public WinMessage winMessage;
    public LevelData[] levels;
    private int currentLevelIndex = 0;

    private DonutView _selectedDonutView = null;
    private PoleView _originalPoleView = null;
    private bool isMoving = false;

    private Dictionary<DonutView, (PoleView, Vector3)> initialDonutStates = new Dictionary<DonutView, (PoleView, Vector3)>();

    void Start()
    {
     
        // Not needed, unity already throws error, and it should. 
        // if (levelGenerator == null)
        // {
        //     Debug.LogError("LevelGenerator is not assigned in GameManager.");
        //     return;
        // }
        //
        // if (winMessage == null)
        // {
        //     Debug.LogError("WinMessage is not assigned in GameManager.");
        //     return;
        // }

        if (levels.Length == 0)
        {
            Debug.LogError("No levels assigned to the GameManager.");
            return;
        }

        
        winMessage.nextLevelButton.onClick.RemoveAllListeners(); 
      //  winMessage.nextLevelButton.onClick.AddListener(LoadNextLevel);

        LoadLevel(levels[currentLevelIndex]);
    }

    public void LoadLevel(LevelData levelData)
    {
        initialDonutStates.Clear();
        levelGenerator.ClearPreviousLevel();
        levelGenerator.GenerateLevel(levelData);

        List<PoleView> poles = new List<PoleView>();
        foreach (var poleObj in levelGenerator.GetGeneratedPoles())
        {
            PoleView poleView = poleObj.GetComponent<PoleView>();
            if (poleView != null)
            {
                poles.Add(poleView);
                foreach (var donut in poleView.GetDonutStack())
                {
                    initialDonutStates[donut] = (poleView, donut.transform.position);
                }
            }
        }
        winConditionChecker.SetPoles(poles);
        winConditionChecker.onWin.RemoveAllListeners(); 
        winConditionChecker.onWin.AddListener(OnLevelComplete); 
    }

    public void LoadNextLevel()
    {
        Debug.Log("LoadNextLevel called");
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Length)
        {
            currentLevelIndex = 0; 
        }
        LoadLevel(levels[currentLevelIndex]);
        winMessage.nextLevelButton.gameObject.SetActive(false);
        winMessage.winText.gameObject.SetActive(false);
    }

    public void HandlePoleClicked(Vector3 clickPosition)
    {
        if (isMoving) return;

        PoleView clickedPoleView = GetClickedPole(clickPosition);
        if (clickedPoleView == null)
        {
            Debug.Log("No pole found at click position");
            return;
        }

        if (_selectedDonutView == null)
        {
            DonutView poppedDonutView = clickedPoleView.PopDonut() as DonutView;
            if (poppedDonutView != null)
            {
                SelectDonut(poppedDonutView, clickedPoleView);
            }
        }
        else
        {
            if (clickedPoleView == _originalPoleView)
            {
                Debug.Log("Donut already selected, same pole clicked. Cancelling operation.");
                PlaceDonutBackToOriginalPole();
            }
            else
            {
                MoveSelectedDonutToPole(_selectedDonutView, clickedPoleView);
            }
        }
    }

    private PoleView GetClickedPole(Vector3 clickPosition)
    {
        if (winConditionChecker == null)
        {
            Debug.LogError("WinConditionChecker is not assigned in GameManager.");
            return null;
        }

        foreach (var pole in winConditionChecker.GetPoles())
        {
            if (Vector3.Distance(pole.transform.position, clickPosition) < 2.0f)
            {
                return pole;
            }
        }
        return null;
    }

    public void SelectDonut(DonutView donutView, PoleView poleView)
    {
        if (_selectedDonutView == null)
        {
            _selectedDonutView = donutView;
            _originalPoleView = poleView;
            isMoving = true;
            _selectedDonutView.MoveTo(donutView.transform.position + Vector3.up * 2, moveDuration).OnComplete(() =>
            {
                isMoving = false;
            });
        }
    }

    public void MoveSelectedDonutToPole(DonutView donutView, PoleView targetPoleView)
    {
        if (_selectedDonutView != null && targetPoleView != null)
        {
            isMoving = true;
            Vector3 targetPosition = new Vector3(targetPoleView.stackPosition.position.x, donutView.transform.position.y, targetPoleView.stackPosition.position.z);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(donutView.transform.DOMove(targetPosition, moveDuration))
                    .Append(donutView.transform.DOMoveY(targetPoleView.stackPosition.position.y + (targetPoleView.GetDonutCount() * targetPoleView.donutHeight), moveDuration))
                    .OnComplete(() =>
                    {
                        targetPoleView.StackDonut(donutView);
                        _selectedDonutView = null;
                        _originalPoleView = null;
                        isMoving = false;
                        winConditionChecker.CheckWinCondition();
                    });
        }
    }

    private void PlaceDonutBackToOriginalPole()
    {
        if (_selectedDonutView != null && _originalPoleView != null)
        {
            isMoving = true;
            Vector3 targetPosition = new Vector3(_originalPoleView.stackPosition.position.x, _selectedDonutView.transform.position.y, _originalPoleView.stackPosition.position.z);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_selectedDonutView.transform.DOMove(targetPosition, moveDuration))
                    .Append(_selectedDonutView.transform.DOMoveY(_originalPoleView.stackPosition.position.y + (_originalPoleView.GetDonutCount() * _originalPoleView.donutHeight), moveDuration))
                    .OnComplete(() =>
                    {
                        _originalPoleView.StackDonut(_selectedDonutView);
                        _selectedDonutView = null;
                        _originalPoleView = null;
                        isMoving = false;
                    });
        }
    }

    public void ResetLevel()
    {
        foreach (var pole in winConditionChecker.GetPoles())
        {
            pole.ClearDonuts(); 
        }

        _selectedDonutView = null;
        _originalPoleView = null;
        isMoving = false;

        
        LoadLevel(levels[currentLevelIndex]);
    }

    private void OnLevelComplete()
    {
        WinMessage.InvokeSetActiveWinMessage(true);
    }
}
