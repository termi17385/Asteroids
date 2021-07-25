using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class AsteroidSpawner : SerializedMonoBehaviour
{
    public delegate void AsteroidCountDelegate(int _increment);
    public static event AsteroidCountDelegate CountEvent;
    
    [SerializeField] private List<Transform> listOfSpawnPoints = new List<Transform>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool debug = false;

    public static AsteroidSpawner instance;
    [SerializeField] private int spawnAmount = 5;
    [SerializeField] private int maxAsteroids;
    
    public int asteroidCount;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(instance);

        CountEvent += AsteroidsSpawning;
        maxAsteroids = 4 * spawnAmount;
    }

    private void AsteroidsSpawning(int _increment)
    {
        asteroidCount += _increment;
        
        if (asteroidCount < maxAsteroids) return;
        spawnAmount += 5;
        Spawn((spawnAmount));
        
        maxAsteroids = (spawnAmount * 4);
        asteroidCount = 0;
    }
    public static void OnAsteroidDestroy(int _increment) => CountEvent?.Invoke(_increment); 
    private void Start() => Spawn(spawnAmount);
    private void Spawn(int _spawnAmount)
    {
        if (listOfSpawnPoints.Count <= 0) 
        {
            foreach (Transform child in transform)
                listOfSpawnPoints.Add(child);
        }
        
        for (int i = 0; i < _spawnAmount; i++)
        {
            var index = Random.Range(0, listOfSpawnPoints.Count);
            var spawnPos = listOfSpawnPoints[index];

            var rotation = Random.rotation;
            rotation.y = (rotation.x = 0);
            Instantiate(prefab, spawnPos.position, rotation);
        }
    }

    private void OnDisable() => CountEvent -= AsteroidsSpawning;
    private void OnDrawGizmos()
    {
        if (debug)
        {
            foreach (Transform child in transform)
                listOfSpawnPoints.Add(child);
        }
    }
}
