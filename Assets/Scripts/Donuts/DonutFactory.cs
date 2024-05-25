using UnityEngine;

namespace DefaultNamespace
{
    public static class DonutFactory
    {
        private static DonutFactoryConfig Config => DonutFactoryConfig.Instance;


        public static DonutView CreateView(DonutType type)
        {
            var obj = GameObject.Instantiate(Config.GetAssetByType(type).View);
            return obj.GetComponent<DonutView>();
        }
        
        public static DonutInstance CreateInstance(DonutType type)
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