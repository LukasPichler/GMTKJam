using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{

    private PlayerInput _playerInput;
    private PlayerInputAction _playerInputAction;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private AnimationCurve _acceleration;
    [SerializeField] private float _accelerationTimeAmplifirer = 1.2f;
    [SerializeField] private AnimationCurve _decceleration;
    [SerializeField] private float _deccelerationTimeAmplifirer = 1.2f;
    private float _currentX=0f;
    private Vector2 _currentSpeed = Vector2.zero;


    [SerializeField] private float _dashCD=1f;
    private float _dashClock=0f;
    [SerializeField] private float _dashSpeed=15f;
    [SerializeField] private float _dashDuration = 0.3f;
    private float _clockDashDuration = 0f;
    private bool _isDashing = false;
    private Vector2 _lastDirectionMoved = Vector2.right;


    [SerializeField] private float _shootCD=0.3f;
    [SerializeField] private float _shootClock = 0f;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private PoolObject _bulletPool;
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Player.Enable();
        _playerInputAction.Player.Dash.performed += Dash;
    }


    private void Update()
    {
        
        
        Vector2 movement = _playerInputAction.Player.Move.ReadValue<Vector2>();
        
        _dashClock += Time.deltaTime;
        _shootClock += Time.deltaTime;
        if (_isDashing)
        {
            
            transform.Translate(_lastDirectionMoved*(_dashSpeed*Time.deltaTime));
            
            
            _clockDashDuration += Time.deltaTime;
            if (_clockDashDuration > _dashDuration)
            {
                _isDashing = false;
                _clockDashDuration = 0f;
            }
        }
        else
        {
            Move(movement);
            if (_shootCD<_shootClock && _playerInputAction.Player.Shoot.ReadValue<float>()>0f)
            {
                _shootClock = 0f;
                Shoot();
            }
        }
    }


    private void Move(Vector2 movement)
    {
        
        if (movement.sqrMagnitude > 0)
        {
            _lastDirectionMoved = movement;
            _currentX = Mathf.Min(Time.deltaTime*movement.magnitude*_accelerationTimeAmplifirer + _currentX,1);
            _currentSpeed = movement * (_acceleration.Evaluate(_currentX) * _maxSpeed);
        }
        else
        {
            _currentX = Mathf.Max( _currentX-Time.deltaTime*_deccelerationTimeAmplifirer ,0);
            _currentSpeed = _currentSpeed.normalized *(_decceleration.Evaluate(_currentX) * _maxSpeed);
        }
        //Debug.Log(_currentSpeed);
        transform.Translate(_currentSpeed*Time.deltaTime);
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (_dashClock > _dashCD)
        {
            _dashClock = 0f;
            _isDashing = true;
        }
    }


    private void Shoot()
    {
        GameObject bullet = _bulletPool.GetBullet();
        var position = _shootingPoint.position;
        bullet.transform.position = position;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
 
        Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        Vector2 direction = (Vector2)(mousePos - position);
        direction = direction.normalized;

        bullet.GetComponent<Bullet>().Direction = direction;
    }
    
}
