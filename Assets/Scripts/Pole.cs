using UnityEngine;
using System.Collections.Generic;

public class Pole : MonoBehaviour
{
    public Transform stackPosition;
    public float donutHeight = 0.2f;
    public float moveDuration = 0.5f;

    private Stack<Donut> donutStack = new Stack<Donut>();

    public void StackDonut(Donut donut)
    {
        Vector3 targetPosition = stackPosition.position + Vector3.up * (donutStack.Count * donutHeight);
        donut.MoveTo(targetPosition, moveDuration);
        donutStack.Push(donut);
    }

    public Donut PopDonut()
    {
        if (donutStack.Count > 0)
        {
            return donutStack.Pop();
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

    public Stack<Donut> GetDonutStack()
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
    }
