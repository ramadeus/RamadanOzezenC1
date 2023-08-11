using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner: MonoBehaviour {
    //ı
    [SerializeField] int spawnRatio = 3;
    int ignoreFirstThree;
    bool lastSpawn;
    private void OnEnable()
    {
        EventsManager.onObjectSpawn += GenerateCollectible;
        EventsManager.onGameStart += OnGameStart;
        EventsManager.onLastStackSpawn += OnLastStackSpawn;
    }



    private void OnDisable()
    {
        EventsManager.onObjectSpawn -= GenerateCollectible;
        EventsManager.onLastStackSpawn -= OnLastStackSpawn;
        EventsManager.onGameStart-= OnGameStart;
    }

    private void OnLastStackSpawn()
    {
        lastSpawn = true;
    }

    private void OnGameStart ()
    {
        ignoreFirstThree = 0;
        lastSpawn = false;
    }

    private void GenerateCollectible(int stackId)
    {
        if(lastSpawn)
        {
            return;
        }
        if(ignoreFirstThree < 3)
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

        float direction = UnityEngine.Random.Range(0, 1) == 0 ? 1 : -1;

        Vector3 targetPos = new Vector3(responsibleStack.position.x, 0, responsibleStack.position.z);
        Vector3 spawnPos = new Vector3(targetPos.x + (direction * 7), targetPos.y + 7, targetPos.z);

        GameObject spawnedObject = ObjectPooler.Instance.SpawnFromPool("Star", spawnPos, Quaternion.identity);
         
        spawnedObject.transform.DOMove(targetPos, 1); 
    }

    private void SpawnDiamond(int stackId)
    {
        Transform responsibleStack = ObjectPooler.Instance.GetPoolObject("Stack", stackId).transform;
        float randomLocator = UnityEngine.Random.Range(0f, responsibleStack.localScale.z);
        float targetZ = (responsibleStack.position.z - responsibleStack.localScale.z / 2) + randomLocator; 
        float direction = UnityEngine.Random.Range(0, 1) == 0 ? 1 : -1;

        Vector3 targetPos = new Vector3(responsibleStack.position.x, 0, targetZ);
        Vector3 spawnPos = new Vector3(targetPos.x + (direction * 7), targetPos.y + 7, targetPos.z);

        GameObject spawnedObject = ObjectPooler.Instance.SpawnFromPool("Diamond", spawnPos, Quaternion.identity);
         
        spawnedObject.transform.DOMove(targetPos, 1);
    }

    private void SpawnCoin(int stackId)
    {
        Transform responsibleStack = ObjectPooler.Instance.GetPoolObject("Stack", stackId).transform; 

        float direction = UnityEngine.Random.Range(0, 1) == 0 ? 1 : -1; 
        int randomSpawnCount = UnityEngine.Random.Range(1, 6);
        float distanceBetweenCoins = responsibleStack.localScale.z / randomSpawnCount;

        for(int i = 0; i < randomSpawnCount; i++)
        {
            float targetZ = (responsibleStack.position.z - responsibleStack.localScale.z / 2) + (i * distanceBetweenCoins);

            Vector3 targetPos = new Vector3(responsibleStack.position.x, 0, targetZ);
            Vector3 spawnPos = new Vector3(targetPos.x + (direction * 7), targetPos.y + 7, targetPos.z);

            GameObject spawnedObject = ObjectPooler.Instance.SpawnFromPool("Coin", spawnPos, Quaternion.identity); 
            spawnedObject.transform.DOMove(targetPos, 1);
        }
    }
}
