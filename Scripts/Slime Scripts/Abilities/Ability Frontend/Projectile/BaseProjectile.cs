using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileBehaviorModule
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
public class BaseProjectile : PooledAbilityObject
{
    public ProjectileBehaviorModule module;
    public PooledImpactObject impactObject;
    public bool allowCasting;
    public float lifeTime;
    public float speed;
    public LayerMask desiredLayers;
    public int slimeLayer;
    public float radius;
    public List<GameObject> disableOnImpact;
    public float castOffset;
    public bool showGizmos;

    protected float currentLife;
    protected bool begunFadeout;
    protected Collider[] targets;
    protected List<GameObject> hitTargets = new List<GameObject>();
    private Vector3 offset;

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position - transform.forward * castOffset, radius);
        }
    }
    public void Update()
    {
        if (begunFadeout)
            return;//stops everything progressing

        currentLife += Time.deltaTime;
        if (currentLife > lifeTime)
            FadeOut();

        if (allowCasting && gameObject.activeSelf)
        {
            transform.position = transform.position + transform.forward * Time.deltaTime * speed;
            OnCastBehavior();
        }      
    }
    protected virtual void FadeOut()
    {
        begunFadeout = true;
        allowCasting = false;
        for (int j = 0; j < disableOnImpact.Count; j++)
            disableOnImpact[j].SetActive(false);
        Invoke("ResetToPool", 1.2f);
    }
    protected virtual void ResetToPool()
    {
        transform.parent = MyParent;
        gameObject.SetActive(false);
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
        offset = transform.position - transform.forward * castOffset;
        targets = Physics.OverlapSphere(offset, radius, desiredLayers);
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].gameObject != MySlime.gameObject)
                {
                    if (ContainerCheck(targets[i].gameObject) && targets[i].gameObject.layer == slimeLayer)
                    {
                        hitTargets.Add(targets[i].gameObject);
                        Slime hitSlime = targets[i].GetComponent<Slime>();

                        if (module.behaviors.Contains(ProjectileBehaviorModule.Behaviors.Damage))
                            ApplyDamage(hitSlime);
                        if (module.behaviors.Contains(ProjectileBehaviorModule.Behaviors.Root))
                            ApplyRoot(hitSlime);
                        if (module.behaviors.Contains(ProjectileBehaviorModule.Behaviors.Stun))
                            ApplyStun(hitSlime);
                        if (module.behaviors.Contains(ProjectileBehaviorModule.Behaviors.Slow))
                            ApplySlow(hitSlime);
                        if (module.behaviors.Contains(ProjectileBehaviorModule.Behaviors.Knockback))
                            ApplyKnockback(hitSlime);
                    }

                    OnImpact();
                    FadeOut();
                }
            }
        }
    }
    public virtual void OnImpact()
    {
        impactObject.transform.parent = null;

        if(impactObject.gameObject.activeSelf)//fixed ps not play issue
            impactObject.gameObject.SetActive(false);

        impactObject.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        if (module.behaviors.Contains(ProjectileBehaviorModule.Behaviors.Heal))
            ApplyHeal();
    }
    protected virtual void OnDisable()
    {
        currentLife = 0;
        begunFadeout = false;
        allowCasting = true;
        for (int j = 0; j < disableOnImpact.Count; j++)
            disableOnImpact[j].SetActive(true);

        if (hitTargets.Count > 0)
            hitTargets.Clear();
    }

    #region Behavior Methods
    private void ApplyDamage(Slime _hitSlime)
    {
        float trueDamage = 0;
        //Power buff gives 20% damage buff
        if(MySlime.MyStatusControls.ApplyPower)
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
