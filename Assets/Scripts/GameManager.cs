using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public List<Pole> poles;
    public float moveDuration = 0.5f;
    public WinConditionChecker winConditionChecker;

    private Donut selectedDonut = null;
    private Pole originalPole = null; // Donut'ın ilk alındığı pole
    private bool isMoving = false; // Donut hareket ederken tıklamaları engellemek için

    public void HandlePoleClicked(Vector3 clickPosition)
    {
        if (isMoving) return; // Donut hareket ederken yeni tıklamalara izin verme

        Pole clickedPole = GetClickedPole(clickPosition);
        if (clickedPole == null)
        {
            Debug.Log("No pole found at click position");
            return;
        }

        if (selectedDonut == null)
        {
            Donut poppedDonut = clickedPole.PopDonut();
            if (poppedDonut != null)
            {
                SelectDonut(poppedDonut, clickedPole);
            }
        }
        else
        {
            if (clickedPole == originalPole)
            {
                // Donut zaten seçili ve aynı pole'a geri yerleştiriliyorsa işlemi iptal et
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
        foreach (var pole in poles)
        {
            if (Vector3.Distance(pole.transform.position, clickPosition) < 1.0f) // Adjust the threshold distance as needed
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
            }); // Move up when first selected
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
                    .Append(donut.transform.DOMoveY(targetPole.stackPosition.position.y + (targetPole.donutStack.Count * targetPole.donutHeight), moveDuration))
                    .OnComplete(() => 
                    {
                        if (targetPole != null)
                        {
                            targetPole.StackDonut(donut);
                        }
                        selectedDonut = null;
                        originalPole = null;
                        isMoving = false;
                        winConditionChecker.CheckWinCondition(); // Win condition'u kontrol et
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
                    .Append(selectedDonut.transform.DOMoveY(originalPole.stackPosition.position.y + (originalPole.donutStack.Count * originalPole.donutHeight), moveDuration))
                    .OnComplete(() => 
                    {
                        if (originalPole != null)
                        {
                            originalPole.StackDonut(selectedDonut);
                        }
                        selectedDonut = null;
                        originalPole = null;
                        isMoving = false;
                        winConditionChecker.CheckWinCondition(); // Win condition'u kontrol et
                    });
        }
    }
}
