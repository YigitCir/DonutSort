using System;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public static class DonutUtil
    {
        public static DonutType GetRandomType()
        {
            var values = Enum.GetNames(typeof(DonutType));
            return (DonutType)Random.Range(0,values.Length);
        }
    }
}