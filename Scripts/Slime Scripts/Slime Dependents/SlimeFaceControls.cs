using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlimeFaceTextureModule
{
    public Texture defaultTexture;
    //Have a TextureSheetAnimation for animated facial expressions later on
}
public class SlimeFaceControls : MonoBehaviour
{
    public SlimeFaceTextureModule passiveModule;
    public SlimeFaceTextureModule unconciousModule;

    public enum FacialBehaviors { Passive, Unconcious }
    private FacialBehaviors facialBehavior;
    public FacialBehaviors FacialBehavior
    {
        get { return facialBehavior; }
        set
        {
            facialBehavior = value;
            FaceOnBehaviorChange();
        }
    }

    public Material animMaterial;
    public Renderer myRenderer;

    private bool _isAnimating;
    private int _currentFrame;
    private float _timer;
    private Material _animMaterial;
    private TextureSheetAnimation _anim;

    private void Start()
    {
        //creates instance of material so everything with original mat doesn't get animated
        _animMaterial = new Material(animMaterial);
        myRenderer.sharedMaterial = _animMaterial;
        _animMaterial.SetTexture("_MainTex", ReadModuleData());
    }
    private void Update()
    {
        if(_isAnimating)
        {
            _timer += Time.deltaTime;

            if(_currentFrame < _anim.frames.Length - 1 && _timer >= _anim.frameInterval)
            {
                _currentFrame++;
                _animMaterial.SetTexture("_MainTex", _anim.frames[_currentFrame]);
                _timer = 0;
            }
            else if (_currentFrame == _anim.frames.Length - 1)
            {
                _animMaterial.SetTexture("_MainTex", ReadModuleData());
                _isAnimating = false;
            }
        }
    }
    void FaceOnBehaviorChange()
    {
        _animMaterial.SetTexture("_MainTex", ReadModuleData());
    }
    private Texture ReadModuleData()
    {
        if (facialBehavior == FacialBehaviors.Unconcious)
            return unconciousModule.defaultTexture;
        //else if etc.....
        return passiveModule.defaultTexture;
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
