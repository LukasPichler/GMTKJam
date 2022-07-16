using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CipEnemy : MonoBehaviour
{
    [SerializeField]private Transform _target;

    [SerializeField] private float _jumpDistance;
    private float _jumpDistancesqrt;
    private Vector2 _jumppoint;

    private Vector2 _startPoint;

    [SerializeField] private float _jumpTime=1.3f;
    private float _jumpTimeClock = 0f;
    
    [SerializeField] private float _jumpCD=2f;

    private float _jumpClock=0f;

    private void Awake()
    {
        _jumppoint = transform.position;
        _startPoint = transform.position;
        _jumpDistancesqrt = _jumpDistance * _jumpDistance;
    }

    void Update()
    {
        _jumpClock += Time.deltaTime;
        _jumpTimeClock += Time.deltaTime;
        
        if (_jumpClock > _jumpCD)
        {
            _jumpClock = 0f;
            _jumpTimeClock = 0f;
            if ((_target.position - transform.position).sqrMagnitude < _jumpDistancesqrt)
            {
                _jumppoint = _target.position;
                Debug.Log(_jumppoint);
            }
            else
            {
                _jumppoint = (_target.position - transform.position).normalized * _jumpDistance;
                Debug.Log(_jumppoint+"  Slef");
            }

            _startPoint = transform.position;
        }

        transform.position = Vector2.Lerp(_startPoint, _jumppoint, _jumpTimeClock/_jumpTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,transform.position+(Vector3.left*_jumpDistance));
    }
}
