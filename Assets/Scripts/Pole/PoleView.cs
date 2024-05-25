using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Pole;

public class PoleView : MonoBehaviour
{
    private DefaultNamespace.Pole.PoleData _data;
    
    public Transform stackPosition;
    public float moveDuration = 0.5f;

    private Stack<DonutView> donutStack = new Stack<DonutView>();

    public void RegisterData(DefaultNamespace.Pole.PoleData data)
    {
        _data = data;
        transform.position = PoleFactoryConfig.Instance.GetPolePosition(data.PoleIndex);
    }

    public void StackDonut(DonutView donutView)
    {
        Vector3 targetPosition = stackPosition.position + Vector3.up * (donutStack.Count * DonutFactoryConfig.Instance.DonutHeight);
        donutView.MoveTo(targetPosition, moveDuration);
        donutStack.Push(donutView);
    }

    public DonutView PopDonut()
    {
        if (donutStack.Count > 0)
        {
            return donutStack.Pop();
        }
        return null;
    }

    public DonutView PeekDonut()
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

    public Stack<DonutView> GetDonutStack()
    {
        return donutStack;
    }

    public void ClearDonuts()
    {
        while (donutStack.Count > 0)
        {
            DonutView donutView = donutStack.Pop();
            Destroy(donutView.gameObject);
        }
    }
}