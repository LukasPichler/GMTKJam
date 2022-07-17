using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRollParticles : MonoBehaviour
{

    [SerializeField] private PlayerMovement _movement;

    [SerializeField] private GameObject _particles;


    private void Awake()
    {
        _movement.SubscribeRoll(Play);
        _movement.SubscribeStopRoll(Stop);
    }

    private void OnEnable()
    {
        
        _movement.SubscribeRoll(Play);
        _movement.SubscribeStopRoll(Stop);
    }

    private void OnDisable()
    {
        _movement.UnSubscribeRoll(Play);
        _movement.UnSubscribeStopRoll(Stop);
    }


    private void Play()
    {
        _particles.SetActive(true);
    }

    private void Stop()
    {
        
        _particles.SetActive(false);
    }
}
