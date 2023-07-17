using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunExplosion : BaseExplosion
{
    [Header("Stun Involved")]
    public float stunDuration;

    public override void OnCastBehavior()
    {
        for (int i = 0; i < castPositions.Count; i++)
        {
            targets = Physics.OverlapSphere(castPositions[i].position, radius, desiredLayers);
            if (targets.Length > 0)
            {
                for (int j = 0; j < targets.Length; j++)
                {
                    if (targets[j].gameObject != MySlime.gameObject)
                    {
                        Slime hitSlime = targets[j].GetComponent<Slime>();
                        if (hitTargets.Count > 0)
                        {
                            if (!hitTargets.Contains(targets[j].gameObject))
                            {
                                hitTargets.Add(targets[j].gameObject);
                                hitSlime.TakeDamage(10);
                                hitSlime.MyStatusControls.SetStunDuration(stunDuration);
                            }
                        }
                        else
                        {
                            hitTargets.Add(targets[j].gameObject);
                            hitSlime.TakeDamage(10);
                            hitSlime.MyStatusControls.SetStunDuration(stunDuration);
                        }
                    }
                }
            }
        }
    }
}
