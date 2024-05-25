using DefaultNamespace.Helpers;
using UnityEngine;

namespace DefaultNamespace.Pole
{
    [CreateAssetMenu(menuName = "Config/PoleFactoryConfig")]
    public class PoleFactoryConfig : ConfigSingleton<PoleFactoryConfig>
    {
        public GameObject PolePrefab;
        public float PoleSpacing = 10;
        public Vector3 StartPosition = Vector3.zero;
        public Vector3 OffsetDirection;

        public Vector3 GetPolePosition(int index)
        {
            return StartPosition + OffsetDirection * PoleSpacing * index;
        }

    }
}