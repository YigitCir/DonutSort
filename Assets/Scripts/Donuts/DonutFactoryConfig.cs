using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Helpers;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Config/DonutFactoryConfig")]
    public class DonutFactoryConfig : ConfigSingleton<DonutFactoryConfig>
    {
        public Vector3 SpawnOffset = Vector3.up * 5f;
        public float DonutHeight = 0.2f;
        
        
        public List<DonutAssetData> Assets;

        public DonutAssetData GetAssetByType(DonutType type)
        {
            return Assets.FirstOrDefault(x => x.Type == type);
        }
        
    }
}