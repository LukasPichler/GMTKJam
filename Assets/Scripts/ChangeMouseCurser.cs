using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMouseCurser : MonoBehaviour
{
  
    void Start()
    {
        Cursor.visible = false;
    }


    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 5.23f;
 

        transform.position = mousePos;
    }
}
