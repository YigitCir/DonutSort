using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data/Level Data")]
public class LevelData : ScriptableObject
{
    public List<PoleStartState> poles;
}

[System.Serializable]
public class PoleStartState
{
    public List<DonutType> Types;
}