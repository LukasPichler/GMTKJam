using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBulletAround : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private int _amount;


    public void Spawn()
    {
        Vector2 defaultdirection = Vector2.up;
        float degreeDistance = 360 / _amount;
        
        for (int i = 0; i < _amount; i++)
        {
            GameObject bulletObject = Instantiate(_bullet);
            bulletObject.transform.position = transform.position;
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            Vector2 newDirection = Rotate(defaultdirection, degreeDistance*i);
            bullet.Direction = newDirection;
        }
    }
    
    private static Vector2 Rotate(Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
