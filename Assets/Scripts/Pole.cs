using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pole : MonoBehaviour
{
    public Transform stackPosition; 
    public float donutHeight = 0.2f;
    public float moveDuration = 0.5f; 

    private Stack<Donut> donutStack = new Stack<Donut>();

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

    public Donut PeekDonut()
    {
        if (donutStack.Count > 0)
        {
            return donutStack.Peek();
        }
        return null;
    }

    public int GetDonutCount()
    {
        return donutStack.Count;
    }

    public IEnumerable<Donut> GetDonutStack()
    {
        return donutStack;
    }

    public void ClearDonuts()
    {
        while (donutStack.Count > 0)
        {
            Donut donut = donutStack.Pop();
            Destroy(donut.gameObject);
        }
    }

    public void RemoveDonut(Donut donut)
    {
        if (donutStack.Contains(donut))
        {
            List<Donut> tempStack = new List<Donut>(donutStack);
            tempStack.Remove(donut);
            donutStack.Clear();
            tempStack.Reverse();
            foreach (var d in tempStack)
            {
                donutStack.Push(d);
            }
        }
    }
}