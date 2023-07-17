using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ability", menuName = "Abilities/General/BasicAttack")]
public class BasicAttack : BaseAbility
{
    public override void AbilityActivated(Transform _castPoint, Slime _slime)
    {
        Debug.Log("Basic Attack");
        AbilityManager.Instance.RequestBasicAttack(_castPoint, _slime);
    }
    public override void AbilityActivated(Slime _slime) { Debug.Log("Basic Attack"); }
    public override void AbilityActivated() { Debug.Log("Basic Attack"); }
}
