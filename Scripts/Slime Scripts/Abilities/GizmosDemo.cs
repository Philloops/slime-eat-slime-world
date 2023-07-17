using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosDemo : MonoBehaviour
{
    public float size;
    public Color color;

    void OnEnable()
    {

    }
    void OnDisable()
    {

    }
    public void OnDrawGizmos()
    {
        if(enabled)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, size);
        }     
    }
}
