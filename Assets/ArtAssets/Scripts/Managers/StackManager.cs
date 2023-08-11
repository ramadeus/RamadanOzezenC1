using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StackManager: MonoBehaviour {
    //ı
    [SerializeField] StarterRoadAnimator starterRoadAnimator;
    [SerializeField] StackDivider stackDivider;
    [SerializeField] Transform stackPrefab;
    [SerializeField] Transform player;
    [SerializeField] float stackMovementSpeed = 2;
    [SerializeField] float offset = 1;
    [SerializeField] float tolerance = .3f;

    ObjectPooler objectPooler;
    Transform currentlyMovingStack;
    Transform lastStandingStack;
    Transform previousStandingStack;
    float limitIncreasement;
    float firstStackPos;
    float currentFinishTargetZ;
    bool spawnStop = true;
    bool directionChanger;
    bool stopSpawnAtNext;


    private void OnEnable()
    {
        objectPooler = ObjectPooler.Instance;
        EventsManager.onObjectSpawn += InitializeSpawnHistory;
        EventsManager.onInitializeGame += OnInitializeTheGame;
        EventsManager.onGameStart += OnGameStart;
        EventsManager.onFinishLineZChanged += OnFinishLineZChanged;
    }
    private void OnDisable()
    {
        EventsManager.onObjectSpawn -= InitializeSpawnHistory;
        EventsManager.onInitializeGame -= OnInitializeTheGame;
        EventsManager.onGameStart -= OnGameStart;
        EventsManager.onFinishLineZChanged -= OnFinishLineZChanged;
    }
    private void OnGameStart()
    {
        objectPooler.SpawnFromPool("Stack", GetStackSpawnPosition(), Quaternion.identity, stackPrefab.localScale);
        spawnStop = false;
    }

    private void OnInitializeTheGame(bool isFirstLevel)
    {
        if(isFirstLevel)
        {
            starterRoadAnimator.SpawnFirstRoadWithAnim(3, 0.0f);
        } else
        {
            starterRoadAnimator.SpawnFirstRoadWithAnim(3, GetFinishPosition());
        }
        stopSpawnAtNext = false;
    }
    private void InitializeSpawnHistory(int id)
    {
        GameObject newSpawnedObject = objectPooler.GetPoolObject("Stack", id);
        previousStandingStack = lastStandingStack;
        lastStandingStack = currentlyMovingStack;
        if(lastStandingStack != null)
        {
            EventsManager.onLastStandingXChanged?.Invoke(lastStandingStack.position.x);
        }
        currentlyMovingStack = newSpawnedObject.transform;
    }
    private void Update()
    {
        if(spawnStop)
        {
            return;
        }

        MoveStackBetweenTheLimits();

        if(Input.GetMouseButtonDown(0))
        {
            SpawnNewStack();
        }
    } 

    private void MoveStackBetweenTheLimits()
    {
        int direction = 1;
        if(directionChanger)
        {
            if(limitIncreasement >= GetStackHeight() + offset)
            {
                directionChanger = false;
            }
        } else
        {
            direction *= -1;
            if(limitIncreasement <= -GetStackHeight() - offset)
            {
                directionChanger = true;
            }
        }
        limitIncreasement += Time.deltaTime * direction * stackMovementSpeed;

        currentlyMovingStack.transform.position = new Vector3(limitIncreasement, currentlyMovingStack.transform.position.y, currentlyMovingStack.transform.position.z);
    }

    void SpawnNewStack()
    {
        if(stopSpawnAtNext)
        {
            currentlyMovingStack.GetComponent<BoxCollider>().enabled = true;
            spawnStop = true;
            return;
        }

        objectPooler.SpawnFromPool("Stack", GetStackSpawnPosition(), Quaternion.identity, stackPrefab.localScale);

        lastStandingStack.GetComponent<BoxCollider>().enabled = true;

        float wastedArea = GetWastedAreaWithTolerance();
        TryCombo(wastedArea == 0);
        if(CheckIfFail(wastedArea))
        {
            SetGameOver();
            spawnStop = true;
            return;
        }
        if(CheckIfStackArrivedToFinish())
        {
            stopSpawnAtNext = true;
        }

        stackDivider.GenerateStackHistory(previousStandingStack, lastStandingStack);
        stackDivider.DivideObject(wastedArea);
        ScaleNewCurrentStack();
    }

    private void TryCombo(bool isSuccessful)
    {
        EventsManager.onComboTry?.Invoke(isSuccessful);
    }

    Vector3 GetStackSpawnPosition()
    {
        float targetZ = currentlyMovingStack.position.z + stackPrefab.localScale.z;
        Vector3 targetPos = new Vector3(lastStandingStack.position.x, lastStandingStack.position.y, targetZ);
        return targetPos;
    }
    private void ScaleNewCurrentStack(bool isInitalizerSpawn = false)
    {
        Vector3 scale = isInitalizerSpawn ? stackPrefab.localScale : lastStandingStack.transform.localScale;
        currentlyMovingStack.transform.localScale = scale;

        currentlyMovingStack.GetComponent<BoxCollider>().enabled = false;

    }

    private bool CheckIfStackArrivedToFinish()
    {

        if(lastStandingStack.position.z >= (currentFinishTargetZ - GetStackHeight() - 1 * 2.5f))
        {
            return true;
        }
        return false;
    }

    private void SetGameOver()
    {
        EventsManager.onGameFinished?.Invoke(false);
    }

    private bool CheckIfFail(float wasterArea)
    {
        return (lastStandingStack.transform.localScale.x - wasterArea) <= 0;
    }
    private void OnFinishLineZChanged(float z)
    {
        currentFinishTargetZ = z;
    }

    private float GetWastedAreaWithTolerance()
    {
        float wastedArea = Mathf.Abs(lastStandingStack.transform.position.x - currentlyMovingStack.transform.position.x);
       
        if(wastedArea < tolerance)
        {
            wastedArea = 0; 
        }
        
        return wastedArea;
    } 
    public float GetStackHeight()
    {
        return stackPrefab.localScale.z;
    }
    private float GetFinishPosition()
    {
        return currentFinishTargetZ + 2.5f;
    }

}
