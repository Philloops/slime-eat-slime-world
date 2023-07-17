using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillMeter: MonoBehaviour
{
    public Image image;

    public void SetFillMeter(int _value, int _max)
    {
        image.fillAmount = (_value / _max);
    }
    public void SetFillMeter(float _value, float _max)
    {
        image.fillAmount = (_value / _max);
    }
}
