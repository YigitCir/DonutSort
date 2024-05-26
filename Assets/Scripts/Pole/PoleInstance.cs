using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Pole
{
    public class PoleInstance 
    {
        public PoleData Data;
        public PoleView View;

        private PoleInstance(PoleData data, PoleView view)
        {
            Data = data;
            View = view;
        }

        public static PoleInstance CreatePoleInstance(int index, int height = 5, List<DonutInstance> donutList = null)
        {
            var view = CreatePoleView();
            var data = new PoleData(index,donutList)
            {
                Height = height,
                BasePosition = PoleFactoryConfig.Instance.GetPolePosition(index),
                Donuts = donutList
            };
            view.InitView(data);
            var instance = new PoleInstance(data,view);
            instance.View.ViewClicked += (() => LevelLogic.TryMove(instance));
            return instance;
        }
        
        private static PoleView CreatePoleView()
        {
            var obj = GameObject.Instantiate(PoleFactoryConfig.Instance.PolePrefab);
            return obj.GetComponent<PoleView>();
        }

       
    }
}