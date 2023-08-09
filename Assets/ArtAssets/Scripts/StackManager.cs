using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StackManager: MonoBehaviour {
    //ı
    [SerializeField] Transform stackPrefab;
    [SerializeField] Transform player;
    [SerializeField] float stackMovementSpeed = 2;
    [SerializeField] float offset = 1;
    [SerializeField] float tolerance = .3f;

    Transform currentlyMovingStack;
    Transform lastStandingStack;
    Transform previousStandingStack;

    float newStackPosition;
    float limitIncreasement;
    bool directionChanger;
    private bool spawnStop;
    float firstStackPos;
    
    
    public void SetFirstStackPos()
    {
        firstStackPos = transform.GetChild(0).position.z;

    }



    //void SpawnNewStack()
    //{

    //}















    public void InitializeStackSpawner(float lastReferenceObjectPos)
    {

        newStackPosition = lastReferenceObjectPos;

        currentlyMovingStack = transform.GetChild(transform.childCount - 1);
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
        previousStandingStack = lastStandingStack;

        lastStandingStack = currentlyMovingStack.transform;
        lastStandingStack.GetComponent<BoxCollider>().enabled = true;

        currentlyMovingStack = transform.GetChild(0);
        currentlyMovingStack.transform.SetAsLastSibling();

        float wastedArea = GetWastedAreaWithTolerance();
        if(CheckIfFail(wastedArea))
        {
            SetGameOver();
            spawnStop = true;
            return;
        }

        DivideObject(wastedArea);

        if(CheckIfStackArrivedToFinish())
        {
            spawnStop = true;
        }
        GenerateNewCurrentStack();
        newStackPosition += GetStackHeight();
    }



  public  void SpawnNewStack(bool isInitalizerSpawn)
    {
        previousStandingStack = lastStandingStack;
        lastStandingStack = currentlyMovingStack.transform;
        lastStandingStack.GetComponent<BoxCollider>().enabled = true;
        currentlyMovingStack = transform.GetChild(0);
        currentlyMovingStack.transform.SetAsLastSibling();
        GenerateNewCurrentStack(isInitalizerSpawn);
        newStackPosition += GetStackHeight();
    }
    private bool CheckIfStackArrivedToFinish()
    {
        Transform finishTarget = GameManager.instance.GetFinishTarget();
        if(previousStandingStack.position.z >= (finishTarget.position.z - GetStackHeight() - finishTarget.localScale.z * 2.5f))
        {
            return true;
        }
        return false;
    }
    private void DivideObject(float wastedArea)
    {
        SetStandingPieceScale(wastedArea);
        float fallingPieceAlignPosition = GetFallingPieceAlignPosition();
        SetStandingPieceAlignPosition(fallingPieceAlignPosition);
        GenerateFallingPiece(positionX: fallingPieceAlignPosition, scaleX: wastedArea);
    }

    private void GenerateNewCurrentStack(bool isInitalizerSpawn = false)
    {
        Vector3 scale = isInitalizerSpawn ? stackPrefab.localScale : lastStandingStack.transform.localScale;
        currentlyMovingStack.transform.localScale = scale;
        if(!spawnStop)
        {
            currentlyMovingStack.transform.position = new Vector3(0, -.5f, newStackPosition);
        }
        currentlyMovingStack.GetComponent<BoxCollider>().enabled = false;

    }

    private float GetFallingPieceAlignPosition()
    {
        float fallingPieceAlignPosition = 0;
        bool alignPositionIsRight = previousStandingStack.transform.position.x > lastStandingStack.transform.position.x;

        if(alignPositionIsRight)
        {
            fallingPieceAlignPosition = previousStandingStack.transform.position.x - (previousStandingStack.transform.localScale.x / 2);
        } else
        {
            fallingPieceAlignPosition = previousStandingStack.transform.position.x + (previousStandingStack.transform.localScale.x / 2);

        }
        return fallingPieceAlignPosition;
    }


    private void SetStandingPieceScale(float wastedArea)
    {
        float standingStackScaleX = lastStandingStack.transform.localScale.x - wastedArea;
        lastStandingStack.transform.localScale = new Vector3(standingStackScaleX, lastStandingStack.transform.localScale.y, lastStandingStack.transform.localScale.z);
    }

    private void SetGameOver()
    {
        EventsManager.onGameFinished?.Invoke(false);
    }

    private bool CheckIfFail(float wasterArea)
    {
        return (lastStandingStack.transform.localScale.x - wasterArea) <= 0;
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

    private void SetStandingPieceAlignPosition(float fallingPieceAlignPosition)
    {
        float standingPieceAlignPosition;
        if(fallingPieceAlignPosition < 0)
        {
            standingPieceAlignPosition = fallingPieceAlignPosition + (lastStandingStack.localScale.x / 2);
        } else
        {
            standingPieceAlignPosition = fallingPieceAlignPosition - (lastStandingStack.localScale.x / 2);

        }
        lastStandingStack.transform.position = new Vector3(standingPieceAlignPosition, lastStandingStack.transform.position.y, lastStandingStack.transform.position.z);
    }
    private void GenerateFallingPiece(float positionX, float scaleX)
    {
        if(scaleX == 0)
        {
            return;
        }
        GameObject fallingPiece = Instantiate(stackPrefab.gameObject, new Vector3(positionX, lastStandingStack.transform.position.y, lastStandingStack.transform.position.z), Quaternion.identity);
        fallingPiece.AddComponent<Rigidbody>();
        fallingPiece.transform.localScale = new Vector3(scaleX, lastStandingStack.localScale.y, lastStandingStack.localScale.z);
        Destroy(fallingPiece, 5);

    }
    public Transform GetNewPassedStack()
    {
        if(lastStandingStack == null)
        {
            return transform.GetChild(transform.childCount - 2);

        }
        return lastStandingStack;
    }
    public float GetStackHeight()
    {
        return stackPrefab.localScale.z;
    }
    public Vector3 GetStackScale()
    {
        return stackPrefab.localScale;
    }

    public float GetForemostPosition()
    {
        return transform.GetChild(transform.childCount - 1).position.z + GetStackHeight();
    }

    public void ReloadNextLevel()
    {
        spawnStop = false;
        for(int i = 0; i < 3; i++)
        {
            SpawnNewStack(true);
        }

    }

    internal void ResetStacks()
    {
        print(firstStackPos);
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform stack = transform.GetChild(i); 
            stack.position = new Vector3(0, -.5f, firstStackPos + (i * GetStackHeight()));
            stack.localScale = stackPrefab.localScale;
        } 
         currentlyMovingStack =null; 
         lastStandingStack  = null;
        previousStandingStack = null;
        spawnStop = false; 
    }
}
