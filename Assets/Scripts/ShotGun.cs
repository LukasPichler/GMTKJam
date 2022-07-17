using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotGun", menuName = "Gun/ShotGun", order = 1)]
public class ShotGun : GunObject
{
    public int _bulletCount = 6;
    public float _arcOfShot = 30f;
    
    
    public override void Shoot(Vector3 target, Vector3 shootingPoint, AudioPlay audioPlayer)
    {
        
        audioPlayer.Clip = AudioClip;
        audioPlayer.Play();
        
        Vector2 defaulDirection = (target - shootingPoint).normalized;
        for (int i = 0; i < _bulletCount; i++)
        {
            
            GameObject bulletObject = Instantiate(Bullet);
            bulletObject.transform.position = shootingPoint;
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            float degree =  (i * 2 * _arcOfShot / _bulletCount)-_arcOfShot;
            Vector2 newDirection = Rotate(defaulDirection, degree);
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
