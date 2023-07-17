using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : ScriptableObject
{
    [Header("ability data")]
    public int indexID;
    public string abilityName;
    public Sprite abilityIcon;
    public int abilityCost;
    public enum AbilityFunction { Direct, DOT, Force, Root, Stun, Slow, Silence, Heal, Protection, Power }
    public List<AbilityFunction> abilityFunctions;
    public enum AffinityBonus { None, Strength, Agility, Intellect, Endurance, Spirit }
    public AffinityBonus affinityBonus;
    public float globalCD;

    public AbilityModulesData abilityModuleData;

    public abstract void AbilityActivated();
    public abstract void AbilityActivated(Slime _slime);
    public abstract void AbilityActivated(Transform _castPoint, Slime _slime);
}
