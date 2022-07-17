using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int _maxHealth=10;
    private int _currentHealth;
    [SerializeField] private Transform _healthbar;

    [SerializeField] private float _damageImmune=0f;
    private float _clockDamageImmune;
    private bool invincible=false;
    private bool invicibleDahs = false;
    
    private Action _tookDamage;
    private Action _dead;
    
    private Action _vincible;
    
    private void Awake()
    {
        _currentHealth = _maxHealth;
        _clockDamageImmune = _damageImmune;
    }

    private void Update()
    {
        _clockDamageImmune += Time.deltaTime;
        if (_clockDamageImmune > _damageImmune)
        {
            invincible = false;
            _vincible?.Invoke();
        }
    }

    public void SubscribeToTookDamage(System.Action call)
    {
        _tookDamage += call;
    }
    
    public void SubscribeToVincible(System.Action call)
    {
        _vincible += call;
    }
    
    public void SubscribeToDeath(System.Action call)
    {
        _dead += call;
    }
    
    public void UnSubscribeToTookDamage(System.Action call)
    {
        _tookDamage -= call;
    }
    
    public void UnSubscribeToVincible(System.Action call)
    {
        _vincible -= call;
    }
    
    public void UnSubscribeToDeath(System.Action call)
    {
        _dead -= call;
    }

    public void MakeInvurnable(float seconds)
    {
        StartCoroutine(InvurnableForTime(seconds));
    }

    IEnumerator InvurnableForTime(float time)
    {
        invicibleDahs = true;
        yield return new WaitForSeconds(time);
        invicibleDahs = false;
    }
    
    public void TakeDamage(int damage)
    {
        if (!invincible && !invicibleDahs)
        {
            int diff = _currentHealth - Mathf.Max(0, _currentHealth - damage);
            if (diff > 0)
            {
                _currentHealth -= diff;
                _tookDamage?.Invoke();
                _clockDamageImmune = 0;
                invincible = true;
                DamagePopup.Create(transform.position, diff);
            }


            if (_currentHealth <= 0)
            {
                _dead?.Invoke();
            }

            _healthbar.localScale = new Vector3((float)_currentHealth / (float)_maxHealth, 1f, 1f);
        }
    }
}
