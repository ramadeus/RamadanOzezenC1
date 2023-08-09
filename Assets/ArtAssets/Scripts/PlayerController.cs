using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
public class PlayerController: MonoBehaviour {
    //ı
    [SerializeField] float speed = 5;
    [SerializeField] Transform cam;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;
    Transform currentFinishTarget;
    Vector3 startPosition;
    Vector3 camStartPosition;
    bool canGoForward = false;
    
    StackManager stackManager; 
    private void Start()
    {
        startPosition = transform.position;
        camStartPosition =cam. transform.position;
        currentFinishTarget = GameManager.instance.GetFinishTarget();
        stackManager = GameManager.instance.stackManager; 
    }
    private void OnEnable()
    {
        EventsManager.onGameFinished += OnGameFinished;
        EventsManager.onGameStart += OnGameStart;
        EventsManager.onRestartGame += OnRestartGame;
    }
 private void OnDisable()
    {
        EventsManager.onGameStart -= OnGameStart;
        EventsManager.onGameFinished -= OnGameFinished;
        EventsManager.onRestartGame -= OnRestartGame;
    }
    private void OnRestartGame()
    {
        transform.position = startPosition;
    cam.transform.position =     camStartPosition;
    }

    private void OnGameStart()
    { 
            anim.SetBool("dance",false);
        canGoForward = true;
        cam.SetParent(transform);
    }


   
    private void OnGameFinished(bool isWin)
    {
        if(isWin)
        {
            anim.SetBool("dance",true);
        }
        canGoForward = false;
        cam.SetParent(null);
        rb.isKinematic = true;
    }

    private void Update()
    {
        if(canGoForward)
        { 
            Vector3 moveDirection = new Vector3(stackManager.GetNewPassedStack().position.x,0,transform.position.z + transform.forward.z) ; 
             transform.position = Vector3.MoveTowards(transform.position, moveDirection,  Time.deltaTime * speed);
            CheckReachingToFinish();
            CheckFalling(); 
        }

    }

    private void CheckReachingToFinish()
    {
        float distanceToFinishTarget = transform.position.z -  GameManager.instance.GetFinishTarget().transform.position.z ;
        if( Mathf.Abs(distanceToFinishTarget)<= .2f)
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
