using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _bulletSpeed = 6f;
    public Vector2 Direction=Vector2.right;
    [SerializeField] private LayerMask _damageLayer;
    [SerializeField] private float _radius=1f;
    [SerializeField] public int Damage = 3;
   
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Direction*(_bulletSpeed*Time.deltaTime));



        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _radius, Vector3.back, Mathf.Infinity, _damageLayer);
        if (hit)
        {
            hit.collider.gameObject.GetComponentInParent<Health>().TakeDamage(Damage);
            
            
            gameObject.SetActive(false); 
            
        }else
        if (Physics2D.CircleCast(transform.position, _radius,Vector3.back))
        {
            gameObject.SetActive(false); 
        }

    }


}
