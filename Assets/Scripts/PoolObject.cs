using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{

    [SerializeField] private GameObject _gameObject;
    [SerializeField] private int _quantity;

    private GameObject[] _pool;

    private void Awake()
    {
        _pool = new GameObject[_quantity];

        for (int i = 0; i < _quantity; i++)
        {
            _pool[i] = Instantiate(_gameObject, transform);
            _pool[i].SetActive(false);
        }
        
    }

    public GameObject GetBullet()
    {
        
        for (int i = 0; i < _quantity; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);
                return _pool[i];
            }
        }

        return null;
    }
}
