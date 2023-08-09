using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {
    //ı
    public static GameManager instance;
    public StackManager stackManager;
    [SerializeField] Transform finishTarget1;
    [SerializeField] Transform finishTarget2;
    [SerializeField] Transform player;
    bool changeFinishTarget;
    Transform currentFinishTarget;
    Transform oldFinishTarget;
    bool isFirstStackSpawn = true;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        EventsManager.onInitializeGame += OnInitializeTheGame;
        EventsManager.onGameFinished += OnGameFinished;
        EventsManager.onRestartGame += OnRestartGame;
    }


    private void OnDisable()
    {
        EventsManager.onInitializeGame -= OnInitializeTheGame;
        EventsManager.onGameFinished -= OnGameFinished;
        EventsManager.onRestartGame -= OnRestartGame;

    }
    private void OnRestartGame()
    {
        currentFinishTarget = null;
        oldFinishTarget = null;
        finishTarget1.gameObject.SetActive(false);
        finishTarget2.gameObject.SetActive(false);
        isFirstStackSpawn = true;
        stackManager.ResetStacks();
        OnInitializeTheGame();
    }
    private void OnInitializeTheGame()
    {
        float lastReferenceObjectPosZ = 0;
        if(isFirstStackSpawn)
        {
            lastReferenceObjectPosZ = stackManager.GetForemostPosition();
        } else
        {
            lastReferenceObjectPosZ = GetFinishPosition();

        }
        stackManager.InitializeStackSpawner(lastReferenceObjectPosZ);
        InitializeFinishTarget();
        StartTheGameWithAnimation();
        isFirstStackSpawn = false;

    }

    private float GetFinishPosition()
    {
        return currentFinishTarget.transform.position.z + currentFinishTarget.localScale.z * 2.5f  ;
    }

    private void StartTheGameWithAnimation()
    {
        int childCount = stackManager.transform.childCount;
     
        if(isFirstStackSpawn)
        {
            player.DOMoveZ(0, 1.4f).OnComplete(() => StartTheGame(true));
            //AnimateStack(stackManager.transform.GetChild(childCount - 3), 1);
            //AnimateStack(stackManager.transform.GetChild(childCount - 2), 1.2f);
            //Transform frontStack = stackManager.transform.GetChild(childCount - 1);
            //frontStack.DOLocalRotate(Vector3.zero, 1.4f);
            //frontStack.DOLocalMove(new Vector3(0, -0.5f, frontStack.transform.position.z), 1.4f)
       }
       else
         {
            stackManager.ReloadNextLevel();
            StartTheGame();
       //Transform stack = stackManager.transform.GetChild(childCount - 2);
        //stack.localScale = stackManager.GetStackScale();
        //float targetZ = oldFinishTarget.transform.position.z + oldFinishTarget.localScale.z * 2.5f;
        //stack.transform.position = new Vector3(stack.position.x, -3, targetZ);
        //stack.DOLocalMove(new Vector3(0, -0.5f, targetZ), 1.4f);
        //stack.DOLocalRotate(Vector3.zero, 1.4f).OnComplete(() => StartTheGame());
        }
        //StartTheGame();
    }
    //void AnimateStack(Transform stack, float timer)
    //{
    //    stack.DOLocalMove(new Vector3(0, -0.5f, stack.transform.position.z), timer);
    //    stack.DOLocalRotate(Vector3.zero, timer);
    //}
    private void StartTheGame(bool isFirstSpan = false)
    {
        if(isFirstSpan)
        {
            stackManager.SetFirstStackPos();
            stackManager.SpawnNewStack(true);
        }
        stackManager.enabled = true;
        EventsManager.onGameStart?.Invoke();
    }
    private void OnGameFinished(bool isWin)
    {
        stackManager.enabled = false;

    }

    private void InitializeFinishTarget()
    {
        float lastFinishTargetZ = 0;
        float scaleMultiplier = 2.5f;
        if(currentFinishTarget != null)
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
        //float value = lastFinishTargetZ + stackManager.GetStackHeight() * 10 + currentFinishTarget.localScale.z * scaleMultiplier;
        //print(value + " --" + lastFinishTargetZ  + " -- " + stackManager.GetStackHeight() * 10 + " -- " + currentFinishTarget.localScale.z * scaleMultiplier);
        currentFinishTarget.transform.position = new Vector3(0, 0, lastFinishTargetZ + stackManager.GetStackHeight() * 10 + currentFinishTarget.localScale.z * scaleMultiplier);
        currentFinishTarget.gameObject.SetActive(true);
    }

    public Transform GetFinishTarget()
    {
        return currentFinishTarget;
    }
}
