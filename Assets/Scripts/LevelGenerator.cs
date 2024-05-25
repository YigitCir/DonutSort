using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public LevelData levelData;
    public GameObject polePrefab;
    public DonutManager donutManager;

    private List<GameObject> generatedPoles = new List<GameObject>();

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        ClearPreviousLevel();

        foreach (var poleData in levelData.poles)
        {
            Vector3 position = poleData.position;
            GameObject poleObject = Instantiate(polePrefab, position, Quaternion.identity);
            generatedPoles.Add(poleObject);

            Pole pole = poleObject.GetComponent<Pole>();
            if (pole == null)
            {
                Debug.LogError("Pole script not found on the instantiated pole prefab!");
                continue;
            }

            pole.stackPosition = poleObject.transform.Find("StackPosition");
            if (pole.stackPosition == null)
            {
                Debug.LogError("StackPosition not found on the pole prefab!");
                continue;
            }

            foreach (var donutSpawnData in poleData.donuts)
            {
                for (int i = 0; i < donutSpawnData.count; i++)
                {
                    Vector3 dropPosition = pole.stackPosition.position + Vector3.up * 5f;
                    Donut donut = donutManager.CreateDonut(donutSpawnData.type, dropPosition, pole.stackPosition);
                    if (donut != null)
                    {
                        donut.ChangeColor(donutManager.GetColor(donutSpawnData.type));
                        pole.StackDonut(donut);
                    }
                }
            }
        }
    }

    private void ClearPreviousLevel()
    {
        foreach (var pole in generatedPoles)
        {
            Destroy(pole);
        }

        generatedPoles.Clear();
    }

    public List<GameObject> GetGeneratedPoles()
    {
        return generatedPoles;
    }
}
