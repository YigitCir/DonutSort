using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public List<PoleStartState> poles;

#if UNITY_EDITOR

    [ContextMenuItem("Generate","Generate")]
    public int PoleCount;
    public int DonutCount;
    
    public void Generate()
    {
        
    }
    
#endif
    
    
    
}

[System.Serializable]
public class PoleStartState
{
    public int Height = 5;
    public List<DonutType> Types;
    
}