using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond: MonoBehaviour, ICollectible {
    //ı 
    public void OnCollected()
    {
        EventsManager.onCollectibleTaken?.Invoke(CollectibleType.diamond);
        gameObject.SetActive(false);
    }
}
