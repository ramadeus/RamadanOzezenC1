using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController: MonoBehaviour {
    //ı
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;
    [SerializeField] float speed = 5;
    float currentFinishTargetZ;
    float lastStandingStackX; 
    bool canGoForward = false; 
    private void OnEnable()
    {
        EventsManager.onInitializeGame += OnInitializeTheGame;
        EventsManager.onGameStart += OnGameStart; 
        EventsManager.onGameFinished += OnGameFinished;

        EventsManager.onFinishLineZChanged += OnFinishLineZChanged;
        EventsManager.onLastStandingXChanged += OnLastStandingXChanged;
    } 
    private void OnDisable()
    {
        EventsManager.onInitializeGame -= OnInitializeTheGame;
        EventsManager.onGameStart -= OnGameStart;
        EventsManager.onGameFinished -= OnGameFinished; 

        EventsManager.onFinishLineZChanged -= OnFinishLineZChanged;
        EventsManager.onLastStandingXChanged -= OnLastStandingXChanged;
    } 
    private void OnLastStandingXChanged(float x)
    {
        lastStandingStackX = x;
    }

    private void OnFinishLineZChanged(float z)
    {
        currentFinishTargetZ = z;
    }

    private void OnInitializeTheGame(bool isFirstLevel)
    {
        if(isFirstLevel)
        {
        transform.DOMoveZ(0, 3f);
        }
    } 
    private void OnGameStart()
    {
        anim.SetBool("dance", false);
        rb.isKinematic = false;
        canGoForward = true;
    } 
    private void OnGameFinished(bool isWin)
    {
        if(isWin)
        {
            anim.SetBool("dance", true);
        }
        canGoForward = false;
        rb.isKinematic = true;
    }
    private void Update()
    {
        if(canGoForward)
        {
            Vector3 moveDirection = new Vector3(lastStandingStackX, 0, transform.position.z + transform.forward.z);
            transform.position = Vector3.MoveTowards(transform.position, moveDirection, Time.deltaTime * speed);
            CheckReachingToFinish();
            CheckFalling();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ICollectible collectible))
        {
            collectible.OnCollected();
        }
    }
    private void CheckReachingToFinish()
    {
        float distanceToFinishTarget = transform.position.z - currentFinishTargetZ;
        if(Mathf.Abs(distanceToFinishTarget) <= .2f)
        {
            EventsManager.onGameFinished?.Invoke(true);
        }
    }
    private void CheckFalling()
    {
        if(transform.position.y <= -10)
        {
            EventsManager.onGameFinished?.Invoke(false);
        }
    }
}
