using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObj : MonoBehaviour
{
      private void OnTriggerEnter2D(Collider2D _other)
      {
            if (_other.CompareTag("Asteroid"))
            {
                  var obj = _other.GetComponent<Asteroid>();
                  AsteroidSpawner.OnAsteroidDestroy(1);
                  obj.asteroidType = AsteroidType.Small;
                  obj.AsteroidDeath();
            }
      }
}
