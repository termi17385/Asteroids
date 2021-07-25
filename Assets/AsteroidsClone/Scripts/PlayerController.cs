using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField, Tooltip("Used to toggle on and off the thruster")] 
        private GameObject thruster;
        [SerializeField, Tooltip("How much thrust to add to the player")] 
        private float thrust;
        [SerializeField, Tooltip("How fast to rotate the player")] 
        private float speed;
        [SerializeField, Tooltip("The spawn point of the objects")] 
        private Transform gun;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject deathEffect;
        [SerializeField] private GameObject shieldObj;
        #endregion
        private float x;
        private bool engage;
        [NonSerialized] public bool shield;
        private Rigidbody2D rb2d;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            rb2d = GetComponent<Rigidbody2D>();
            thruster.SetActive(false);

            x = 0;
            shield = true;
            GameManager.DeathEvent += Death;
        }

        // Update is called once per frame
        void Update()
        {
            EngageThrusters();
            
            if (shield)
            {
                shieldObj.SetActive(true);
                x += Time.deltaTime;
                if (x >= 5)
                {
                    shieldObj.SetActive(false);
                    shield = false;
                }
            }
            
            if(Input.GetKeyDown(KeyCode.Space)) Shoot();
            if(Input.GetKey(KeyCode.D)) PlayerRotation((speed * 10) * Time.deltaTime);
            if(Input.GetKey(KeyCode.A)) PlayerRotation((-speed * 10) * Time.deltaTime);
        }
        private void FixedUpdate() {if (engage) Thruster();}
        private void Shoot() => Instantiate(prefab, gun.position, gun.rotation);
        private void EngageThrusters() => thruster.SetActive (engage = Input.GetKey(KeyCode.W));
        private void PlayerRotation(float _rotationSpeed)
        {
            var rotation = Vector3.zero;
            rotation.z -= _rotationSpeed;
            transform.Rotate(rotation);
        }
        private void Thruster()
        {
            rb2d.AddForce(transform.up * thrust * Time.fixedDeltaTime);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, 25);
        }

        private void Death()
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            GameManager.DeathEvent -= Death;
            Destroy(gameObject);
        }
        private void OnDestroy() => Debug.Log("death");
    }
}
