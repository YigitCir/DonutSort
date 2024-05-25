using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data/Level Data")]
public class LevelData : ScriptableObject
{
    public List<PoleData> poles;
}

[System.Serializable]
public class PoleData
{
    public GameObject polePrefab;
    public Vector3 position;
    public List<DonutData> donuts;
}