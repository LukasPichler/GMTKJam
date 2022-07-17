using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granate : Bullet
{

    [SerializeField] private float _throwDistance = 2f;

    
    [SerializeField] private Transform _shadow;
    [SerializeField] private Transform _scale;
    
    [SerializeField] private AnimationCurve _scaleOverJump;
    [SerializeField] private AnimationCurve _yOverJump;
    private Vector2 _startPos;
    
    [SerializeField] private float _timeExplode=1.5f;
    private float _clockExplode=0f;

    [SerializeField] private float _timeThrow=0.7f;
    private float _clockThrow=0f;
    
    private Action _explode;

    public override void SubscribeDestruction(Action call)
    {
        _explode += call;
    }

    public override void UnSubscribeDestruction(Action call)
    {
        _explode -= call;
    }


    private Vector2 _target;
    private Vector3 _shadowStartPoint;

    private void Start()
    {
        
        _shadowStartPoint = _shadow.position-transform.position;
        _target = Direction * _throwDistance+(Vector2)transform.position;
        _startPos = transform.position;
    }

    void Update()
    {
        _clockExplode += Time.deltaTime;
        _clockThrow += Time.deltaTime;
        float t = _clockThrow / _timeThrow;
        transform.position = Vector2.Lerp(_startPos, _target, t)+Vector2.up*_yOverJump.Evaluate(t);
        _scale.localScale = Vector3.one * _scaleOverJump.Evaluate(t);
        
        _shadow.position = new Vector2(_shadow.position.x,
            _shadowStartPoint.y + Vector2.Lerp(_startPos, _target, t).y);
        
        if (_clockExplode > _timeExplode)
        {
            _explode?.Invoke();
            
            gameObject.SetActive(false);
        }

    }
}
