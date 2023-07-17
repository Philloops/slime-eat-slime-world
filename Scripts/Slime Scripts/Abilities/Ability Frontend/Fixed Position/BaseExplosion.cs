using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExplosionBehaviorModule
{
    public enum Behaviors { Damage, Heal, Root, Stun, Slow, Knockback }
    public List<Behaviors> behaviors = new List<Behaviors>();

    public int damageAmt;
    public int healAmt;
    public float rootDuration;
    public float stunDuration;
    public float slowDuration;
    public float slowIntensity;
    public float KnockbackForce;
}
public class BaseExplosion : PooledAbilityObject
{
    public ExplosionBehaviorModule module;
    public float startDelay;
    public float duration;
    public float radius;
    public LayerMask desiredLayers;
    public List<Transform> castPositions;
    public bool showGizmos;

    protected List<GameObject> hitTargets = new List<GameObject>();
    protected float currentTime;
    protected Collider[] targets;
    protected bool allowCasting;

    void Update()
    {
        if(allowCasting)
        {
            OnCastBehavior();

            currentTime += Time.deltaTime;
            if (currentTime >= duration)
                gameObject.SetActive(false);
        }
    }
    void OnEnable()
    {
        if(startDelay > 0)
            Invoke("BeginStartDelay", startDelay);
    }
    void BeginStartDelay()
    {
        allowCasting = true;

        if (module.behaviors.Contains(ExplosionBehaviorModule.Behaviors.Heal))
            ApplyHeal();
    }
    void OnDisable()
    {
        allowCasting = false;
        currentTime = 0;

        if(hitTargets.Count > 0)
            hitTargets.Clear();

        transform.parent = MyParent;
    }
    private bool ContainerCheck(GameObject _obj)
    {
        if (hitTargets.Count > 0)
        {
            if (!hitTargets.Contains(_obj))
                return true;
            else
                return false;
        }
        return true;
    }
    public virtual void OnCastBehavior()
    {
        for (int i = 0; i < castPositions.Count; i++)
        {
            targets = Physics.OverlapSphere(castPositions[i].position, radius, desiredLayers);
            if(targets.Length > 0)
            {
                for (int j = 0; j < targets.Length; j++)
                {
                    if(targets[j].gameObject != MySlime.gameObject)
                    {
                        if(ContainerCheck(targets[j].gameObject))
                        {
                            hitTargets.Add(targets[j].gameObject);
                            Slime hitSlime = targets[j].GetComponent<Slime>();

                            if (module.behaviors.Contains(ExplosionBehaviorModule.Behaviors.Damage))
                                ApplyDamage(hitSlime);
                            if (module.behaviors.Contains(ExplosionBehaviorModule.Behaviors.Root))
                                ApplyRoot(hitSlime);
                            if (module.behaviors.Contains(ExplosionBehaviorModule.Behaviors.Stun))
                                ApplyStun(hitSlime);
                            if (module.behaviors.Contains(ExplosionBehaviorModule.Behaviors.Slow))
                                ApplySlow(hitSlime);
                            if (module.behaviors.Contains(ExplosionBehaviorModule.Behaviors.Knockback))
                                ApplyKnockback(hitSlime);
                        }
                    }
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            if(castPositions.Count > 0)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < castPositions.Count; i++)
                    Gizmos.DrawSphere(castPositions[i].position, radius);
            }        
        }
    }
    #region Behavior Methods
    private void ApplyDamage(Slime _hitSlime)
    {
        float trueDamage = 0;
        //Power buff gives 20% damage buff
        if (MySlime.MyStatusControls.ApplyPower)
            trueDamage = module.damageAmt * 1.2f;
        else
            trueDamage = module.damageAmt;

        _hitSlime.TakeDamage(trueDamage);
    }
    private void ApplyRoot(Slime _hitSlime)
    {
        _hitSlime.MyStatusControls.SetRootDuration(module.rootDuration);
    }
    private void ApplyStun(Slime _hitSlime)
    {
        _hitSlime.MyStatusControls.SetStunDuration(module.stunDuration);
    }
    private void ApplySlow(Slime _hitSlime)
    {
        _hitSlime.MyStatusControls.SetSlowDuration(module.slowDuration, module.slowIntensity);
    }
    private void ApplyKnockback(Slime _hitSlime)
    {
        Vector3 dir = _hitSlime.transform.position - transform.position;
        _hitSlime.MyStatusControls.RequestImpact(dir, module.KnockbackForce);
    }
    private void ApplyHeal()
    {
        if (MySlime != null)
            MySlime.HealDamage(module.healAmt);
        else
            Debug.LogError("My slime was null inside baseexplosion during enable heal cast");
    }
    #endregion
}
