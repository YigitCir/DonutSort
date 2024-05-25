using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class WinConditionChecker : MonoBehaviour
{
    public UnityEvent onWin;
    private List<Pole> poles;

    public void SetPoles(List<Pole> poles)
    {
        this.poles = poles;
    }

    public List<Pole> GetPoles()
    {
        return poles;
    }

    public void CheckWinCondition()
    {
        if (poles == null || poles.Count == 0)
        {
            Debug.LogError("No poles available for win condition check.");
            return;
        }

        Dictionary<DonutType, bool> typeWinStatus = new Dictionary<DonutType, bool>();

        
        HashSet<DonutType> currentTypesInGame = new HashSet<DonutType>();

        foreach (Pole pole in poles)
        {
            foreach (Donut donut in pole.GetDonutStack())
            {
                currentTypesInGame.Add(donut.type);
            }
        }

        foreach (DonutType type in currentTypesInGame)
        {
            typeWinStatus[type] = false;
        }

        foreach (Pole pole in poles)
        {
            if (pole.GetDonutCount() == 0)
                continue;

            Donut firstDonut = pole.PeekDonut();
            DonutType firstDonutType = firstDonut.type;
            bool allSameType = pole.GetDonutStack().All(donut => donut.type == firstDonutType);

            if (allSameType && pole.GetDonutCount() >= 3)
            {
                typeWinStatus[firstDonutType] = true;
            }
        }

        if (typeWinStatus.Values.All(status => status))
        {
            Debug.Log("You win!");
            onWin.Invoke();
        }
    }
}