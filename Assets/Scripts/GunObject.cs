using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun/Default", order = 1)]
public class GunObject : ScriptableObject
{
    public float ShootCD = 1f;
    public GameObject Gun;
    public GameObject Bullet;
    public AudioClip AudioClip;
    public int bullets = 10;
    

    public virtual void Shoot(Vector3 target, Vector3 shootingPoint, AudioPlay audioPlayer)
    {
        audioPlayer.Clip = AudioClip;
        audioPlayer.Play();
        Vector2 direction = (target - shootingPoint).normalized;
        GameObject bulletObject = Instantiate(Bullet,shootingPoint,Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Direction = direction;
    }

}
