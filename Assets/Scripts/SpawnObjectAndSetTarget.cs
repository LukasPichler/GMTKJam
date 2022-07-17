using System.Collections;
using System.Collections.Generic;
using GameJam.Enemies;
using UnityEngine;

public class SpawnObjectAndSetTarget : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Transform _destroy;
    
    public Transform Target;


    public void Spawn()
    {
        GameObject gameObject = Instantiate(_gameObject, transform.position, Quaternion.identity);
        gameObject.GetComponent<EnemyAI>().playerTarget = Target;
        Destroy(_destroy.gameObject);
    }
}
