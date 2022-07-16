using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RotateTowardsMouse : MonoBehaviour
{
    [SerializeField] private Transform _gun;

    [SerializeField] private Transform _playerModelTrans;
    
    private bool _leftside = true;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
 
        Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
 
        
        
        
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;


        _leftside = angle > -90f && angle < 90f;

        Vector3 scaleOfPlayerModel = _playerModelTrans.localScale;
        Vector3 scaleOfGun = _gun.localScale;
        if (_leftside)
        {
            scaleOfPlayerModel = new Vector3(Mathf.Abs(scaleOfPlayerModel.x), scaleOfPlayerModel.y,
                scaleOfPlayerModel.z);
            scaleOfGun = new Vector3(scaleOfGun.x, Mathf.Abs(scaleOfGun.y),
                scaleOfGun.z);
        }
        else
        {
            scaleOfPlayerModel = new Vector3(-Mathf.Abs(scaleOfPlayerModel.x), scaleOfPlayerModel.y,
                scaleOfPlayerModel.z);
            scaleOfGun = new Vector3(scaleOfGun.x, -Mathf.Abs(scaleOfGun.y),
                scaleOfGun.z);
        }
        
        _playerModelTrans.localScale = scaleOfPlayerModel;
        _gun.localScale = scaleOfGun;
        _gun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
