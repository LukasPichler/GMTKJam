using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamagePopup : MonoBehaviour
{


    [SerializeField] private float _lifeTime=2f;
    [SerializeField] private AnimationCurve _sizeOverLifeTime;
    [SerializeField] private AnimationCurve _hightOverLifeTime;
    [SerializeField] private AnimationCurve _xDiretionOverLifeTime;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minX;

    private float _heightScale;
    private float _xScale;
    private Vector3 _startPos;
    
    private float _clock = 0f;
    
    public static DamagePopup Create(Vector3 position, int damageAmount)
    {
        GameObject damagePopupGameObject = Instantiate(GameAssets.Instance.DamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupGameObject.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);
       return damagePopup;
    }
    
    private TextMeshPro _textMeshPro;
    
    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        _heightScale = Random.Range(_minHeight, _maxHeight);
        _xScale = Random.Range(_minX, _maxX);
        _startPos = transform.position;
    }


    private void Update()
    {
        _clock += Time.deltaTime;
        transform.localScale = Vector3.one*_sizeOverLifeTime.Evaluate(_clock);
        transform.position = new Vector3(_xDiretionOverLifeTime.Evaluate(_clock) * _xScale,
            _hightOverLifeTime.Evaluate(_clock) * _heightScale, 0)+_startPos;
        if (_lifeTime < _clock)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(int damageAmount)
    {
        _textMeshPro.SetText(damageAmount.ToString());
    }
}
