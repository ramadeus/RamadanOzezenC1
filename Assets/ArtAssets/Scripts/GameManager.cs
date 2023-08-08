using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {
    //ı
    public static GameManager instance; 
    [SerializeField] Transform finishTarget1;
    [SerializeField] Transform finishTarget2;
    Transform currentFinishTarget;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
        InitializeFirstFinishTarget();
    }

  
    private void InitializeFirstFinishTarget()
    {
        currentFinishTarget = finishTarget1;
        currentFinishTarget.transform.position = new Vector3(0, 0, 100f);
        currentFinishTarget.gameObject.SetActive(true);
    }

    public Transform GetFinishTarget()
    {
        return currentFinishTarget;
    }
}
