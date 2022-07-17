using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private Health _health;


    private void Awake()
    {
        if (_health != null)
        {
            _health.SubscribeToDeath(Dead);
            _health.SubscribeToTookDamage(Damaged);
        }
    }

    private void OnEnable()
    {
        if (_health != null)
        {
            _health.SubscribeToDeath(Dead);
            _health.SubscribeToTookDamage(Damaged);
        }
    }


    private void OnDisable()
    {
        if (_health != null)
        {
            _health.UnSubscribeToDeath(Dead);
            _health.UnSubscribeToTookDamage(Damaged);
        }
    }

    private void Dead()
    {
        _animator.SetBool("Dead",true);
    }

    private void Damaged()
    {
        StartCoroutine(DamagedPhase());
    }

    IEnumerator DamagedPhase()
    {
        _animator.SetBool("Damaged",true);
        yield return new WaitForSeconds(0.1f);
        
        _animator.SetBool("Damaged",false);

    }
}
