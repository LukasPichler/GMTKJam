using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    [SerializeField] private GameObject _gameObject;



    public void Spawn()
    {
        Instantiate(_gameObject, transform.position, Quaternion.identity);
        Destroy(gameObject.GetComponentInParent<EnemyAnimator>().gameObject);
    }
}
