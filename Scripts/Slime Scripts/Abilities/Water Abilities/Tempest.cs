using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ability", menuName = "Abilities/WaterType/Tempest")]
public class Tempest : BaseAbility
{
    public override void AbilityActivated(Transform _castPoint, Slime _slime)
    {
        Debug.Log("Tempest");
        AbilityManager.Instance.RequestAbilityAttack(_castPoint, _slime, indexID);
    }
    public override void AbilityActivated(Slime _slime) { Debug.Log("Tempest"); }
    public override void AbilityActivated() { Debug.Log("Tempest"); }
}
