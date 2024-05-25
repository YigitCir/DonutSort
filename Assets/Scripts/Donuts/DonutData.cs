using System;
using DefaultNamespace.Movement;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[Serializable]
public class DonutData
{
    public DonutType Type;
    public event Action<MovementData> MoveEvent;


    public void OnMoveEvent(MovementData obj)
    {
        MoveEvent?.Invoke(obj);
    }
}

public enum DonutType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5,
    Type6,
    Type7,
    Type8,
    Type9,
    Type10
}