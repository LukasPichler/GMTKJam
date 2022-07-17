using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _bulletSpeed = 6f;
    public Vector2 Direction=Vector2.right;
    [SerializeField] private LayerMask _damageLayer;
    [SerializeField] private LayerMask _collisionMask;
    [SerializeField] private float _radius=1f;
    [SerializeField] public int Damage = 3;
    [SerializeField] private float _lifeTime=5f;
    private float _clockLifeTime;

    private Action destruction;

    public virtual void SubscribeDestruction(System.Action call)
    {
        destruction += call;
    }
    
    public virtual void UnSubscribeDestruction(System.Action call)
    {
        destruction -= call;
    }
    
    
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Direction*(_bulletSpeed*Time.deltaTime));

        _clockLifeTime += Time.deltaTime;

        if (_clockLifeTime > _lifeTime)
        {
            destruction?.Invoke();
            Destroy(gameObject);
        }


        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _radius, Vector3.back, Mathf.Infinity, _damageLayer);
        if (hit)
        {
            hit.collider.gameObject.GetComponentInParent<Health>().TakeDamage(Damage);
            destruction?.Invoke();
            Destroy(gameObject);
            
        }else
        if (Physics2D.CircleCast(transform.position, _radius, Vector3.back, Mathf.Infinity, _collisionMask))
        {
            destruction?.Invoke();
            Destroy(gameObject);
        }

    }


}
