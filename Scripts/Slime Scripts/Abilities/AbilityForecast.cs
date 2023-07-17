using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityForecast : MonoBehaviour
{
    public Transform Scaler { get; set; }
    public GameObject visualObj;
    public GameObject collisionObj;

    public void Initialize()
    {
        Scaler = transform.GetChild(0);
    }

    public void EnableVisual(bool _state)
    {
        visualObj.SetActive(_state);
    }
    public void EnableVisual(bool _state, Vector3 _scale)
    {
        Scaler.transform.localScale = _scale;
        visualObj.SetActive(_state);
    }
   
    public void EnableCollision(bool _state)
    {
        collisionObj.SetActive(_state);
    }
}
