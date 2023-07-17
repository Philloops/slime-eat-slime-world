using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAbilityController : BaseAbilityController
{
    public AILocomotion Locomotion { get; set; }
    public bool AbilityToggled { get; set; }

    private int currentIndex;
    public int CurrentIndex
    {
        get { return currentIndex; }
        set
        {
            if (value < 0)
                AbilityToggled = false;
            else
                currentIndex = value;
        }
    }
    public override bool RestrictCasting
    {
        get { return restrictCasting; }
        set
        {
            restrictCasting = value;
            MySlime.MyCombatCanvas.SilenceAll(value);
        }
    }
    public bool IsAliveBehavior
    {
        get
        {//Read only
            if(!MySlime.IsAlive)
            {
                Locomotion.DisableBehavior = true;
                CancelToggledAbility();
                MySlime.MyCombatCanvas.ClearDefeatedSlimeData();
                return false;
            }
            return true;
        }
    }
    private void Awake()
    {
        animator = GetComponent<SlimeAnimator>();
        MySlime = GetComponent<Slime>();
        canvas = MySlime.MyCombatCanvas;//maybe need?
        Locomotion = GetComponent<AILocomotion>();
    }
    private void AbilityUpdater()
    {
        if (!Locomotion.EnemyActive)
        {
            Locomotion.controlledState = AILocomotion.ControlledState.Waiting;
        }
        else if(Locomotion.EnemyActive)
        {
            if (MySlime.CurrentHealth <= MySlime.MaxHealth / 2)
                Locomotion.controlledState = AILocomotion.ControlledState.Defensive;
            else//need to add stragegic as well.....
                Locomotion.controlledState = AILocomotion.ControlledState.Aggressive;
        }        

        MySlime.BasicAttackTimer.UpdateMethod();
        for (int i = 0; i < MySlime.AbilityTimers.Count; i++)
            MySlime.AbilityTimers[i].UpdateMethod();
    }
    private void CheckRadialTimer(float _current, float _desired, float _max, int _index)
    {
        if (_current != _desired)
        {
            _current = _desired;
            canvas.SetAbilityFillMeter(_current, _max, _index);
        }

        if (MySlime.AbilityTimers[_index].OnCooldown || MySlime.AbilityTimers[_index].Timeout)
            canvas.SetAbilityMask(_index, true);
        else
            canvas.SetAbilityMask(_index, false);
    }
    private void AbilityCDVisuals()
    {
        CheckRadialTimer(canvas.AbilityTimer01, MySlime.AbilityTimers[0].CooldownTimer, MySlime.AbilityTimers[0].GlobalCD, 0);
        CheckRadialTimer(canvas.AbilityTimer02, MySlime.AbilityTimers[1].CooldownTimer, MySlime.AbilityTimers[1].GlobalCD, 1);
        CheckRadialTimer(canvas.AbilityTimer03, MySlime.AbilityTimers[2].CooldownTimer, MySlime.AbilityTimers[2].GlobalCD, 2);
    }
    void Update()
    {
        if (!IsAliveBehavior)
            return;

        AbilityUpdater();
        AbilityCDVisuals();
        AbilityInputsCheck();
    }
    private void CheckAbilityInput(bool _decision, int _index)
    {
        if (_decision && !MySlime.AbilityTimers[_index].OnCooldown)
        {
            if (MySlime.CurrentEnergy < MySlime.data.abilities[_index].abilityCost)
                return;

            CurrentIndex = _index;
            AbilityToggled = true;
        }
    }
    private Transform AssignCastPoint(BaseAbility _ability)
    {
        if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Lane)
            return laneSpawn;
        else if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Cone)
            return coneSpawn;
        else if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Free)
        {//spawn effect on player
            freeCircleSpawn.position = new Vector3(Locomotion.TargetSlime.transform.position.x, 
                freeCircleSpawn.position.y, Locomotion.TargetSlime.transform.position.z);
            return freeCircleSpawn;
        }
        else if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Bound ||
            _ability.abilityModuleData.projection == AbilityModulesData.Projection.Instant)
            return boundCircleSpawn;

        return transform;
    }
    private bool AbilityInputDecision(int _index)
    {
        if(MySlime.data.abilities[_index].abilityModuleData.projection == AbilityModulesData.Projection.Free)
        {
            if (Locomotion.Distance <= MySlime.data.abilities[_index].abilityModuleData.moduleData[0].modifier)
                return true;

            return false;
        }
        else if(MySlime.data.abilities[_index].abilityModuleData.projection == AbilityModulesData.Projection.Bound)
        {//more data 
            bool healthFull = (MySlime.CurrentHealth >= MySlime.MaxHealth);

            if (!healthFull && MySlime.data.abilities[_index].abilityFunctions.Contains(BaseAbility.AbilityFunction.Heal))
                return true;
            if (Locomotion.Distance <= MySlime.data.abilities[_index].abilityModuleData.moduleData[0].modifier)
                return true;

            return false;
        }
        else if(MySlime.data.abilities[_index].abilityModuleData.projection == AbilityModulesData.Projection.Lane)
        {
            if(Locomotion.Distance <= MySlime.data.abilities[_index].abilityModuleData.moduleData[0].modifier)
            {
                Locomotion.LeadSpeed = MySlime.data.abilities[_index].abilityModuleData.moduleData[1].modifier;
                return true;
            }
            return false; 
        }
        else if (MySlime.data.abilities[_index].abilityModuleData.projection == AbilityModulesData.Projection.Cone)
        {
            if (Locomotion.Distance <= MySlime.data.abilities[_index].abilityModuleData.moduleData[0].modifier)
                return true;

            return false;
        }
        else if(MySlime.data.abilities[_index].abilityModuleData.projection == AbilityModulesData.Projection.Instant)
        {
            bool healthFull = (MySlime.CurrentHealth >= MySlime.MaxHealth);
            if (!healthFull && MySlime.data.abilities[_index].abilityFunctions.Contains(BaseAbility.AbilityFunction.Heal))
                return true;
            //check if cast heals, do it!
            //check if provides protection, do it!
            //etc....etc...
        }

        return false;
        //slime needs to judge it's actions
        //if in range do damage skill
        //if low on health, do healing skill
        //if inside combat range(or health drops), pop buff skill
    }
    private void AbilityInputsCheck()
    {
        if (RestrictCasting || !Locomotion.EnemyActive)
            return;

        if (!AbilityToggled)
        {
            CheckAbilityInput(AbilityInputDecision(0), 0);
            CheckAbilityInput(AbilityInputDecision(1), 1);
            CheckAbilityInput(AbilityInputDecision(2), 2);
        }

        if (AbilityToggled && MySlime.AbilityTimers[currentIndex].ActivationCheck())
        {//cast ability
            //MySlime.MyStatusControls.SetSlowDuration(1, .25f);
            MySlime.data.abilities[currentIndex].AbilityActivated(AssignCastPoint(MySlime.data.abilities[currentIndex]), MySlime);
            MySlime.DrainEnergy(MySlime.data.abilities[currentIndex].abilityCost);
            CurrentIndex = -1;
            MySlime.BasicAttackTimer.Timeout = true;
        }
        else
        {//basic attack
            if (MySlime.BasicAttackTimer.ActivationCheck() && Locomotion.Distance <= MySlime.basicAttack.abilityModuleData.moduleData[0].modifier)
            {
                MySlime.MyStatusControls.SetSlowDuration(MySlime.basicAttack.globalCD, .25f);
                Locomotion.LeadSpeed = MySlime.basicAttack.abilityModuleData.moduleData[1].modifier;
                MySlime.basicAttack.AbilityActivated(laneSpawn, MySlime);
                animator.PlayAnimEvent("Ranged Attack Basic");
            }
        }

        if (!AbilityToggled)
            return;

        CancelToggledAbility();
    }
    public override void CancelToggledAbility()
    {
        if (AbilityToggled)
        {//cancel ability | Read->(r-click/l-trigger hit)
            CurrentIndex = -1;
            //CurrentForcast.EnableVisual(false);
        }
    }
}
