using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Donut Data", menuName = "Pole Elements/Donut Data")]
public class DonutData : ScriptableObject
{
    public List<DonutPrefabData> donutPrefabs;

    public GameObject GetPrefab(DonutType type)
    {
        foreach (var donutPrefab in donutPrefabs)
        {
            if (donutPrefab.type == type)
            {
                return donutPrefab.prefab;
            }
        }
        return null;
    }

    public Color GetColor(DonutType type)
    {
        foreach (var donutPrefab in donutPrefabs)
        {
            if (donutPrefab.type == type)
            {
                return donutPrefab.color;
            }
        }
        return Color.white;
    }
}

[System.Serializable]
public class DonutPrefabData
{
    public DonutType type;
    public GameObject prefab;
    public Color color;
}

public enum DonutType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5
}