using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CipEnemy : MonoBehaviour
{
    [SerializeField] public Transform Target;
    [SerializeField] private Health _health;
    [SerializeField] private float _jumpDistance;
    private float _jumpDistancesqrt;
    private Vector2 _jumppoint;

    [SerializeField]private DamageNear _damageNear;
    private Vector2 _startPoint;

    [SerializeField] private float _jumpTime=1.3f;
    private float _jumpTimeClock = 0f;
    
    [SerializeField] private float _jumpCD=2f;

    [SerializeField] private Transform _spriteTransform;
    
    [SerializeField] private Transform _facingTransform;
    [SerializeField] private Transform _shadow;
    
    
    [SerializeField] private AnimationCurve _scaleOverJump;
    [SerializeField] private AnimationCurve _yOverJump;

    private Vector2 _shadowStartPoint;
    
    private float _jumpClock=0f;

    private bool _alive = true;

    private void Awake()
    {
        _jumppoint = transform.position;
        _startPoint = transform.position;
        _jumpDistancesqrt = _jumpDistance * _jumpDistance;
        _shadowStartPoint = _shadow.position-transform.position;
        
        _health.SubscribeToDeath(OnDeath);
    }

    private void OnEnable()
    {
        
        _health.SubscribeToDeath(OnDeath);
    }

    private void OnDisable()
    {
        
        _health.UnSubscribeToDeath(OnDeath);
    }

    void Update()
    {
      
            _jumpClock += Time.deltaTime;
            _jumpTimeClock += Time.deltaTime;

            if (_jumpClock > _jumpCD && _alive)
            {
                _jumpClock = 0f;
                _jumpTimeClock = 0f;
                if ((Target.position - transform.position).sqrMagnitude < _jumpDistancesqrt)
                {
                    _jumppoint = Target.position;
                }
                else
                {
                    _jumppoint = (Target.position - transform.position).normalized * _jumpDistance +
                                 transform.position;

                }

                _startPoint = transform.position;

                Vector3 scale = _facingTransform.localScale;

                if (_startPoint.x > _jumppoint.x)
                {
                    _facingTransform.localScale = new Vector3(Math.Abs(scale.x), scale.y, scale.z);
                }
                else
                {
                    _facingTransform.localScale = new Vector3(-Math.Abs(scale.x), scale.y, scale.z);
                }

            }

            float t = _jumpTimeClock / _jumpTime;
            if ((t < 0.1f || t > 0.9f) && _alive)
            {
                _damageNear.enabled = true;
            }
            else
            {
                _damageNear.enabled = false;
            }
            
            transform.position = Vector2.Lerp(_startPoint, _jumppoint, t) + Vector2.up * _yOverJump.Evaluate(t);
            _shadow.position = new Vector2(_shadow.position.x,
                _shadowStartPoint.y + Vector2.Lerp(_startPoint, _jumppoint, t).y);

            _spriteTransform.localScale = Vector3.one * _scaleOverJump.Evaluate(t);
        
    }

    private void OnDeath()
    {
        _alive = false;
        _damageNear.enabled = false;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,transform.position+(Vector3.left*_jumpDistance));
    }
}
