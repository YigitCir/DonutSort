using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Pole;
using DG.Tweening;

public class LevelGenerator 
{
    private List<GameObject> generatedPoles = new List<GameObject>();
    public float dropHeight = 5f; 
    public float dropDuration = 0.5f; 
    public float dropInterval = 0.3f;

    public List<DonutInstance> DonutInstances;
    public List<PoleInstance> PoleInstances;

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

            GameObject pole = GameObject.Instantiate(poleData.polePrefab, poleData.position, Quaternion.identity);
            PoleView poleViewComponent = pole.GetComponent<PoleView>();

            if (poleViewComponent == null)
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

                
                Vector3 startPosition = poleViewComponent.stackPosition.position + Vector3.up * dropHeight;
                Vector3 targetPosition = poleViewComponent.stackPosition.position + Vector3.up * (poleViewComponent.GetDonutCount() * poleViewComponent.donutHeight);

                GameObject donutObject = GameObject.Instantiate(donutData., startPosition, Quaternion.identity);
                DonutView donutViewComponent = donutObject.GetComponent<DonutView>();
                if (donutViewComponent == null)
                {
                    Debug.LogError("Donut prefab does not have a Donut component.");
                    continue;
                }

                donutViewComponent.type = donutData.type;
                float dropDelay = donutCount * dropInterval;

                
                donutViewComponent.transform.DOMove(targetPosition, dropDuration)
                    .SetDelay(dropDelay)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() => 
                    {
                        poleViewComponent.StackDonut(donutViewComponent);
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
            PoleView poleViewComponent = pole.GetComponent<PoleView>();
            if (poleViewComponent != null)
            {
                poleViewComponent.ClearDonuts(); 
            }
            //Be careful about what you are destroying. pole would destroy the component only, while pole.gameobject destroys the entire gameobjects
            GameObject.Destroy(pole.gameObject);
            //GameObject.Destroy(pole);
        }
        generatedPoles.Clear();


        foreach (var pole in PoleInstances)
        {
            pole.Clear();
        }
        
    }
}
