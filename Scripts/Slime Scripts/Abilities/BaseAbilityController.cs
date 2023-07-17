using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbilityController : MonoBehaviour
{
    public Transform laneSpawn;
    public Transform coneSpawn;
    public Transform boundCircleSpawn;
    public Transform freeCircleSpawn;

    public Slime MySlime { get; set; }
    protected bool restrictCasting;
    public virtual bool RestrictCasting
    {
        get { return restrictCasting; }
        set { restrictCasting = value; }
    }

    protected CombatCanvas canvas;
    protected SlimeAnimator animator;

    public virtual void CancelToggledAbility() {}
}
