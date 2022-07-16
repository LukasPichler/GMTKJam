using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
       
        private Vector3 targetPosition;
        private float distanceUntilDestroy = 0.1f;

        private void Start()
        {
            targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        }

        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position,targetPosition) < distanceUntilDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
