using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Animations;
using Random = UnityEngine.Random;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Animator _animatorPlayer;
    [SerializeField] private Health _health;
    
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
    [SerializeField] private Animator _animatorGun;
    [SerializeField] private AnimationCurve _sizeOfBulletNR;
    
    [SerializeField] private float _collisionDistance=0.5f;
    [SerializeField] private LayerMask _collisionMask;

    private int _rollNr=1;

    [SerializeField] private AnimationCurve _rollNrMovementSpeed;
    [SerializeField] private ChangeSpriteAfterRoll _changeSpriteAfterRoll;
    [SerializeField] private Transform _gun;
    [SerializeField] private AnimationCurve _sizeOfGunAfterRoll;
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Player.Enable();
        _playerInputAction.Player.Dash.performed += Dash;
        _health.SubscribeToTookDamage(TakeDamage);
        _health.SubscribeToDeath(Death);
        _health.SubscribeToVincible(TakeDamageAgain);
    }

    private void OnEnable()
    {
        _health.SubscribeToTookDamage(TakeDamage);
        _health.SubscribeToDeath(Death);
        _health.SubscribeToVincible(TakeDamageAgain);
    }

    private void OnDisable()
    {
        _health.UnSubscribeToTookDamage(TakeDamage);
        _health.UnSubscribeToDeath(Death);
        _health.UnSubscribeToVincible(TakeDamageAgain);
    }

    private void Update()
    {
        
        
        Vector2 movement = _playerInputAction.Player.Move.ReadValue<Vector2>();
        
        _dashClock += Time.deltaTime;
        _shootClock += Time.deltaTime;
        if (_isDashing)
        {
            var dashDirection = CollisionCheck(_lastDirectionMoved);
            transform.Translate(dashDirection*(_dashSpeed*Time.deltaTime));
            
            
            _clockDashDuration += Time.deltaTime;
            if (_clockDashDuration > _dashDuration)
            {
                _isDashing = false;
                _animatorPlayer.SetBool("Rolling",false);
                _clockDashDuration = 0f;
                _changeSpriteAfterRoll.ChangeSprite(_rollNr);
            }
        }
        else
        {
            Move(movement);
            if (_shootCD<_shootClock && _playerInputAction.Player.Shoot.ReadValue<float>()>0f)
            {
                _shootClock = 0f;
                Shoot();
                _animatorGun.SetBool("Shoot",true);
            }
            else
            {
                _animatorGun.SetBool("Shoot",false);
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
        _currentSpeed = CollisionCheck(_currentSpeed);
        transform.Translate(_currentSpeed*(Time.deltaTime*_rollNrMovementSpeed.Evaluate(_rollNr)));
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (_dashClock > _dashCD)
        {
            _dashClock = 0f;
            _isDashing = true;
            _animatorPlayer.SetBool("Rolling",true);
            int newNr;
            do
            {
                newNr = Random.Range(1, 6);
            } while (newNr == _rollNr);

            _rollNr = newNr;
            
        }
    }


    private void Shoot()
    {
        GameObject bulletGameobject = _bulletPool.GetBullet();
        var position = _shootingPoint.position;
        bulletGameobject.transform.position = position;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
 
        Vector3 objectPos = Camera.main.WorldToScreenPoint (position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        Vector2 direction = (Vector2)(mousePos - position);
        direction = direction.normalized;
        bulletGameobject.transform.localScale = Vector3.one*_sizeOfBulletNR.Evaluate(_rollNr);
        _gun.localScale = Vector3.one * _sizeOfGunAfterRoll.Evaluate(_rollNr);
        Bullet bullet = bulletGameobject.GetComponent<Bullet>();
        bullet.Direction = direction;
        bullet.Damage = _rollNr;
    }

    private Vector2 CollisionCheck(Vector2 move)
    {
        if (move.x>0 && Physics2D.Raycast(transform.position, Vector2.right, _collisionDistance, _collisionMask))
        {
            move.x = 0;
        }
        if (move.x<0 && Physics2D.Raycast(transform.position, Vector2.left, _collisionDistance, _collisionMask))
        {
            move.x = 0;
        }
        if (move.y>0 && Physics2D.Raycast(transform.position, Vector2.up, _collisionDistance, _collisionMask))
        {
            move.y = 0;
        }
        if (move.y<0 && Physics2D.Raycast(transform.position, Vector2.down, _collisionDistance, _collisionMask))
        {
            move.y = 0;
        }

        return move;
    }

    private void TakeDamage()
    {
        _animatorPlayer.SetBool("Hurt",true);
    }

    private void TakeDamageAgain()
    {
        _animatorPlayer.SetBool("Hurt",false);
    }
    
    private void Death()
    {
        _animatorPlayer.SetBool("Dead",true);
        this.enabled = false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawLine(position,(Vector2)position+(Vector2.right*_collisionDistance));
    }
}
