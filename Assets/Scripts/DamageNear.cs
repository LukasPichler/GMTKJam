using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNear : MonoBehaviour
{

    [SerializeField]private LayerMask _damageMask;
    [SerializeField]private int _damage;
    [SerializeField] private float _radius;
    
    void Update()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _radius, Vector3.back, Mathf.Infinity, _damageMask);
        if (hit)
        {
            hit.collider.gameObject.GetComponentInParent<Health>().TakeDamage(_damage);
            
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position,_radius);
    }
}
