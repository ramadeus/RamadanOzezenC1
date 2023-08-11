using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager  
{
    //ı
    #region Initializers
    public static Action<bool>  onInitializeGame ; 
    public static Action  onGameStart; 
    public static Action<bool> onGameFinished;
    #endregion

    #region Helpers
    public static Action  onLastStackSpawn;
    public static Action<bool> onComboTry;
    public static Action<int>  onObjectSpawn;
    public static Action<float> onFinishLineZChanged;  
    public static Action<float>  onLastStandingXChanged;
    public static Action<CollectibleType> onCollectibleTaken;
    #endregion
}
