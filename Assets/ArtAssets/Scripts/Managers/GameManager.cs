using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {
    //ı  
    [SerializeField] Transform finishTarget1;
    [SerializeField] Transform finishTarget2; 
    Transform currentFinishTarget;
    Transform oldFinishTarget; 
    int distanceLevel = 0;
    bool changeFinishTarget;
     
    private void OnEnable()
    {
        EventsManager.onInitializeGame += OnInitializeTheGame;
    }
    private void OnDisable()
    {
        EventsManager.onInitializeGame -= OnInitializeTheGame;
    }
    private void OnInitializeTheGame(bool isFirstLevel)
    {
        StartCoroutine(StartTheGame(3, isFirstLevel));
    }
    private IEnumerator StartTheGame(float timer, bool isFirstLevel)
    {
        yield return new WaitForSeconds(timer);
        InitializeFinishTarget(isFirstLevel);
        EventsManager.onGameStart?.Invoke();
    }
    private void InitializeFinishTarget(bool isFirstLevel)
    {
        float lastFinishTargetZ = 0;
        float scaleMultiplier = 2.5f;
        float stackHeight = 3;

        if(!isFirstLevel)
        {
            lastFinishTargetZ = currentFinishTarget.position.z;
            scaleMultiplier = 2;
        }
        if(changeFinishTarget)
        {
            currentFinishTarget = finishTarget1;
            oldFinishTarget = finishTarget2;
        } else
        {
            currentFinishTarget = finishTarget2;
            oldFinishTarget = finishTarget1;
        }
        changeFinishTarget = !changeFinishTarget;
        float distance = lastFinishTargetZ + stackHeight * 10 + (stackHeight * 3 * distanceLevel) + currentFinishTarget.localScale.z * scaleMultiplier;
        currentFinishTarget.transform.position = new Vector3(0, 0, distance);
        EventsManager.onFinishLineZChanged?.Invoke(currentFinishTarget.position.z);
        distanceLevel++;
        currentFinishTarget.gameObject.SetActive(true);
    }
}
