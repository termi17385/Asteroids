using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum AsteroidType
{
    Large,
    Medium,
    Small
}
public class Asteroid : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private float moveSpeed;
    [FormerlySerializedAs("m_AsteroidType")] [SerializeField] private AsteroidType asteroidType;
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private List<AudioClip> soundEffects = new List<AudioClip>();

    [SerializeField] private AsteroidHandler handler;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        soundEffect = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        soundEffect.clip = AssignSoundEffect(asteroidType);
        transform.localScale = ResizeAsteroid(asteroidType);

        handler = null;
        if (FindObjectOfType<AsteroidHandler>())
        {
            handler = FindObjectOfType<AsteroidHandler>();
            handler.asteroids.Add(gameObject);
        } 

        moveSpeed *= Random.Range(1, 3);

        var transform1 = transform;
        var rotation = transform1.rotation;
        rotation = Random.rotation;
        rotation.y = 0;
        rotation.x = 0;

        transform1.rotation = rotation;
    }
    private void FixedUpdate() => MoveAsteroid();

    private void MoveAsteroid()
    {
        rb2d.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;
    }

    public void Kill()
    {
        // the new size of the asteroid
        Vector3 scale = Vector3.one;
        AsteroidType newType = asteroidType;
        switch (asteroidType)
        {
            case AsteroidType.Large: 
            scale = ResizeAsteroid(AsteroidType.Medium);
            newType = AsteroidType.Medium;
            break;
            
            case AsteroidType.Medium:
            scale = ResizeAsteroid(AsteroidType.Small);
            newType = AsteroidType.Small;
            break;
        }
        
        // disables colliders and renderer
        var component = GetComponent<PolygonCollider2D>();
        var component1 = GetComponent<SpriteRenderer>();
        
        component.enabled = false;
        component1.enabled = false;
        
        if (asteroidType == AsteroidType.Small)
            StartCoroutine(DestroyMe());
        
        else
        {
            StartCoroutine(DestroyMe());
        
            // spawns 2 new asteroids in place
            for (int i = 0; i < 2; i++)
            {
                var transform1 = transform;
                var rotation = transform1.rotation;
                rotation.z += (2 * i);
            
                var obj = Instantiate(prefab, transform1.position, rotation).transform;
                obj.localScale = scale;
                
                obj.GetComponent<PolygonCollider2D>().enabled = true;
                obj.GetComponent<SpriteRenderer>().enabled = true;
                
                var asteroid = obj.GetComponent<Asteroid>();
                asteroid.soundEffect.clip = AssignSoundEffect(asteroid.asteroidType = newType);
            }
        }
    }
    public void DebugButton(string _type)
    {
        // sets the type 
        var aType = (AsteroidType) Enum.Parse
        (typeof(AsteroidType), _type);
        
        // resizes asteroid by type
        var scale = ResizeAsteroid(aType);
        
        // disables collider
        var component = GetComponent<PolygonCollider2D>();
        var component1 = GetComponent<SpriteRenderer>();
        
        component.enabled = false;
        component1.enabled = false;

        StartCoroutine(DestroyMe());
        // spawns 2 new asteroids in place
        for (int i = 0; i < 2; i++)
        {
            var transform1 = transform;
            var rotation = transform1.rotation;
            rotation.z += (2 * i);
            
            var obj = Instantiate(prefab, transform1.position, rotation).transform;
            obj.localScale = scale;
            obj.GetComponent<PolygonCollider2D>().enabled = true;
            obj.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private AudioClip AssignSoundEffect(AsteroidType _type)
    {
        switch (_type)
        {
            case AsteroidType.Large: 
                transform.localScale = ResizeAsteroid(AsteroidType.Medium);
                return soundEffects[0];
            break;
            
            case AsteroidType.Medium:
                transform.localScale = ResizeAsteroid(AsteroidType.Medium);
                return soundEffects[1];
            break;  
            
            case AsteroidType.Small:
                transform.localScale = ResizeAsteroid(AsteroidType.Small);
                return soundEffects[2];
            break;
        }

        return null;
    }
    private Vector3 ResizeAsteroid(AsteroidType _type) {
        switch (_type)
        {
            case AsteroidType.Large:    return new Vector3(6, 6, 1);
            case AsteroidType.Medium:   return new Vector3(3, 3, 1);
            case AsteroidType.Small:    return new Vector3(1, 1, 1);
        } return Vector3.one; 
    }

    private IEnumerator DestroyMe()
    {
        var transform1 = transform;
        Instantiate(deathEffect, transform1.position, transform1.rotation);
        soundEffect.Play();
        
        yield return new WaitForSecondsRealtime(3);
        if (handler != null) 
            handler.RemoveFromList(gameObject);
        else Destroy(gameObject);
    }
}
