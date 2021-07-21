using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Button button;

    private bool spawn = false;

    private void Start() => button.onClick.AddListener(()=>spawn=true);
    private void Update() => SpawnObject();
    private void SpawnObject()
    {
        if (spawn)
        {
            var transform1 = transform;
            var rotation = transform1.rotation;
            var obj = Instantiate(prefab, transform1.position, rotation);
        }
        spawn = false;
    }

    private void OnDrawGizmosSelected()
    {
        var up = transform.up;
        var position = transform.position;
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(position, up * 2);
    }
}
