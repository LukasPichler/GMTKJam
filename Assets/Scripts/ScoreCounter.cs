using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private float _clock=0f;

    private void Awake()
    {
        _text.text = 0 + "";
    }

    private void Update()
    {
        _clock += Time.deltaTime;
        if (_clock > 3f)
        {
            _clock = 0f;
            Score.Count++;
            _text.text = Score.Count.ToString();
        }
    }
}
