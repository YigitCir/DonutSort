using DefaultNamespace.Helpers;
using DefaultNamespace.Level;
using DefaultNamespace.Movement;
using DefaultNamespace.Pole;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelLogic
    {
        public static IMove SelectedObject;
        
        public static void TryMove(PoleInstance pole)
        {
            if (SelectedObject == null && pole.Data.HasAnyDonut)
            {
                SelectedObject = pole.Data.PopDonut();
                Rise(SelectedObject, pole);
            }
            else if(SelectedObject!=null)
            {
                TravelAndLand(pole);
                pole.Data.AddDonut(SelectedObject as DonutInstance);
                SelectedObject = null;
            }
        }

        private void OnLevelStateChanged()
        {
            
        }
        
        public static void TravelAndLand(PoleInstance pole)
        {
            var obj = SelectedObject;
            Travel(obj,pole).OnComplete(()=>Land(obj,pole));
        }

        public static bool CheckLevelComplete(LevelInstance instance)
        {
            foreach (var poleInstance in instance.PoleInstances)
            {
                if (!poleInstance.Data.IsSolvedState()) return false;
            }

            return true;
        }

        static Tween Land(IMove obj, PoleInstance pole) => MovementFactory.Land(obj.GetTransform(),(pole.Data.LandPosition));
        static Tween Rise(IMove obj, PoleInstance pole) => MovementFactory.Rise(obj.GetTransform(),(pole.Data.RisePosition));
        static Tween Travel(IMove obj, PoleInstance pole) => MovementFactory.Travel(obj.GetTransform(),(pole.Data.RisePosition));
            
        public static void MoveDonutsIn(PoleInstance poleInstance)
        {
            for (var i = 0; i < poleInstance.Data.Donuts.Count; i++)
            {
                var donut = poleInstance.Data.Donuts[i];
                MovementFactory.Spawn(donut.GetTransform(),poleInstance.Data.LandPosition);
            }
        }

        public static void ClearUp(PoleInstance poleInstance)
        {
            for (var i = 0; i < poleInstance.Data.Donuts.Count; i++)
            {
                var donut = poleInstance.Data.Donuts[i];
                MovementFactory.Rise(donut.GetTransform(), poleInstance.Data.ExitPosition.Add((i * DonutFactoryConfig.Instance.DonutHeight)));
            }
        }
    }
}