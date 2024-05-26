using System;
using System.Collections.Generic;
using DefaultNamespace.Helpers;
using UnityEngine;

namespace DefaultNamespace.Level
{
    [CreateAssetMenu(menuName = "Config/LevelsConfig")]
    public class LevelsConfig : ConfigSingleton<LevelsConfig>
    {
        public List<LevelData> Levels;

        public LevelData GetCurrentLevel()
        {
            return Levels[LevelSaveData.LevelIndex & Levels.Count];
        }
        
    }
    
    
    public static class LevelSaveData
    {
        private static string KEY_Level_Index = typeof(LevelSaveData).ToString();
        public static int LevelIndex
        {
            get =>
                PlayerPrefs.GetInt(KEY_Level_Index, 0);
            set =>
                PlayerPrefs.SetInt(KEY_Level_Index,value);
        }
    }
    
}