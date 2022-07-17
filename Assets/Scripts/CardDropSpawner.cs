using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _card;

    [SerializeField] private Vector2 _startPos;
    [SerializeField] private Vector2 _startPosOffset;

    [SerializeField] private Vector2 _endPos;
    [SerializeField] private Vector2 _endPosOffset;
    
    [SerializeField] private float _spawnMinTime = 5f;
    [SerializeField] private float _spawnMaxTime = 7f;
    [SerializeField] private AnimationCurve _spawnAmount;
    [SerializeField] private Transform _target;
    
    
    private float _spawnTime;
    [SerializeField] private float _clockSpawnTime = 0f;
    
    private int _spawnCount;


    private void Awake()
    {
        _spawnTime = Random.Range(_spawnMinTime, _spawnMaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        _clockSpawnTime += Time.deltaTime;
        if (_clockSpawnTime > _spawnTime)
        {
            _clockSpawnTime = 0f;
            _spawnTime = Random.Range(_spawnMinTime, _spawnMaxTime);

            Vector2 endPos = new Vector2(Random.Range(-_endPosOffset.x, _endPosOffset.x) + _endPos.x,
                Random.Range(-_endPosOffset.y, _endPosOffset.y) + _endPos.y);
            Vector2 startPos = new Vector2(endPos.x,
                Random.Range(-_startPosOffset.y, _startPosOffset.y) + _startPos.y);

            GameObject spawn = Instantiate(_card);

            CardDrop drop = spawn.GetComponent<CardDrop>();
            spawn.GetComponentInChildren<SpawnObjectAndSetTarget>().Target = _target;
            drop.StartPos = startPos;
            drop.EndPos = endPos;
            drop.OnInstatiate();

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawCube(_startPos,_startPosOffset*2f);
        
        
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawCube(_endPos,_endPosOffset*2f);
        
    }
}
