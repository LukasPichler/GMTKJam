using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CipsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cip;
    [SerializeField] private float _spawnMinTime = 5f;
    [SerializeField] private float _spawnMaxTime = 7f;
    [SerializeField] private AnimationCurve _spawnAmount;
    [SerializeField] private Vector2 _maxSpawnPoint;
    [SerializeField] private Transform _target;
    
    
    private float _spawnTime;
    [SerializeField] private float _clockSpawnTime = 0f;
    
    private int _spawnCount;

    private void Awake()
    {
        _spawnTime = Random.Range(_spawnMinTime, _spawnMaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        _clockSpawnTime += Time.deltaTime;
        if (_clockSpawnTime > _spawnTime)
        {
            _clockSpawnTime = 0f;
            _spawnTime = Random.Range(_spawnMinTime, _spawnMaxTime);
            int amount = (int)_spawnAmount.Evaluate(_spawnCount);
            _spawnCount++;
            for (int i = 0; i < amount; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-_maxSpawnPoint.x, _maxSpawnPoint.x), Random.Range(-_maxSpawnPoint.y, _maxSpawnPoint.y),0f);

                GameObject gameObject = Instantiate(_cip, transform.position + offset, Quaternion.identity);
                gameObject.GetComponent<CipEnemy>().Target = _target;
            }
            
        }
    }
}
