using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSheetAnimator : MonoBehaviour
{
    public Texture defaultTexture;
    public Material animMaterial;

    private bool _isAnimating;
    private int _currentFrame;
    private float _timer;
    private Material _animMaterial;
    private TextureSheetAnimation _anim;

    private void Start()
    {
        //creates instance of material so everything with original mat doesn't get animated
        _animMaterial = new Material(animMaterial);
        GetComponent<Renderer>().sharedMaterial = _animMaterial;
    }
    private void Update()
    {
        if (_isAnimating)
        {
            _timer += Time.deltaTime;

            if (_currentFrame < _anim.frames.Length - 1 && _timer >= _anim.frameInterval)
            {
                _currentFrame++;
                _animMaterial.SetTexture("_MainTex", _anim.frames[_currentFrame]);
                _timer = 0;
            }
            else if (_currentFrame == _anim.frames.Length - 1)
            {
                _animMaterial.SetTexture("_MainTex", defaultTexture);
                _isAnimating = false;
            }
        }
    }

    public void animateTextureSheet(TextureSheetAnimation anim)
    {
        _anim = anim;
        _timer = 0;
        _currentFrame = 0;
        _animMaterial.SetTexture("_MainTex", anim.frames[0]);
        _isAnimating = true;
    }
}
