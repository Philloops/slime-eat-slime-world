using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantCastBehaviorModule
{
    public enum Behaviors { Power, Protection, burstHeal, overTimeHeal, MoveSpeed }
    public List<Behaviors> behaviors = new List<Behaviors>();

    public float powerDuration;
    public float protectionDuration;
    public int burstHealAmt;
    public int overTimeHealAmt;
    public float overTimeHealDuration;
    public float moveSpeedDuration;
    public float moveSpeedAmt;
}
public class BaseInstantCast : PooledAbilityObject
{
    public InstantCastBehaviorModule module;
    public float startDelay;
    public float duration;
    public float fadeOutDuration;
    public List<ParticleSystem> ps;

    private ParticleSystem.MainModule mainModule;
    private bool fadeOut;
    private float lifeTime;
    private float fadeOutTime;


    void OnDisable()
    {
        lifeTime = 0;
        fadeOutTime = 0;
        for (int i = 0; i < ps.Count; i++)
        {
            mainModule = ps[i].main;
            mainModule.loop = true;
        }
    }
    void OnEnable()
    {
        if (startDelay > 0)
            Invoke("BeginStartDelay", startDelay);
        else
            BeginStartDelay();
    }
    void BeginStartDelay()
    {
        OnCastBehaviorInstant();
    }  
    public void Update()
    {
        VFXHandling();
    }
    private void VFXHandling()
    {
        if (!fadeOut)
        {
            lifeTime += Time.deltaTime;
            if (lifeTime >= duration)
            {
                fadeOut = true;
                for (int i = 0; i < ps.Count; i++)
                {
                    mainModule = ps[i].main;
                    mainModule.loop = false;
                }
            }
        }
        else
        {
            fadeOutTime += Time.deltaTime;
            if (fadeOutTime >= fadeOutDuration)
            {
                transform.parent = MyParent;
                gameObject.SetActive(false);
                fadeOut = false;
            }
        }
    }   
    public virtual void OnCastBehaviorInstant()
    {
        if (MySlime == null)
            Debug.LogError("Slime ref null");

        if (module.behaviors.Contains(InstantCastBehaviorModule.Behaviors.Power))
            MySlime.MyStatusControls.SetPowerDuration(module.powerDuration);
        if (module.behaviors.Contains(InstantCastBehaviorModule.Behaviors.Protection))
            MySlime.MyStatusControls.SetProtectionDuration(module.protectionDuration);
        if (module.behaviors.Contains(InstantCastBehaviorModule.Behaviors.burstHeal))
            MySlime.HealDamage(module.burstHealAmt);
        if (module.behaviors.Contains(InstantCastBehaviorModule.Behaviors.overTimeHeal))
            MySlime.MyStatusControls.SetHotDuration(module.overTimeHealDuration, module.overTimeHealAmt);
        if (module.behaviors.Contains(InstantCastBehaviorModule.Behaviors.MoveSpeed))
            Debug.Log("Would give a movement speed buff");
    }
}
