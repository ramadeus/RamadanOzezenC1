using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin: PooledObject, ICollectible {
    //ı 
    GameObject particle;
    public override void OnObjectSpawn()
    {
        // to clear parent's code
    }
    public void OnCollected()
    {
        EventsManager.onCollectibleTaken?.Invoke(CollectibleType.coin);
        AudioManager.Instance.PlaySFX("CollectibleCoin");
        particle = ObjectPooler.Instance.SpawnFromPool("CoinParticle", transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
