using UnityEngine;
using System.Collections.Generic;

public class DonutManager : MonoBehaviour
{
    public DonutData donutData;
    private List<Donut> allDonuts = new List<Donut>();

    public Donut CreateDonut(DonutType type, Vector3 position, Transform parent)
    {
        GameObject prefab = donutData.GetPrefab(type);
        if (prefab == null)
        {
            Debug.LogError("Donut prefab is not assigned for type: " + type);
            return null;
        }

        GameObject donutObject = Instantiate(prefab, position, Quaternion.identity);
        donutObject.transform.SetParent(parent);
        donutObject.transform.localScale = new Vector3(8f, 0.4f, 8f);
        Donut donut = donutObject.GetComponent<Donut>();
        if (donut != null)
        {
            donut.type = type; // Set the donut type
            Renderer renderer = donutObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = donutData.GetColor(type);
            }
            allDonuts.Add(donut);
        }
        else
        {
            Debug.LogError("Donut script not found on the instantiated prefab!");
        }
        return donut;
    }

    public Color GetColor(DonutType type)
    {
        return donutData.GetColor(type);
    }

    public void ClearAllDonuts()
    {
        foreach (var donut in allDonuts)
        {
            if (donut != null)
            {
                Destroy(donut.GetPrefab());
            }
        }
        allDonuts.Clear();
    }
}