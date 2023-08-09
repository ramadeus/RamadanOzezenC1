using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //ı
    [SerializeField] GameObject uiPanel;
    [SerializeField] GameObject nextLevelButton;
    [SerializeField] GameObject restartGameButton;

   public void StartTheGame()
    {
        EventsManager.onInitializeGame ?.Invoke();
        DeactivateUI();
        nextLevelButton.GetComponentInChildren<TMP_Text>().text = "Next Level";
    }

    private void DeactivateUI()
    {
        uiPanel.SetActive(false);
        nextLevelButton.SetActive(false);
        restartGameButton.SetActive(false);
    }

    public void RestartTheGame()
    {
        EventsManager.onRestartGame?.Invoke(); 
        DeactivateUI();
    }
    private void OnEnable()
    {
        EventsManager.onGameFinished+= OnGameFinished; 
    }


    private void OnDisable()
    {
        EventsManager.onGameFinished -= OnGameFinished;


    }
    private void OnGameFinished(bool isWin)
    {
        uiPanel.SetActive(true);
        if(isWin)
        {
            nextLevelButton.SetActive(true);
        } else
        {
            restartGameButton.SetActive(true);
        }
    }

}
