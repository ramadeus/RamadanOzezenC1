using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star: MonoBehaviour, ICollectible {
    //ı 
    public void OnCollected()
    {
        EventsManager.onCollectibleTaken?.Invoke(CollectibleType.star);
        gameObject.SetActive(false);
    }
}
