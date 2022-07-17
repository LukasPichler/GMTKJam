using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;


    private void Awake()
    {
        _text.text = Score.Count.ToString();
    }
}
