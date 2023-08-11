using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour {
    //ı
    [SerializeField] CinemachineVirtualCamera cam;
    Transform playerPos;
    Quaternion startRot;
    GameObject rotateOrigin;
    private void Awake()
    {
        playerPos = cam.Follow;
        startRot = transform.rotation;
    }
    private void OnEnable()
    {
        EventsManager.onGameFinished += OnGameFinished;
        EventsManager.onInitializeGame += OnInitializeGame;
    }

    private void OnDisable()
    {
        EventsManager.onGameFinished -= OnGameFinished;
        EventsManager.onInitializeGame -= OnInitializeGame;

    }

    private void OnInitializeGame(bool obj)
    {
        transform.parent = null;
        if(rotateOrigin != null)
        {
            Destroy(rotateOrigin);
            rotateOrigin = null;
        }
        cam.LookAt = null; 
            cam.Follow = playerPos;
        transform.DORotateQuaternion(startRot, 1f);
    }

    private void OnGameFinished(bool isWin)
    {
        if(isWin)
        {
            cam.Follow = null;
                    cam.LookAt = playerPos;
      DanceCam();
            //cam.transform.DOLookAt(playerPos.position, .5f)
            //    .OnComplete(() =>
            //    {

            //  
            //    });

        }
    }
    private void DanceCam()
    {
        rotateOrigin = new GameObject("CamOrigin");
        rotateOrigin.transform.position = playerPos.position;
        cam.transform.SetParent(rotateOrigin.transform);
        rotateOrigin.AddComponent<ObjectRotater>();
    }
}
