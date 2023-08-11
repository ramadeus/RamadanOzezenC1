using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //ı
    [SerializeField] Image fadeInOut;
    [SerializeField] GameObject uiPanel;
    [SerializeField] GameObject nextLevelButton;
    [SerializeField] GameObject restartGameButton;
    bool isFirstLevel = true;
    private void Awake()
    {
        Color clr = fadeInOut.color;
        clr.a = 1;
        fadeInOut.color = clr;

        fadeInOut.DOFade(0.0f, 2).OnComplete(() => { fadeInOut.raycastTarget = false; });
    }
    public void StartTheGame()
    {
        EventsManager.onInitializeGame?.Invoke(isFirstLevel); 
        DeactivateUI();
        isFirstLevel = false;
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
        fadeInOut.DOFade(1f, 1).OnComplete(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });

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
