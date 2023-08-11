using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //ı
    [SerializeField] Image fadeInOut;
    [SerializeField] GameObject uiPanel;
    [SerializeField] GameObject infoPanel;
    [SerializeField] GameObject nextLevelButton;
    [SerializeField] GameObject restartGameButton;
    [SerializeField] Image playImage;
    [SerializeField] Sprite nextLevelSprite;


    bool isFirstLevel = true;
    private void Awake()
    {
        Color clr = fadeInOut.color;
        clr.a = 1;
        fadeInOut.color = clr;

        fadeInOut.DOFade(0.0f, 1).OnComplete(() => { fadeInOut.raycastTarget = false; });
        uiPanel.SetActive(true);
        InitializeButtonClickSounds();

    }

    private void InitializeButtonClickSounds()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => { AudioManager.Instance.PlaySFX("ButtonClick"); });
        }
    }

    public void StartTheGame()
    {
        EventsManager.onInitializeGame?.Invoke(isFirstLevel); 
        DeactivateUI();
        infoPanel.SetActive(true);
        isFirstLevel = false;
        playImage.sprite = nextLevelSprite;
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
        if(isWin)
        {
        StartCoroutine(ActivatePanelWithDelay());
            nextLevelButton.SetActive(true);
        } else
        {
            restartGameButton.SetActive(true);
            uiPanel.SetActive(true);
        }
    }
    IEnumerator ActivatePanelWithDelay()
    {
        yield return new WaitForSeconds(5f);
        uiPanel.SetActive(true);
    }

}
