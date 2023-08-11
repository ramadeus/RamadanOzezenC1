using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond: PooledObject, ICollectible {
    //ı 
    GameObject particle;
    public override void OnObjectSpawn()
    {
         //to clear parent's code
    }
    public void OnCollected()
    {
        EventsManager.onCollectibleTaken?.Invoke(CollectibleType.diamond);
        AudioManager.Instance.PlaySFX("CollectibleDiamond");
        particle = ObjectPooler.Instance.SpawnFromPool("DiamondParticle", transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
