using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ability", menuName = "Abilities/NatureType/Enveloping Blossoms")]
public class EnvelopingBlossoms : BaseAbility
{
    public override void AbilityActivated(Transform _castPoint, Slime _slime)
    {
        Debug.Log("Enveloping Blossoms");
        AbilityManager.Instance.RequestAbilityAttack(_castPoint, _slime, indexID);
    }
    public override void AbilityActivated(Slime _slime) { Debug.Log("Enveloping Blossoms"); }
    public override void AbilityActivated() { Debug.Log("Enveloping Blossoms"); }
}
