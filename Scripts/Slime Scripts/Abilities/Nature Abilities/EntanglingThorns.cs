using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ability", menuName = "Abilities/NatureType/Entangling Thorns")]
public class EntanglingThorns : BaseAbility
{
    public override void AbilityActivated(Transform _castPoint, Slime _slime)
    {
        Debug.Log("Entangling Thorns");
        AbilityManager.Instance.RequestAbilityAttack(_castPoint, _slime, indexID);
    }
    public override void AbilityActivated(Slime _slime) { Debug.Log("Entangling Thorns"); }
    public override void AbilityActivated() { Debug.Log("Entangling Thorns"); }
}
