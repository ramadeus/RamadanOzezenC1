using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Menu : MonoBehaviour
{
    //ı
    [SerializeField] TMP_Text coinText;
    [SerializeField] TMP_Text diamondText;
    [SerializeField] TMP_Text starText;
    private void OnEnable()
    {
        coinText.text = GetData("coin").ToString();
        diamondText.text = GetData("diamond").ToString();
        starText.text = GetData("star").ToString();
    }
    private int GetData(string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }
        return 0;
    }
}
