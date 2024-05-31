using UnityEngine;
using DG.Tweening;

public class Donut : MonoBehaviour, IPoleElement
{
    public DonutType type;
    public Tween MoveTo(Vector3 targetPosition, float duration)
    {
        return transform.DOMove(targetPosition, duration).SetEase(Ease.InOutCirc);
    }

    public GameObject GetPrefab()
    {
        return gameObject;
    }
}