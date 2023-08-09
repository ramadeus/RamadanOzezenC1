using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
   //ı
   public enum StackOrder
    {
        currentlyMoving,
        lastStanding,
        previousStanding,
        other
    }
    public StackOrder stackOrder = StackOrder.other;

}
