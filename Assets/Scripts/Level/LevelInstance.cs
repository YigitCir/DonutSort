using System;
using System.Collections.Generic;
using DefaultNamespace.Pole;

namespace DefaultNamespace.Level
{
    public class LevelInstance
    {
        public static event Action<LevelInstance> OnLevelStart;
        public static event Action<LevelInstance> OnLevelStateChange;
        public static event Action<LevelInstance> OnLevelWin;
        public static event Action<LevelInstance> OnLevelReset;
        
        public LevelInstance() { }
        
        public LevelData Data;
        public List<PoleInstance> PoleInstances;
        
        public void StartLevel()
        {
            OnLevelStart?.Invoke(this);
        }

        public void ResetLevel()
        {
            OnLevelReset?.Invoke(this);
        }

        public void CompleteLevel()
        {
            
            OnLevelWin?.Invoke(this);
        }

    }
}