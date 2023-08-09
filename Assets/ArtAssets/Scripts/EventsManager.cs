using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager  
{
    //ı
    public static Action<bool> onGameFinished;
    public static Action  onGameStart;
    public static Action  onRestartGame;
    public static Action  onInitializeGame;
}
