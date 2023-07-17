using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTransition : MonoBehaviour
{
    public float transitionSpd;
    private bool fadedIn;
    public bool transition;
    public bool Transition
    {
        get
        {
            if (transitionImg.color.a <= 0)
            {
                if(!fadedIn)
                    transition = true;
                else
                    gameObject.SetActive(false);
            }

            if (transitionImg.color.a >= 1)
            {
                fadedIn = true;
                transition = false;
            }

            return transition;
        }
    }
    public Image transitionImg;
    public Color color;

    void OnDisable()
    {
        color.a = 0;
        transitionImg.color = color;
        fadedIn = false;
    }
    void Update()
    {
        if(Transition)
        {
            if(transitionImg.color.a < 1)
                color.a += Time.deltaTime * transitionSpd;
            transitionImg.color = color;
        }
        if(!Transition)
        {
            if (transitionImg.color.a > 0)
                color.a -= Time.deltaTime * transitionSpd;
            transitionImg.color = color;
        }
    }
}
