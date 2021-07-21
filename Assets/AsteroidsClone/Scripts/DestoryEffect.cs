using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryEffect : MonoBehaviour
{
    private ParticleSystem effect;
    private void Start() => effect = GetComponent<ParticleSystem>();
    private void Update() { if (effect.isStopped) Destroy(gameObject); }
}
