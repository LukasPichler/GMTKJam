using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _bulletSpeed = 6f;
    public Vector2 Direction=Vector2.right;

   
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Direction*(_bulletSpeed*Time.deltaTime));
    }
}
