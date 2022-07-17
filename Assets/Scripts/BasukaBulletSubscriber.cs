using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasukaBulletSubscriber : MonoBehaviour
{
    [SerializeField]
    private SpawnBulletAround _spawn;

    [SerializeField] private Bullet _bullet;


    private void Awake()
    {
        _bullet.SubscribeDestruction(_spawn.Spawn);
    }

    private void OnEnable()
    {
        
        _bullet.SubscribeDestruction(_spawn.Spawn);
    }

    private void OnDisable()
    {
        _bullet.UnSubscribeDestruction(_spawn.Spawn);
        
    }
}
