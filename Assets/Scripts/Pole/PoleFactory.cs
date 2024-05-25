using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Pole
{
    public static class PoleFactory
    {

        public static PoleInstance CreatePoleInstance(int i, List<DonutData> donutData)
        {
            var view = CreatePoleView();
            var data = new PoleData(i,donutData);
            return new PoleInstance(data,view);
        }
        
        
        private static PoleView CreatePoleView()
        {
            var obj = GameObject.Instantiate(PoleFactoryConfig.Instance.PolePrefab);
            return obj.GetComponent<PoleView>();
        }
    }
}