using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace.Level;
using DG.Tweening;
using UI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelLoader Loader;
    
    void Start()
    {
        Loader.LoadCurrent();
        WinCanvas.Instance.Hide(true);
    }
    
}
