using UnityEngine;
using DG.Tweening;

public interface IPoleElement
{
    GameObject GetPrefab();
    void ChangeColor(Color color);
    Tween MoveTo(Vector3 targetPosition, float duration);
}