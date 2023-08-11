using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    //ı
    int comboCounter; 
    private void OnEnable()
    {
        EventsManager.onComboTry += GenerateComboSystem;
    }
    private void OnDisable()
    {
        EventsManager.onComboTry -= GenerateComboSystem;
    }

    private void GenerateComboSystem(bool isSuccessful)
    {
        if(isSuccessful)
        {
            comboCounter++;
        } else
        { 
            comboCounter = 0;
        }
        print(comboCounter);
    }
}
