using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void ScoreDelegate(Transform _pos, int _score);
    public static event ScoreDelegate ScoreEvent;
    
    public delegate void DeathDelegate();
    public static event DeathDelegate DeathEvent;
    
    public static GameManager instance;
    
    private void Start()
    {
        if (instance == null) instance = this;
        else if(instance != this) Destroy(this);
    }

    public void OnAsteroidDeath(Transform _spawnPoint, int _score) => ScoreEvent?.Invoke(_spawnPoint, _score);
    public void OnPlayerDeath() => DeathEvent?.Invoke();
}
