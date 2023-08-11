using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner: MonoBehaviour {
    //ı
    [SerializeField] int spawnRatio = 3;
    int ignoreFirstThree;
    private void OnEnable()
    {
        EventsManager.onObjectSpawn += GenerateCollectible;
        EventsManager.onInitializeGame += Initialize;
    }



    private void OnDisable()
    {
        EventsManager.onObjectSpawn -= GenerateCollectible;
        EventsManager.onInitializeGame -= Initialize;
    }
    private void Initialize(bool obj)
    {
        ignoreFirstThree = 0;
    }

    private void GenerateCollectible(int stackId)
    {
        if(ignoreFirstThree<3)
        {
            ignoreFirstThree++;
            return;
        }
        bool tryToSpawn = GetAPossibility(spawnRatio);
        if(!tryToSpawn)
        {
            return;
        }
        int spawnAType = UnityEngine.Random.Range(0, 3);
        switch((CollectibleType)spawnAType)
        {
            case CollectibleType.coin:
                SpawnCoin(stackId);
                break;
            case CollectibleType.diamond:
                SpawnDiamond(stackId);
                break;
            case CollectibleType.star:
                SpawnStar(stackId);
                break;
            default:
                break;
        }
    }

    private bool GetAPossibility(int possibilityRatio)
    {
        return UnityEngine.Random.Range(0, possibilityRatio) == 0;
    }

    private void SpawnStar(int stackId)
    {
        Transform responsibleStack = ObjectPooler.Instance.GetPoolObject("Stack", stackId).transform;
        Vector3 spawnPos = new Vector3(responsibleStack.position.x, 0, responsibleStack.position.z);
        ObjectPooler.Instance.SpawnFromPool("Star", spawnPos, Quaternion.identity);
    }

    private void SpawnDiamond(int stackId)
    {
        Transform responsibleStack = ObjectPooler.Instance.GetPoolObject("Stack", stackId).transform;
        float randomLocator = UnityEngine.Random.Range(0f, responsibleStack.localScale.z);
        float targetZ = (responsibleStack.position.z - responsibleStack.localScale.z / 2) + randomLocator;
        Vector3 spawnPos = new Vector3(responsibleStack.position.x, 0, targetZ);
        ObjectPooler.Instance.SpawnFromPool("Diamond", spawnPos, Quaternion.identity);
    }

    private void SpawnCoin(int stackId)
    {
        Transform responsibleStack = ObjectPooler.Instance.GetPoolObject("Stack", stackId).transform;
        int randomSpawnCount = UnityEngine.Random.Range(1, 6);
        float distanceBetweenCoins = responsibleStack.localScale.z / randomSpawnCount;
        for(int i = 0; i < randomSpawnCount; i++)
        {
            float targetZ = (responsibleStack.position.z - responsibleStack.localScale.z / 2) + (i * distanceBetweenCoins);
            Vector3 spawnPos = new Vector3(responsibleStack.position.x, 0, targetZ);
            ObjectPooler.Instance.SpawnFromPool("Coin", spawnPos, Quaternion.identity);
        }
    }
}
