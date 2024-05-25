using DefaultNamespace.Movement;
using UnityEngine;
using DG.Tweening;

public class DonutView : MonoBehaviour
{
    public DonutType type;
    private DonutData _data;

    public void RegisterData(DonutData data)
    {
        _data = data;
        data.MoveEvent += OnMove;
    }

    private void OnMove(MovementData moveData)
    {
        switch (moveData.Type)
        {
            
        }
    }
    

}