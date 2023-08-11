using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    //ı
    [SerializeField] float timer;
    private void OnEnable()
    {
        EventsManager.onGameStart += DestroyGameObject;
    }
    private void OnDisable()
    {
        EventsManager.onGameStart -= DestroyGameObject;
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject, timer);
    }


}
