using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Helpers;
using DefaultNamespace.Movement;
using UnityEngine;

namespace DefaultNamespace.Pole
{
    [Serializable]
    public class PoleData
    {
        public int PoleIndex;
        public int Height;
        public Vector3 BasePosition;
        public List<DonutInstance> Donuts;

        public Vector3 RisePosition => BasePosition.Add(y: Height);
        public Vector3 LandPosition => BasePosition.Add(y: Donuts.Count * DonutFactoryConfig.Instance.DonutHeight);

        public Vector3 ExitPosition => BasePosition.Add(y: 15f);

        public PoleData(int poleIndex, List<DonutInstance> donuts)
        {
            PoleIndex = poleIndex;
            Donuts = donuts;
        }

        public void AddDonut(DonutInstance data)
        {
            Donuts.Add(data);
        }

        public DonutInstance PopDonut()
        {
            var data = Donuts[^1];
            Donuts.Remove(data);
            return data;
        }

        public bool HasAnyDonut => Donuts.Count != 0;

        public bool IsSolvedState()
        {
            if (!HasAnyDonut) return true;
            var firstType = Donuts[0].Data.Type;
            return Donuts.All(instance => instance.Data.Type == firstType);
        }
        
        
    }
}