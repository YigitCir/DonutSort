using System;
using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Helpers;
using DefaultNamespace.Pole;

public class PoleView : MonoBehaviour, IClickable
{
    private DefaultNamespace.Pole.PoleData _data;
    public Transform CenterBar;
    public event Action ViewClicked;
    
    public void InitView(DefaultNamespace.Pole.PoleData data)
    {
        _data = data;
    }
    
    public void OnClick()
    {
        ViewClicked?.Invoke();
    }

    private void OnDestroy()
    {
        ViewClicked = null;
    }
}