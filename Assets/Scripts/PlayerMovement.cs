using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Animations;
using Random = UnityEngine.Random;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<GunObject> _guns;
    [SerializeField] private Transform _gunParent;
    private GameObject _currentGun;
    private GunObject _currentGunObject;
    private Transform _shootingPoint;
    private AudioPlay _audioPlay;
    [SerializeField] private RotateTowardsMouse _rotateTowards;
    private Action _rollAction;
    private Action _stopRollAction;
    private int _magazineSize;
    
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
    [SerializeField] private Animator _animatorGun;
    
    [SerializeField] private float _collisionDistance=0.5f;
    [SerializeField] private LayerMask _collisionMask;

    private int _rollNr=1;

    [SerializeField] private ChangeSpriteAfterRoll _changeSpriteAfterRoll;
    
    private void Awake()
    {
        LoadGun(_rollNr);
        
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
        _text.text = _magazineSize.ToString();
        
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
                _stopRollAction?.Invoke();
                _animatorPlayer.SetBool("Rolling",false);
                _clockDashDuration = 0f;
                _changeSpriteAfterRoll.ChangeSprite(_rollNr);
                LoadGun(_rollNr);
            }
        }
        else
        {
            Move(movement);
            if (_magazineSize >0 && _shootCD<_shootClock && _playerInputAction.Player.Shoot.ReadValue<float>()>0f)
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
        transform.Translate(_currentSpeed*(Time.deltaTime));
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (_dashClock > _dashCD)
        {
            _audioSource.Play();
            _health.MakeInvurnable(_dashDuration);
            _dashClock = 0f;
            _isDashing = true;
            _animatorPlayer.SetBool("Rolling",true);
            _rollAction?.Invoke();
            int newNr;
            do
            {
                newNr = Random.Range(1, 7);
            } while (newNr == _rollNr);

            _rollNr = newNr;
            
        }
    }


    private void Shoot()
    {
        
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 5.23f;
        _currentGunObject.Shoot(mousePos,_shootingPoint.position,_audioPlay);
        _magazineSize--;


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

    private void LoadGun(int nr)
    {
        GunObject gunObject = _guns[nr-1];
        if (_currentGun != null)
        {
            Destroy(_currentGun);
        }
        _currentGun = Instantiate(gunObject.Gun, _gunParent);
        _rotateTowards.Gun = _currentGun.transform;
        _shootCD = gunObject.ShootCD;
        _animatorGun = _currentGun.GetComponentInChildren<Animator>();
        _currentGunObject = gunObject;
        _shootingPoint = _currentGun.transform.GetChild(0).GetChild(1);
        _audioPlay = _currentGun.GetComponent<AudioPlay>();
        _shootClock = 10f;
        _magazineSize = gunObject.bullets;

    }

    public void SubscribeRoll(System.Action call)
    {
        _rollAction += call;
    }
    public void UnSubscribeRoll(System.Action call)
    {
        _rollAction -= call;
    }
    
    public void SubscribeStopRoll(System.Action call)
    {
        _stopRollAction += call;
    }
    public void UnSubscribeStopRoll(System.Action call)
    {
        _stopRollAction -= call;
    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawLine(position,(Vector2)position+(Vector2.right*_collisionDistance));
    }
}
