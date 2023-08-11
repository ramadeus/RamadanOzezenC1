using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterRoadAnimator: MonoBehaviour {
    //ı 
    ObjectPooler objectPooler;
   
    private void OnEnable   ()
    { 
        objectPooler = ObjectPooler.Instance;
  
    }
    public void SpawnFirstRoadWithAnim(int roadCount, float firstZpos)
    {
        for(int i = 0; i < roadCount; i++)
        {
            Transform stackPrefab = objectPooler.GetPoolObjectPrefab("Stack").transform;
            
            float roadHeight = stackPrefab.localScale.z;
            float direction = i % 2 == 0 ? 1 : -1;
            
            Vector3 targetPos = new Vector3(0, -.5f, firstZpos + (i * roadHeight));
            Vector3 spawnPos = new Vector3(targetPos.x + (direction * 7), targetPos.y -3, targetPos.z);
             
            Quaternion spawnRot = Quaternion.Euler(new Vector3(0,0, direction * -45));

            GameObject spawnedObject = objectPooler.SpawnFromPool("Stack", spawnPos, spawnRot, stackPrefab.localScale);

            spawnedObject.transform.DOMove(targetPos, 2);
            spawnedObject.transform.DORotateQuaternion(Quaternion.identity, 2);
        }
    }
}
