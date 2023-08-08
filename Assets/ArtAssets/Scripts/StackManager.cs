using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StackManager: MonoBehaviour {
    //ı
    [SerializeField] GameObject stackPrefab;
    [SerializeField] Transform player;
    [SerializeField] float stackMovementSpeed = 2;
    [SerializeField] float offset = 1;
    GameObject currentStack;
    Transform oldStack; 
    Transform oldStack2;
    float stackHeight;
    float starterPosition;
    float limitIncreasement;
    bool directionChanger;
    public enum Direction {
        Left,
        Right,
        Front,
        Back
    }
    private void Start()
    {
        stackHeight = stackPrefab.transform.localScale.z;
        int positiveZStackCount = (transform.childCount - 4);
        starterPosition = positiveZStackCount * stackHeight;
        currentStack = transform.GetChild(transform.childCount - 1).gameObject;
        currentStack = transform.GetChild(transform.childCount - 1).gameObject;
        SpawnStack(true);
    }
    private void Update()
    {
        MoveStackBetweenTheLimits();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnStack();
        }
    }

    private void MoveStackBetweenTheLimits()
    {
        int direction = 1;
        if(directionChanger)
        {

            if(limitIncreasement >= stackHeight + offset)
            {
                directionChanger = false;
            }
        } else
        {
            direction *= -1;
            if(limitIncreasement <= -stackHeight - offset)
            {
                directionChanger = true;
            }
        }
        limitIncreasement += Time.deltaTime * direction * stackMovementSpeed;

        currentStack.transform.position = new Vector3(limitIncreasement, currentStack.transform.position.y, currentStack.transform.position.z);
    }

    void SpawnStack(bool isFirstSpawn = false)
    { 
        oldStack2 = oldStack ;
        oldStack = currentStack.transform;
        currentStack = transform.GetChild(0).gameObject;
        currentStack.transform.SetAsLastSibling();
         DivideObject(isFirstSpawn);
        starterPosition += stackHeight;
    }
    private void DivideObject(bool isFirstSpawn  )
    {
    
        float value =Mathf.Abs(oldStack.transform.position.x- currentStack.transform.position.x);
        float xValue = oldStack.transform.localScale.x - value;
        if(xValue <= 0)
        {
            xValue = .1f;
            print("done");
        }
        oldStack.transform.localScale = new Vector3(xValue, oldStack.transform.localScale.y, oldStack.transform.localScale.z);
        
        float old2x = 0;
        if(!isFirstSpawn)
        {
            if(oldStack2.transform.position.x > oldStack.transform.position.x)
            {
                old2x = oldStack2.transform.position.x - (oldStack2.transform.localScale.x / 2); 
            } else
            {
                old2x = oldStack2.transform.position.x + (oldStack2.transform.localScale.x / 2); 

            }
            print(old2x);
            float lastResult = 0;
            if(old2x<0)
            {
                lastResult = old2x + (oldStack.localScale.x / 2);
            } else
            {
                lastResult = old2x - (oldStack.localScale.x / 2);

            }
            oldStack.transform.position = new Vector3(lastResult, oldStack.transform.position.y, oldStack.transform.position.z);
        }
        //if(!isFirstSpawn)
        //{
        //    oldStack.transform.position = new Vector3(oldPos.x, oldStack.transform.position.y, oldStack.transform.position.z);
        //}
        currentStack.transform.localScale = oldStack.transform.localScale;
        currentStack.transform.position = new Vector3(0, -.5f, starterPosition);
        
    }

    private Vector3 GetPositionEdge(MeshRenderer mesh )
    {
        Vector3 extents = mesh.bounds.extents;
        Vector3 position = mesh.transform.position;
        if(directionChanger)
        {
                position.x += extents.x;

        } else
        {

                position.x += -extents.x;
        }
         

        return position;
    }
}
