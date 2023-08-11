using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    //ı
    public int coinCount;
    public int diamondCount;
    public int starCount;
    private void OnEnable()
    {
        EventsManager.onCollectibleTaken += OnCollectibleTaken;
        EventsManager.onGameFinished += SaveDatas;
    }

    private void OnDisable()
    {
        EventsManager.onCollectibleTaken -= OnCollectibleTaken;
        EventsManager.onGameFinished -= SaveDatas;
    }
    void OnCollectibleTaken(CollectibleType collectibleType)
    {
        switch(collectibleType)
        {
            case CollectibleType.coin:
                coinCount++;
                break;
            case CollectibleType.diamond:
                diamondCount++;
                break;
            case CollectibleType.star:
                starCount++;
                break;
            default:
                break;
        }
    }
    private void SaveDatas(bool obj)
    {
        SaveData("coin", coinCount);
        SaveData("diamond", diamondCount);
        SaveData("star", starCount);
    }
    void SaveData(string key,int value)
    {
        int previousValue = 0;
        if(PlayerPrefs.HasKey(key))
        {
            previousValue = PlayerPrefs.GetInt(key);
        }

       int totalValue = previousValue + value;
        PlayerPrefs.SetInt(key, totalValue);
    }
   
   
}
