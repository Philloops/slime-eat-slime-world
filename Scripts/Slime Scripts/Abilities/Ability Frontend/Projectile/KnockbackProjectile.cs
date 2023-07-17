using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackProjectile : BaseProjectile
{
    [Header("Inherit Dependents")]
    public float knockbackForce;
    private float mass = 3;

    public override void OnCastBehavior()
    {
        Vector3 offset = transform.position - transform.forward * castOffset;
        targets = Physics.OverlapSphere(offset, radius, desiredLayers);
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if(targets[i].gameObject != MySlime.gameObject)
                {
                    if (targets[i].gameObject.layer == slimeLayer)
                    {
                        Slime hitSlime = targets[i].GetComponent<Slime>();
                        hitSlime.TakeDamage(10);
                        Vector3 dir = targets[i].transform.position - offset;
                        float force = Mathf.Clamp(knockbackForce / mass, 0, knockbackForce);
                        hitSlime.MyStatusControls.RequestImpact(dir, force);
                    }
                    OnImpact();
                    FadeOut();
                }                           
            }
        }
    }
}
