using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteAfterRoll : MonoBehaviour
{
    [SerializeField]private List<Sprite> _sprites;
    [SerializeField] private SpriteRenderer _renderer;

 

    public void ChangeSprite(int nr)
    {
        _renderer.sprite = _sprites[nr-1];
    }
    
}
