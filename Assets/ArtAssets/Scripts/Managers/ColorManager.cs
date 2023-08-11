using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    //ı
    [SerializeField] Material[] materials;
    int index;
    private void OnEnable()
    {
        EventsManager.onObjectSpawn += ChangeColor;
    }
    private void OnDisable()
    {
        EventsManager.onObjectSpawn -= ChangeColor;
    }
    private void ChangeColor(int stackId)
    {
       GameObject spawnedObject = ObjectPooler.Instance.GetPoolObject("Stack", stackId);
        MeshRenderer mesh = spawnedObject.GetComponent<MeshRenderer>();
        mesh.material = materials[index];
        GenerateIndex();
    }

    private void GenerateIndex()
    {
        index = (index + 1) % materials.Length;
    }
}
