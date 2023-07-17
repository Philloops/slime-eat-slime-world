using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageImpactObject : PooledImpactObject
{
    public float radius;
    public float castOffset;
    public LayerMask desiredLayers;
    public int slimeLayer;
    public bool showGizmos;

    private float mass = 3;
    private int damage;
    private float knockbackForce;
    private Collider[] targets;
    private BaseProjectile myProjectile;

    void OnEnable()
    {
        if (myProjectile == null)
            myProjectile = parentObject.GetComponent<BaseProjectile>();

        //damage = myProjectile.damage / 3;
        knockbackForce = 120 / 3;

        Vector3 offset = transform.position - transform.forward * castOffset;
        targets = Physics.OverlapSphere(offset, radius, desiredLayers);
        if(targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if(targets[i].gameObject != myProjectile.MySlime.gameObject)
                {
                    if(targets[i].gameObject.layer == slimeLayer)
                    {
                        Slime hitSlime = targets[i].GetComponent<Slime>();
                        hitSlime.TakeDamage(damage);
                        Vector3 dir = targets[i].transform.position - offset;
                        float force = Mathf.Clamp(knockbackForce / mass, 0, knockbackForce);
                        hitSlime.MyStatusControls.RequestImpact(dir, force);
                    }
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position - transform.forward * castOffset, radius);
        }
    }
}
