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
    public Vector3 position;
    public List<DonutSpawnData> donuts;
}

[System.Serializable]
public class DonutSpawnData
{
    public DonutType type;
    public int count;
}