using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Helpers;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace.Movement
{
    [CreateAssetMenu(menuName = "Config/MovemetConfig")]
    public class MovementConfig : ConfigSingleton<MovementConfig>
    {
        public List<MovementData> MovementDatum;

        private Dictionary<MovementType, MovementData> _dict;

        public MovementData GetByType(MovementType type)
        {
            _dict ??= new Dictionary<MovementType, MovementData>();
            if (_dict.ContainsKey(type))
            {
                return _dict[type];
            }
            else
            {
                var data = MovementDatum.FirstOrDefault(x => x.Type == type);
                if (data != null)
                {
                    _dict.Add(type,data);
                }

                return data;
            }
        }

    }

    [Serializable]
    public class MovementData
    {
        public MovementType Type;
        public Ease BaseEase;
        public float Duration;

    }

    public static class MovementFactory
    {
        public static Tween Rise(Transform T, Vector3 targetPos)
        {
            var data = MovementConfig.Instance.GetByType(MovementType.Rise);
            return T.DOMove(targetPos, data.Duration).SetEase(data.BaseEase);
        }
        
        public static Tween Land(Transform T, Vector3 targetPos)
        {
            var data = MovementConfig.Instance.GetByType(MovementType.Land);
            return T.DOMove(targetPos, data.Duration).SetEase(data.BaseEase);
        }
        
        public static Tween MoveBetween(Transform T, Vector3 targetPos)
        {
            var data = MovementConfig.Instance.GetByType(MovementType.MoveBetween);
            return T.DOMove(targetPos, data.Duration).SetEase(data.BaseEase);
        }
        
        public static Tween Spin(Transform T, Vector3 targetRotation)
        {
            var data = MovementConfig.Instance.GetByType(MovementType.Spin);
            return T.DORotate(targetRotation, data.Duration).SetEase(data.BaseEase);
        }
        
        public static Tween Spawn(Transform T, Vector3 targetPos)
        {
            var data = MovementConfig.Instance.GetByType(MovementType.Spawn);
            return T.DOMove(targetPos, data.Duration).SetEase(data.BaseEase);
        }
    }

    public enum MovementType
    {
        Rise, Land, MoveBetween, Spin, Spawn
    }
}