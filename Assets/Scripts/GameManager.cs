using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public WinConditionChecker winConditionChecker;
    public float moveDuration = 1f;
    public WinMessage winMessage;
    public LevelData[] levels;
    public CommandManager commandManager; // Komut yöneticisi

    private int currentLevelIndex = 0;
    private Donut selectedDonut = null;
    private Pole originalPole = null;
    private bool isMoving = false;
    private Dictionary<Donut, (Pole, Vector3)> initialDonutStates = new Dictionary<Donut, (Pole, Vector3)>();

    void Start()
    {
        if (levelGenerator == null)
        {
            Debug.LogError("LevelGenerator is not assigned in GameManager.");
            return;
        }

        if (winMessage == null)
        {
            Debug.LogError("WinMessage is not assigned in GameManager.");
            return;
        }

        if (levels.Length == 0)
        {
            Debug.LogError("No levels assigned to the GameManager.");
            return;
        }

        winMessage.nextLevelButton.onClick.RemoveAllListeners();
        winMessage.nextLevelButton.onClick.AddListener(LoadNextLevel);

        LoadLevel(levels[currentLevelIndex]);
    }

    public void LoadLevel(LevelData levelData)
    {
        initialDonutStates.Clear();
        levelGenerator.ClearPreviousLevel();
        levelGenerator.GenerateLevel(levelData);

        List<Pole> poles = new List<Pole>();
        foreach (var poleObj in levelGenerator.GetGeneratedPoles())
        {
            Pole pole = poleObj.GetComponent<Pole>();
            if (pole != null)
            {
                poles.Add(pole);
                foreach (var donut in pole.GetDonutStack())
                {
                    initialDonutStates[donut] = (pole, donut.transform.position);
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

        Pole clickedPole = GetClickedPole(clickPosition);
        if (clickedPole == null)
        {
            Debug.Log("No pole found at click position");
            return;
        }

        if (selectedDonut == null)
        {
            Donut poppedDonut = clickedPole.PopDonut() as Donut;
            if (poppedDonut != null)
            {
                SelectDonut(poppedDonut, clickedPole);
            }
        }
        else
        {
            if (clickedPole == originalPole)
            {
                Debug.Log("Donut already selected, same pole clicked. Cancelling operation.");
                PlaceDonutBackToOriginalPole();
            }
            else
            {
                MoveSelectedDonutToPole(selectedDonut, clickedPole);
            }
        }
    }

    private Pole GetClickedPole(Vector3 clickPosition)
    {
        if (winConditionChecker == null)
        {
            Debug.LogError("WinConditionChecker is not assigned in GameManager.");
            return null;
        }

        foreach (var pole in winConditionChecker.GetPoles())
        {
            if (Vector3.Distance(pole.transform.position, clickPosition) < 1.0f)
            {
                return pole;
            }
        }
        return null;
    }

    public void SelectDonut(Donut donut, Pole pole)
    {
        if (selectedDonut == null)
        {
            selectedDonut = donut;
            originalPole = pole;
            isMoving = true;
            selectedDonut.MoveTo(donut.transform.position + Vector3.up * 2, moveDuration).OnComplete(() =>
            {
                isMoving = false;
            });
        }
    }

    public void MoveSelectedDonutToPole(Donut donut, Pole targetPole)
    {
        if (selectedDonut != null && targetPole != null)
        {
            isMoving = true;
            Vector3 targetPosition = new Vector3(targetPole.stackPosition.position.x, donut.transform.position.y, targetPole.stackPosition.position.z);
            Vector3 originalPosition = new Vector3(originalPole.stackPosition.position.x, donut.transform.position.y, originalPole.stackPosition.position.z);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(donut.transform.DOMove(targetPosition, moveDuration))
                    .Append(donut.transform.DOMoveY(targetPole.stackPosition.position.y + (targetPole.GetDonutCount() * targetPole.donutHeight), moveDuration))
                    .OnComplete(() =>
                    {
                        targetPole.StackDonut(donut);
                        MoveCommand moveCommand = new MoveCommand(donut, originalPole, targetPole, originalPosition, targetPosition, moveDuration);
                        commandManager.ExecuteCommand(moveCommand);
                        selectedDonut = null;
                        originalPole = null;
                        isMoving = false;
                        winConditionChecker.CheckWinCondition();
                    });
        }
    }

    private void PlaceDonutBackToOriginalPole()
    {
        if (selectedDonut != null && originalPole != null)
        {
            isMoving = true;
            Vector3 targetPosition = new Vector3(originalPole.stackPosition.position.x, selectedDonut.transform.position.y, originalPole.stackPosition.position.z);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(selectedDonut.transform.DOMove(targetPosition, moveDuration))
                    .Append(selectedDonut.transform.DOMoveY(originalPole.stackPosition.position.y + (originalPole.GetDonutCount() * originalPole.donutHeight), moveDuration))
                    .OnComplete(() =>
                    {
                        originalPole.StackDonut(selectedDonut);
                        selectedDonut = null;
                        originalPole = null;
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

        selectedDonut = null;
        originalPole = null;
        isMoving = false;

        LoadLevel(levels[currentLevelIndex]);
    }

    private void OnLevelComplete()
    {
        winMessage.ShowWinMessage();
    }
}
