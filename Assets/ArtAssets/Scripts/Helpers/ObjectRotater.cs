using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotater : MonoBehaviour
{
    //ı
        float spinSpeed  = 30;
    private void Update()
    {

        
            transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        
    }
}
