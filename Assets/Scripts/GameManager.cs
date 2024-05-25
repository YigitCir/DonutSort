using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public WinConditionChecker winConditionChecker;
    public float moveDuration = 1f;

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

        levelGenerator.GenerateLevel();

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
        foreach (var pole in winConditionChecker.GetPoles())
        {
            if (Vector3.Distance(pole.transform.position, clickPosition) < 2.0f)
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
            Sequence sequence = DOTween.Sequence();
            sequence.Append(donut.transform.DOMove(targetPosition, moveDuration))
                    .Append(donut.transform.DOMoveY(targetPole.stackPosition.position.y + (targetPole.GetDonutCount() * targetPole.donutHeight), moveDuration))
                    .OnComplete(() =>
                    {
                        targetPole.StackDonut(donut);
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
            pole.ClearDonuts(); // Pole'daki mevcut donut'ları temizle
        }

        foreach (var pair in initialDonutStates)
        {
            Donut donut = pair.Key;
            Pole pole = pair.Value.Item1;
            Vector3 initialPosition = pair.Value.Item2;

            donut.transform.position = initialPosition;
            pole.StackDonut(donut); // Donut'ları ilk yerleştirildikleri pole'lara geri koy
        }

        selectedDonut = null;
        originalPole = null;
        isMoving = false;
    }
}
