using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager: MonoBehaviour {
    //ı
    [SerializeField] AudioClip[] notes;
    [SerializeField] AudioClip noComboNote;
    int lastStandingStackId;
    int currentStandingStackId;
    int comboCounter;
    private void OnEnable()
    {
        EventsManager.onInitializeGame += OnInitializeGame;
        EventsManager.onComboTry += GenerateComboSystem;
        EventsManager.onObjectSpawn += OnStackObjectSpawn;
    }
    private void OnDisable()
    {
        EventsManager.onInitializeGame -= OnInitializeGame;
        EventsManager.onComboTry -= GenerateComboSystem;
        EventsManager.onObjectSpawn -= OnStackObjectSpawn;
    }

    private void OnInitializeGame(bool obj)
    {
        comboCounter = 0; 
    }


    private void OnStackObjectSpawn(int stackId)
    {
        lastStandingStackId = currentStandingStackId;
        currentStandingStackId = stackId;
    }

    private void GenerateComboSystem(bool isSuccessful)
    {

        if(isSuccessful)
        {
            AudioManager.Instance.PlaySFX("ComboNote", notes[comboCounter]);
            ComboEffect();
            comboCounter++;
        } else
        {
            AudioManager.Instance.PlaySFX("ComboNote", noComboNote);
            comboCounter = 0;
        }
        print(comboCounter);
    }
    private void ComboEffect()
    {
        GameObject stack = ObjectPooler.Instance.GetPoolObject("Stack", lastStandingStackId);
        for(int i = 0; i < 4; i++)
        {
            GameObject particle = SpawnParticle("CoinParticle", stack);
            StartCoroutine(DeactivateParticle(particle)); 
        }
    }
    GameObject SpawnParticle(string particleKey, GameObject stack)
    {
        Vector3 target = GetRandomPosition(stack.transform);
        return ObjectPooler.Instance.SpawnFromPool(particleKey, target, Quaternion.identity);
    }
    IEnumerator DeactivateParticle(GameObject particle)
    {
        yield return new WaitForSeconds(.3f);
        particle.SetActive(false);
    }

    private Vector3 GetRandomPosition(Transform stack)
    {
        float randomX = (stack.position.x - stack.localScale.x / 2) + UnityEngine.Random.Range(0, stack.localScale.x);
        float randomZ = (stack.position.z - stack.localScale.z / 2) + UnityEngine.Random.Range(0, stack.localScale.z);
        return new Vector3(randomX, -.5f, randomZ);
    }
}
