using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class WinConditionChecker : MonoBehaviour
{
    public List<Pole> poles;
    public UnityEvent onWin; 

    public void CheckWinCondition()
    {
        bool redDonutsWon = false;
        bool blueDonutsWon = false;

        foreach (Pole pole in poles)
        {
            if (pole.donutStack.Count == 0)
                continue;

            Color firstDonutColor = pole.donutStack.Peek().GetComponent<Renderer>().material.color;
            bool allSameColor = pole.donutStack.All(donut => donut.GetComponent<Renderer>().material.color == firstDonutColor);

            if (allSameColor)
            {
                if (firstDonutColor == Color.red && pole.donutStack.Count == 3)
                {
                    redDonutsWon = true;
                }
                else if (firstDonutColor == Color.blue && pole.donutStack.Count == 3)
                {
                    blueDonutsWon = true;
                }
            }
        }

        if (redDonutsWon && blueDonutsWon)
        {
            Debug.Log("You win!");
            onWin.Invoke(); 
        }
    }
}