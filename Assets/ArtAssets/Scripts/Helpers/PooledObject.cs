using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public   class PooledObject : MonoBehaviour
{
    //ı
    public int stackId;
    public virtual  void OnObjectSpawn()
    {
        EventsManager.onObjectSpawn?.Invoke(stackId);
    } 
    public   void InitializeId(int id)
    { 
        stackId = id;
    }
}
