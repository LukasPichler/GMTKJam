using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteAfterRoll : MonoBehaviour
{
    [SerializeField]private List<Sprite> _sprites;
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private List<Sprite> _uiSprite;
    [SerializeField] private Image _image;

    public void ChangeSprite(int nr)
    {
        _renderer.sprite = _sprites[nr-1];
        _image.sprite = _uiSprite[nr - 1];
    }
    
}
