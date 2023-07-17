using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ability", menuName = "Abilities/WaterType/Ocean Torrent")]
public class OceanTorrent : BaseAbility
{
    public override void AbilityActivated(Transform _castPoint, Slime _slime)
    {
        Debug.Log("Ocean Torrent");
        AbilityManager.Instance.RequestAbilityAttack(_castPoint, _slime, indexID);
    }
    public override void AbilityActivated(Slime _slime) { Debug.Log("Ocean Torrent"); }
    public override void AbilityActivated() { Debug.Log("Ocean Torrent"); }
}
