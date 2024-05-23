using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Pole : MonoBehaviour
{
    public Transform stackPosition; // Position where the donuts will stack
    public float donutHeight = 0.2f;
    public float moveDuration = 0.5f; // Duration of the movement animation

    public Stack<Donut> donutStack = new Stack<Donut>();

    [System.Serializable]
    public class DonutEvent : UnityEvent<Donut> { }

    public DonutEvent OnDonutPopped;
    public DonutEvent OnDonutStacked;

    public void StackDonut(Donut donut)
    {
        Vector3 targetPosition = stackPosition.position + Vector3.up * (donutStack.Count * donutHeight);
        donut.MoveTo(targetPosition, moveDuration);
        donutStack.Push(donut);
        OnDonutStacked.Invoke(donut);
    }

    public Donut PopDonut()
    {
        if (donutStack.Count > 0)
        {
            Donut topDonut = donutStack.Pop();
            OnDonutPopped.Invoke(topDonut);
            return topDonut;
        }
        return null;
    }
}
