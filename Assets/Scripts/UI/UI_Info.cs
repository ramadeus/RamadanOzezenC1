using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Info : MonoBehaviour
{
    //ı
    [SerializeField] TMP_Text coinText;
    [SerializeField] TMP_Text diamondText;
    [SerializeField] TMP_Text starText;

    int coinCounter,diamondCounter,starCounter = 0;
    private void OnEnable()
    {
        EventsManager.onCollectibleTaken += UpdateInfoPanel;
    }
    private void OnDisable()
    {
        EventsManager.onCollectibleTaken -= UpdateInfoPanel;

    }
    private void UpdateInfoPanel(CollectibleType collectibleType)
    {
        switch(collectibleType)
        {
            case CollectibleType.coin:
                coinCounter++;
                coinText.text = coinCounter.ToString();
                break;
            case CollectibleType.diamond:
                diamondCounter++;
                diamondText.text = diamondCounter.ToString();
                break;
            case CollectibleType.star:
                starCounter++;
                starText.text = starCounter.ToString();
                break;
            default:
                break;
        }
    }
}
