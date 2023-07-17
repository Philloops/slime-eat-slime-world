using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlossomingBlades : MonoBehaviour
{
    public List<OrientateObject> pivotVisuals;

    public LayerMask desiredLayers;
    public float radius;
    //baseability ref
    private float delayReset = .35f;
    private float delay;
    private Collider[] targets;
    

    void Awake()
    {
        //for (int i = 0; i < pivotVisuals.Count; i++)
        //    pivotVisuals[i].Initialize();
    }

    void Update()
    {
        DamageCasts();

        //for (int i = 0; i < pivotVisuals.Count; i++)
        //    pivotVisuals[i].RotateObject();
    }

    private void DamageCasts()
    {
        if(gameObject.activeSelf)
        {
            delay -= Time.deltaTime;
            if(delay <= 0)
            {
                targets = Physics.OverlapSphere(transform.position, radius, desiredLayers);
                if(targets.Length > 0)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        //if has slime component and isn't slime who casted ability
                            //deal damage
                    }
                }
                delay = delayReset;
            }
        }
    }
}
