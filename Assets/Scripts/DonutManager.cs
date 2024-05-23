using UnityEngine;
using System.Collections.Generic;

public class DonutManager : MonoBehaviour
{
    public GameObject donutPrefab; // Prefab of the donut
    public Transform[] spawnPoints; // Pole'ların spawn noktaları
    public GameManager gameManager; // GameManager referansı

    private Color[] colors = { Color.red, Color.blue }; // Kullanılacak iki renk

    void Start()
    {
        if (spawnPoints.Length != gameManager.poles.Count)
        {
            Debug.LogError("Spawn points count must match the number of poles.");
            return;
        }

        SpawnDonuts();
    }

    void SpawnDonuts()
    {
        List<GameObject> allDonuts = new List<GameObject>();

        // 3 kırmızı ve 3 mavi donut oluştur
        for (int i = 0; i < 3; i++)
        {
            CreateDonut(Color.red, allDonuts);
            CreateDonut(Color.blue, allDonuts);
        }

        // Donutları pole'lara rastgele dağıt
        ShuffleDonutsAndAssignToPoles(allDonuts);
    }

    void CreateDonut(Color color, List<GameObject> allDonuts)
    {
        // Donut oluştur ve listeye ekle
        GameObject donutObject = Instantiate(donutPrefab);
        Donut donut = donutObject.GetComponent<Donut>();

        if (donut == null)
        {
            Debug.LogError("Donut script not found on the instantiated prefab!");
        }
        else
        {
            donut.ChangeColor(color);
            allDonuts.Add(donutObject);
        }
    }

    void ShuffleDonutsAndAssignToPoles(List<GameObject> donuts)
    {
        System.Random rng = new System.Random();
        int n = donuts.Count;

        // Fisher-Yates shuffle algoritması ile donutları karıştır
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = donuts[k];
            donuts[k] = donuts[n];
            donuts[n] = value;
        }

        // İlk iki pole'a donutları eşit şekilde dağıt
        int poleIndex = 0;
        foreach (var donutObject in donuts)
        {
            Donut donut = donutObject.GetComponent<Donut>();
            if (donut != null)
            {
                Pole targetPole = gameManager.poles[poleIndex];
                targetPole.StackDonut(donut);
                poleIndex = (poleIndex + 1) % 2; // Sadece ilk iki pole'u kullan
            }
        }
    }
}
