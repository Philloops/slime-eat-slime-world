using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatusEffectUI
{
    public GameObject root;
    public GameObject positive;
    public GameObject negative;

    public void SetState(int _stateIndex)
    {
        negative.SetActive(false);
        positive.SetActive(false);

        if (_stateIndex < 0)
            negative.SetActive(true);
        else if(_stateIndex > 0)
            positive.SetActive(true);
    }
    public void Reset()
    {
        root.SetActive(false);
    }
}
