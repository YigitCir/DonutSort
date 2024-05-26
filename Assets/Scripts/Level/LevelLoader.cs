using System;
using System.Collections.Generic;
using DefaultNamespace.Pole;
using UI;
using UnityEngine;

namespace DefaultNamespace.Level
{
    public class LevelLoader : MonoBehaviour
    {
        public Transform LevelParent;

        private void OnEnable()
        {
            LevelInstance.OnLevelStart += OnLevelStart;
            LevelInstance.OnLevelReset += OnLevelReset;
            LevelInstance.OnLevelWin += OnLevelWin;
        }

        private void OnDisable()
        {
            LevelInstance.OnLevelStart -= OnLevelStart;
            LevelInstance.OnLevelReset -= OnLevelReset;
            LevelInstance.OnLevelWin -= OnLevelWin;
        }

        public void LoadCurrent()
        {
            Load(LevelsConfig.Instance.GetCurrentLevel());
        }

        public void Load(LevelData data)
        {
            Clear();
            LevelParent = new GameObject($"-{data.name}- Level Parent").transform;
            LevelParent.SetParent(transform);
            
            int index = 0;
            var poles = new List<PoleInstance>();
            foreach (var poleStart in data.poles)
            {
                var donuts = CreatePoleDonuts(poleStart);
                var pole = CreateLevelPole(index, poleStart, donuts);
                poles.Add(pole);
                index++;
            }
            
            var level = new LevelInstance()
            {
                Data = data,
                PoleInstances = poles,
            };
            
            
            
            level.StartLevel();
        }
        void OnLevelReset(LevelInstance obj) => LoadCurrent();
        void OnLevelWin(LevelInstance x)
        {
            for (var i = 0; i < x.PoleInstances.Count; i++)
            {
                var pole = x.PoleInstances[i];
                LevelLogic.ClearUp(pole);
            }
            
            LevelSaveData.LevelIndex++;
            WinCanvas.InvokeSetActiveWinMessage(true);
        }

        void OnLevelStart(LevelInstance levelInstance)
        {
            for (var i = 0; i < levelInstance.PoleInstances.Count; i++)
            {
                var pole = levelInstance.PoleInstances[i];
                LevelLogic.MoveDonutsIn(pole);
            }
        }

        private PoleInstance CreateLevelPole(int index, PoleStartState poleStart, List<DonutInstance> donuts)
        {
            var polesParent = new GameObject("").transform;
            polesParent.SetParent(LevelParent);
            var instance = PoleInstance.CreatePoleInstance(index, poleStart.Height, donuts);
            instance.View.transform.SetParent(polesParent);
            return instance;
        }

        private List<DonutInstance> CreatePoleDonuts(PoleStartState poleStart)
        {
            var donutsParent = new GameObject("DonutsParent").transform;
            donutsParent.SetParent(LevelParent);
            List<DonutInstance> donuts = new List<DonutInstance>();
            for (var i = 0; i < poleStart.Types.Count; i++)
            {
                var donut = DonutInstance.CreateDonutInstance(poleStart.Types[i]);
                donut.GetTransform().SetParent(donutsParent);
            }

            return donuts;
        }

        public void Clear()
        {
            if(LevelParent) GameObject.Destroy(LevelParent);
        }
        
        
    }
}