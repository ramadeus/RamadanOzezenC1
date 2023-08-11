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

    // switch the following process to look at process   
    private void OnGameFinished(bool isWin)
    {
        cam.Follow = null;
        if(isWin)
        {
            cam.LookAt = playerPos;
            DanceCam(); 
        }
    }
    // switch back the look at process to following process   
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
    /// <summary>
    /// create an empty object to the player's position and make the cam child of it so it is the new origin of camera, then rotate the origin, so rotate the camera
    /// </summary>
    private void DanceCam()
    {
        rotateOrigin = new GameObject("CamOrigin");
        rotateOrigin.transform.position = playerPos.position;
        cam.transform.SetParent(rotateOrigin.transform);
        rotateOrigin.AddComponent<ObjectRotater>();
    }
}
