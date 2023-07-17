using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledImpactObject : MonoBehaviour
{
    public Transform parentObject;
    public float duration;
    private float life;

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            life += Time.deltaTime;
            if(life >= duration)
            {
                transform.parent = parentObject;
                gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        life = 0;
        transform.position = parentObject.position;
        transform.rotation = parentObject.rotation;
    }
}
