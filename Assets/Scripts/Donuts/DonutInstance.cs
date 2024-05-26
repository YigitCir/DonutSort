using DefaultNamespace.Movement;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class DonutInstance: IMove
    {
        private static DonutFactoryConfig Config => DonutFactoryConfig.Instance;
        public DonutView View;
        public DonutData Data;

        public Transform GetTransform() => View.transform;

        private DonutInstance(DonutData data, DonutView view)
        {
            View = view;
            Data = data;
            view.RegisterData(data);
        }

        private static DonutView CreateView(DonutType type)
        {
            var obj = GameObject.Instantiate(Config.GetAssetByType(type).View);
            return obj.GetComponent<DonutView>();
        }
        
        public static DonutInstance CreateDonutInstance(DonutType type)
        {
            var view = CreateView(type);
            var data = new DonutData()
            {
                Type = type
            };
            return new DonutInstance(data,view);
        }
        
    }
}