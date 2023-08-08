using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerController: MonoBehaviour {
    //ı
    [SerializeField] float speed = 5; 
    Transform currentFinishTarget;
    bool canGoForward = true;
   
    private void Start()
    {
        currentFinishTarget = GameManager.instance.GetFinishTarget();
    }
    private void Update()
    {
        if(canGoForward)
        {
     transform.position =   Vector3.MoveTowards(transform.position, currentFinishTarget.position, Time.deltaTime * speed);
        }
         
    }
}
