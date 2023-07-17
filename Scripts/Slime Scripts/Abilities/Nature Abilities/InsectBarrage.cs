using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ability", menuName = "Abilities/NatureType/Insect Barrage")]
public class InsectBarrage : BaseAbility
{
    public float channelDuration;

    public override void AbilityActivated(Transform _castPoint, Slime _slime)
    {
        Debug.Log("Insect Barrage");
        AbilityManager.Instance.RequestAbilityAttack(_castPoint, _slime, indexID);
        _slime.MyStatusControls.SetChannelDuration(channelDuration);//-------
        _slime.MyStatusControls.SetSlowDuration(channelDuration, .25f);
    }
    public override void AbilityActivated(Slime _slime) { Debug.Log("Insect Barrage"); }
    public override void AbilityActivated() { Debug.Log("Insect Barrage"); }
}
