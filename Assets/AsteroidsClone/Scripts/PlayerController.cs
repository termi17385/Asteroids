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
        #endregion

        private bool engage;
        private Rigidbody2D rb2d;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            rb2d = GetComponent<Rigidbody2D>();
            thruster.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            EngageThrusters();
            
            if(Input.GetKeyDown(KeyCode.Space)) Shoot();
            if(Input.GetKey(KeyCode.D)) PlayerRotation((speed * 10) * Time.deltaTime);
            if(Input.GetKey(KeyCode.A)) PlayerRotation((-speed * 10) * Time.deltaTime);
        }
        private void FixedUpdate() {if (engage) Thruster();}

        private void Shoot() => Instantiate(prefab, gun.position, gun.rotation);
        private void EngageThrusters() => thruster.SetActive (engage = Input.GetKey(KeyCode.W));
        private void PlayerRotation(float _rotationSpeed)
        {
            var _rotation = Vector3.zero;
            _rotation.z -= _rotationSpeed;
            transform.Rotate(_rotation);
        }
        private void Thruster()
        {
            rb2d.AddForce(transform.up * thrust * Time.fixedDeltaTime);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, 25);
        }
    }
}
