using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class AsteroidSpawner : SerializedMonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private Transform OTransform;
    [SerializeField] private Vector2 spawnCoords;

    [SerializeField] private Transform overlap;
    public bool pressed;

    private Color color;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Transform transform2 = null;
        for (int i = 0; i < 4; i++)
        {
            if(overlap != null) transform2 = overlap;
            spawnCoords.x = Random.Range(-4, 4);
            spawnCoords.y = Random.Range(-4, 4);

            var transform1 = transform;
            var position = transform1.position;
        
            position.x += (spawnCoords.x * 2);
            position.y += (spawnCoords.y * 2);
            position.z = 0;

            var rotation = transform1.rotation;
            rotation = Random.rotation;
            
            rotation.x = 0;
            rotation.y = 0;
            rotation.z *= 2;
            
            overlap = Instantiate(OTransform, position, rotation).transform;
            
            if(transform2 != null)
            { if (transform2.position == overlap.position)
                {
                    var t = overlap.position;
                    t.x *= 2;
                    t.y *= 2;
                    overlap.position = t;
                }
            }
        }
    }
}
