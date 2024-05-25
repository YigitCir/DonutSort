using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Movement;

namespace DefaultNamespace.Pole
{
    [Serializable]
    public class PoleData
    {
        public int PoleIndex;
        public List<DonutData> Donuts; 

        public PoleData(int poleIndex, List<DonutData> donuts)
        {
            PoleIndex = poleIndex;
            Donuts = donuts;
        }

        public void AddDonut(DonutData data)
        {
            Donuts.Add(data);
            data.OnMoveEvent(MovementConfig.Instance.GetByType(MovementType.Land));
        }

        public void RemoveDonut(DonutData data)
        {
            Donuts.Remove(data);
            data.OnMoveEvent(MovementConfig.Instance.GetByType(MovementType.Rise));
        }

        public bool IsSolvedState()
        {
            if (Donuts.Count == 0) return true;
            var firstType = Donuts[0].Type;
            return Donuts.All(data => data.Type == firstType);
        }
        
        
    }
}