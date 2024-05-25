using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class LevelGenerator : MonoBehaviour
{
    private List<GameObject> generatedPoles = new List<GameObject>();
    public float dropHeight = 5f; // Donutların düşeceği yükseklik
    public float dropDuration = 0.5f; // Düşme süresi
    public float dropInterval = 0.3f; // Donutların teker teker düşme aralığı

    public void GenerateLevel(LevelData levelData)
    {
        ClearPreviousLevel();

        int donutCount = 0;
        foreach (var poleData in levelData.poles)
        {
            if (poleData.polePrefab == null)
            {
                Debug.LogError("Pole prefab is null. Please assign a valid prefab in LevelData.");
                continue;
            }

            GameObject pole = Instantiate(poleData.polePrefab, poleData.position, Quaternion.identity);
            Pole poleComponent = pole.GetComponent<Pole>();

            if (poleComponent == null)
            {
                Debug.LogError("Pole prefab does not have a Pole component.");
                continue;
            }

            foreach (var donutData in poleData.donuts)
            {
                if (donutData.GetPrefab() == null)
                {
                    Debug.LogError("Donut prefab is null. Please assign a valid prefab in DonutData.");
                    continue;
                }

                // Donut'ları yukarıdan spawn et ve aşağı düşme animasyonu uygula
                Vector3 startPosition = poleComponent.stackPosition.position + Vector3.up * dropHeight;
                Vector3 targetPosition = poleComponent.stackPosition.position + Vector3.up * (poleComponent.GetDonutCount() * poleComponent.donutHeight);

                GameObject donutObject = Instantiate(donutData.GetPrefab(), startPosition, Quaternion.identity);
                Donut donutComponent = donutObject.GetComponent<Donut>();
                if (donutComponent == null)
                {
                    Debug.LogError("Donut prefab does not have a Donut component.");
                    continue;
                }

                donutComponent.type = donutData.type;
                float dropDelay = donutCount * dropInterval;

                // Donut'ı hedef pozisyona doğru hareket ettir
                donutComponent.transform.DOMove(targetPosition, dropDuration)
                    .SetDelay(dropDelay)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() => 
                    {
                        poleComponent.StackDonut(donutComponent);
                    });

                donutCount++;
            }

            generatedPoles.Add(pole);
        }
    }

    public List<GameObject> GetGeneratedPoles()
    {
        return generatedPoles;
    }

    public void ClearPreviousLevel()
    {
        foreach (var pole in generatedPoles)
        {
            Pole poleComponent = pole.GetComponent<Pole>();
            if (poleComponent != null)
            {
                poleComponent.ClearDonuts(); // Pole'daki mevcut donut'ları temizle
            }
            Destroy(pole);
        }
        generatedPoles.Clear();
    }
}
