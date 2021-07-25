using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   [SerializeField] private float speed;
   [SerializeField] private CircleCollider2D m_CircleCollider2D;
   private Rigidbody2D rb2d;

   private void Start()
   {
      rb2d = GetComponent<Rigidbody2D>();
      StartCoroutine(DestroyMe());
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Asteroid"))
      {
         var asteroid = other.GetComponent<Asteroid>();
         asteroid.AsteroidDeath();
         Destroy(gameObject);
      }
   }

   private void FixedUpdate() => Movement();
   private void Movement() => rb2d.velocity = transform.up * (speed * 10) * Time.fixedDeltaTime;
   
   /// <summary>
   /// for debugging
   /// </summary>
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      var position = transform.position;
      Gizmos.DrawRay(position, transform.up * 2);
      
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(position, m_CircleCollider2D.radius);
   }
   IEnumerator DestroyMe()
   {
      yield return new WaitForSecondsRealtime(.5f);
      Destroy(gameObject);
   }
}
