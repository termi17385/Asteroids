using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.player;
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
    #region Variables
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private float moveSpeed;

    [FormerlySerializedAs("m_AsteroidType")] [SerializeField]
    public AsteroidType asteroidType;

    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private List<AudioClip> soundEffects = new List<AudioClip>();

    [SerializeField] private AsteroidHandler handler;
    private Rigidbody2D rb2d;
    private int score = 20;
    #endregion
    #region Start and Update
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

        moveSpeed += Random.Range(100f, 200f);

        var transform1 = transform;
        var rotation = transform1.rotation;
        rotation = Random.rotation;
        rotation.y = 0;
        rotation.x = 0;

        transform1.rotation = rotation;
    }
    private void FixedUpdate()
    {
        MoveAsteroid();
    }
    #endregion
    #region Methods
    private void MoveAsteroid()
    {
        rb2d.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                if(!_other.GetComponent<PlayerController>().shield)
                    GameManager.instance.OnPlayerDeath();
            }
            
            AsteroidDeath();
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
        for (var i = 0; i < 2; i++)
        {
            var transform1 = transform;
            var rotation = transform1.rotation;
            rotation.z += 2 * i;

            var obj = Instantiate(prefab, transform1.position, rotation).transform;
            obj.localScale = scale;
            obj.GetComponent<PolygonCollider2D>().enabled = true;
            obj.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    
    #region Assigning
    /// <summary>
    /// Assigns the correct audio clip to the given asteroid type
    /// </summary>
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
    /// <summary>
    /// Resizes Asteroids Based on type
    /// </summary>
    /// <param name="_type">the type of asteroid we want</param>
    private Vector3 ResizeAsteroid(AsteroidType _type)
    {
        switch (_type)
        {
            case AsteroidType.Large:  score = 20;  
                return new Vector3(6, 6, 1);
            case AsteroidType.Medium: score = 50; 
                return new Vector3(3, 3, 1);
            case AsteroidType.Small:  score = 100;  
                return new Vector3(1, 1, 1);
        }

        return Vector3.one;
    }
    #endregion
    #region Death
    /// <summary>
    /// when called kills the asteroid And spawns a new one in place of different type
    /// unless at the last type
    /// </summary>
    public void AsteroidDeath()
    {
        var scale = Vector3.one; // re scales the asteroid
        var newType = asteroidType;     // sets the new asteroid type
        switch (asteroidType)
        {
            case AsteroidType.Large:
                scale = ResizeAsteroid(AsteroidType.Medium);
                newType = AsteroidType.Medium;
                score = 20;
                break;

            case AsteroidType.Medium:
                scale = ResizeAsteroid(AsteroidType.Small);
                newType = AsteroidType.Small;
                score = 50;
                break;
        }

        // disables colliders and renderer
        var component = GetComponent<PolygonCollider2D>();
        var component1 = GetComponent<SpriteRenderer>();

        component.enabled = false;
        component1.enabled = false;

        // completely destorys the asteroid if its a small one
        if (asteroidType == AsteroidType.Small)
        {
            if(GameManager.instance != null) GameManager.instance.OnAsteroidDeath(transform, 100);
            AsteroidSpawner.OnAsteroidDestroy(1); 
            StartCoroutine(DestroyMe());
        }
        // else spawns 2 new ones 
        else
        {
            if(GameManager.instance != null) GameManager.instance.OnAsteroidDeath(transform, score);
            StartCoroutine(DestroyMe());

            // spawns 2 new asteroids in place
            for (var i = 0; i < 2; i++)
            {
                var transform1 = transform;
                var rotation = transform1.rotation;
                rotation.z += 2 * i;

                var obj = Instantiate(prefab, transform1.position, rotation).transform;
                obj.localScale = scale;

                obj.GetComponent<PolygonCollider2D>().enabled = true;
                obj.GetComponent<SpriteRenderer>().enabled = true;

                var asteroid = obj.GetComponent<Asteroid>();
                asteroid.soundEffect.clip = AssignSoundEffect(asteroid.asteroidType = newType);
            }
        }
    }
    private IEnumerator DestroyMe()
    {
        var transform1 = transform;
        Instantiate(deathEffect, transform1.position, transform1.rotation);
        soundEffect.Play();

        // handles destroying asteroids
        yield return new WaitForSecondsRealtime(3);
        if (handler != null) handler.RemoveFromList(gameObject);
        else Destroy(gameObject);
    }
    #endregion
    #endregion
}