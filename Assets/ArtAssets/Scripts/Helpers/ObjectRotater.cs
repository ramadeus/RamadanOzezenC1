using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotater : MonoBehaviour
{
    //ı
        float spinSpeed  = 30;
    private void Update()
    { 
        // to rotate origin while dancing
            transform.Rotate(0, spinSpeed * Time.deltaTime, 0); 
    }
}
