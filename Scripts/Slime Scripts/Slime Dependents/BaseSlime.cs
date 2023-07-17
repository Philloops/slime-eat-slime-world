using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSlime : MonoBehaviour
{
    [Header("BaseSlime")]
    public BaseAbility basicAttack;
    public SlimeData data;

    public int hpMultiplier;
    public int MaxHealth
    {//read only
        get
        {
            return data.statMapping.endurance.CurrentStatValue * hpMultiplier;
        }
    }
    protected float currentHealth;
    public virtual float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        }
    }

    public int maxEnergy;
    protected float currentEnergy = 100;
    public virtual float CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            currentEnergy = value;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        }
    }

    public virtual void OnSpawnToWorld()
    {
        //data.levelMapping.LevelToExperience();
        //data.statMapping.GenerateStats(data.levelMapping.level, data.levelMapping.levelFlux);
        //data.TrackedLevel = data.levelMapping.level;
        CurrentHealth = MaxHealth;

        //for (int i = 0; i < 3; i++)
        //    data.abilities.Add(AbilityManager.Instance.AbilityMapRequest(data));
    }
}
