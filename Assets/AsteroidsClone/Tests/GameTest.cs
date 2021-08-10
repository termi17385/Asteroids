using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameTest
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator AsteroidsMove()
    {
        GameObject gameGO = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/[Game]"));
        var game = gameGO.transform;

        var spawner = game.GetComponentInChildren<AsteroidSpawner>();
        var asteroid = spawner.Spawn();
        
        yield return new WaitForSeconds(10f);
        UnityEngine.Assertions.Assert.IsNotNull(asteroid);
    }
}
