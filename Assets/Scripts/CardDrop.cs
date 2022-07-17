using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrop : MonoBehaviour
{
    [SerializeField] public Vector2 StartPos;

    [SerializeField] public Vector2 EndPos;

    
    [SerializeField] private Transform _card;

    [SerializeField] private Transform _shadow;

    [SerializeField] private float _speed = 3f;

    [SerializeField] private Animator _animator;
    

    public void OnInstatiate()
    {
        _shadow.position = EndPos;
        _card.position = StartPos;
    }
    
    void Update()
    {
        _card.position = Vector2.MoveTowards(_card.position, EndPos,_speed*Time.deltaTime);

        if (((Vector2)_card.position - EndPos).sqrMagnitude < 0.1f)
        {
            _animator.SetBool("Droped",true);
        }
        
    }
}
