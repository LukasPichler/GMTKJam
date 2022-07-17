using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    [SerializeField] public AudioClip Clip;
    [SerializeField] private AudioSource _source;

    [SerializeField] private float _maxPicth = 1f;
    [SerializeField] private float _minPicth = 1f;

    [SerializeField] private float _maxVolume = 1f;
    [SerializeField] private float _minVolume = 1f;

    
    public void Play()
    {
        _source.pitch = Random.Range(_minPicth, _maxPicth);
        _source.volume = Random.Range(_minVolume, _maxVolume);
        _source.PlayOneShot(Clip);
        
    }
}
