using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int _maxHealth=10;
    private int _currentHealth;
    [SerializeField] private Transform _healthbar;


    private Action _tookDamage;
    private Action _dead;
    
    private void Awake()
    {
        _currentHealth = _maxHealth;
    }



    public void SubscribeToTookDamage(System.Action call)
    {
        _tookDamage += call;
    }
    
    public void SubscribeToDeath(System.Action call)
    {
        _dead += call;
    }
    
    public void TakeDamage(int damage)
    {
        int diff= _currentHealth-Mathf.Max(0, _currentHealth - damage);
        Debug.Log(diff);
        if (diff>0)
        {
            _currentHealth -= diff;
            _tookDamage.Invoke();
            DamagePopup.Create(transform.position, diff);
        }
        
        
        if (_currentHealth <= 0)
        {
            _dead.Invoke();
            Debug.Log("Dead");
        }

        _healthbar.localScale = new Vector3((float)_currentHealth / (float)_maxHealth, 1f, 1f);
    }
}
