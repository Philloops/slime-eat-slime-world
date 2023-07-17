using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ability", menuName = "Abilities/WaterType/Tideless Waves")]
public class TidelessWaves : BaseAbility
{
    public override void AbilityActivated(Transform _castPoint, Slime _slime)
    {
        Debug.Log("Tideless Waves");
        AbilityManager.Instance.RequestAbilityAttack(_castPoint, _slime, indexID);
    }
    public override void AbilityActivated(Slime _slime) { Debug.Log("Tideless Waves"); }
    public override void AbilityActivated() { Debug.Log("Tideless Waves"); }
}
