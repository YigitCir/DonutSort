using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Donut Data", menuName = "Pole Elements/Donut Data")]
public class DonutData : ScriptableObject, IPoleElement
{
    public GameObject prefab;
    public DonutType type;

    public GameObject GetPrefab()
    {
        return prefab;
    }

    public Tween MoveTo(Vector3 targetPosition, float duration)
    {
        if (prefab != null)
        {
            return prefab.transform.DOMove(targetPosition, duration).SetEase(Ease.InOutCirc);
        }
        return null;
    }
}

public enum DonutType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5,
    Type6,
    Type7,
    Type8,
    Type9,
    Type10
}