using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin: MonoBehaviour, ICollectible {
    //ı 
    public void OnCollected()
    {
        EventsManager.onCollectibleTaken?.Invoke(CollectibleType.coin);
        gameObject.SetActive(false); 
    }
}
