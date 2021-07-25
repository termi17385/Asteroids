using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AsteroidHandler : MonoBehaviour
{
    [ReadOnly] public List<GameObject> asteroids = new List<GameObject>();
    private AsteroidSpawner spawner;
    
    
    private void Awake()
    {
        spawner = FindObjectOfType<AsteroidSpawner>();
    }

    private void Update()
    {
        foreach (var asteroid in asteroids)
            asteroid.transform.SetParent(transform);
    }

    public void Restart() => SceneManager.LoadScene(0);
    public void RemoveFromList(GameObject _asteroid)
    {
        for (var i = asteroids.Count - 1; i >= 0; i--)
        {
            var asteroid = asteroids[i];
            if (_asteroid == asteroid)
            { 
                asteroids.RemoveAt(i);
                Destroy(_asteroid);
                break;
            }
        }
    }
}
