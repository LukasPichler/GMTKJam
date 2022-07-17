using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExplosionSoundAfterBulletDestruction : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private Bullet _bullet;


    private void Awake()
    {
        _bullet.SubscribeDestruction(Explode);
    }

    private void OnEnable()
    {
        _bullet.SubscribeDestruction(Explode);
    }

    private void OnDisable()
    {
        
        _bullet.UnSubscribeDestruction(Explode);
    }


    private void Explode()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
    }

}
