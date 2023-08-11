using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star: PooledObject, ICollectible {
    //ı 
    GameObject particle;
    public override void OnObjectSpawn()
    {
    }
    public void OnCollected()
    {
        EventsManager.onCollectibleTaken?.Invoke(CollectibleType.star);
        AudioManager.Instance.PlaySFX("CollectibleStar");
        particle = ObjectPooler.Instance.SpawnFromPool("StarParticle", transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
