using UnityEngine;
using DG.Tweening;

public class Donut : MonoBehaviour, IPoleElement
{
    public DonutType type;
    [SerializeField]
    private Renderer donutRenderer;

    void Start()
    {
        donutRenderer = GetComponent<Renderer>();
    }

    public void ChangeColor(Color newColor)
    {
        if (donutRenderer != null)
        {
            donutRenderer.material.color = newColor;
        }
        else
        {
            Debug.LogError("Renderer component not found!");
        }
    }

    public Tween MoveTo(Vector3 targetPosition, float duration)
    {
        return transform.DOMove(targetPosition, duration).SetEase(Ease.InOutCirc);
    }

    public GameObject GetPrefab()
    {
        return gameObject;
    }
}