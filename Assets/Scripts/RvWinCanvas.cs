using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class RvWinCanvas : WinCanvas
{
    public Button RVButton;

    protected override void Start()
    {
        base.Start();
        winText.text = "Watch RV To Earn more!";
        RVButton.onClick.AddListener(RVButtonPressed);
    }
    
    protected override void OnDestroy()
    {
        RVButton.onClick.RemoveListener(RVButtonPressed);
        base.OnDestroy();
    }
    
    private void RVButtonPressed()
    {
        var isRVReady = true; //from ad provider
        if (isRVReady)
        {
            //example sdk
            // VoodooSauce.ShowRewardedVideo(OnRvShowComplete);
        }
    }

    //RV callback
    void OnRVShowComplete(bool isSuccess)
    {
        if (isSuccess)
        {
            //SendRewardEvent
            
        }
        else
        {
            OnButtonPressed();
        }
    }

   
}