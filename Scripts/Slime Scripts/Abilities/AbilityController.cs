using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityController : BaseAbilityController
{
    public PlayerLocomotion Locomotion { get; set; }

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

    public List<AbilityForecast> abilityForecasts;//0 cone, 1 lane, 2 circle
    public AbilityForecast CurrentForcast { get; set; }
    public bool AbilityToggled { get; set; }

    private SlimeInputMap slimeInputMap;
    private FreeMoveAbility freeMoveAbility;

    private void Awake()
    {
        animator = GetComponent<SlimeAnimator>();
        slimeInputMap = GetComponent<SlimeInputMap>();
        MySlime = GetComponent<Slime>();
        Locomotion = GetComponent<PlayerLocomotion>();
        canvas = MySlime.MyCombatCanvas;
        freeMoveAbility = abilityForecasts[2].GetComponent<FreeMoveAbility>();

        for (int i = 0; i < abilityForecasts.Count; i++)
            abilityForecasts[i].Initialize();
    }
    private void AbilityUpdater()
    {
        MySlime.BasicAttackTimer.UpdateMethod();
        for (int i = 0; i < MySlime.AbilityTimers.Count; i++)
            MySlime.AbilityTimers[i].UpdateMethod();
    }
    private void CheckRadialTimer(float _current, float _desired, float _max, int _index)
    {
        if(_current != _desired)
        {
            _current = _desired;
            canvas.SetAbilityFillMeter(_current, _max, _index);
        }

        if(MySlime.AbilityTimers[_index].OnCooldown || MySlime.AbilityTimers[_index].Timeout)
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
    private bool GetForecast(BaseAbility _ability)
    {
        if(_ability.abilityModuleData.projection == AbilityModulesData.Projection.Instant)
        {
            CurrentForcast = null;
            return false;
        }

        if(_ability.abilityModuleData.projection == AbilityModulesData.Projection.Cone)
            CurrentForcast = abilityForecasts[0];
        else if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Lane)
            CurrentForcast = abilityForecasts[1];
        else if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Free)
        {
            CurrentForcast = abilityForecasts[2];
            freeMoveAbility.Reach = _ability.abilityModuleData.moduleData[0].modifier;
        }
        else if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Bound)
            CurrentForcast = abilityForecasts[3];

        AbilityToggled = true;
        return true;
    }
    private void CheckAbilityInput(bool _input, int _index)
    {        
        if(_input && !MySlime.AbilityTimers[_index].OnCooldown)
        {
            if (MySlime.CurrentEnergy < MySlime.data.abilities[_index].abilityCost)
                return;//checks to make sure we have enough energy for ability to be toggled.

            CurrentIndex = _index;
            
            if (GetForecast(MySlime.data.abilities[_index]))
            {
                //uses projection before cast
                CurrentForcast.EnableVisual(true, MySlime.data.abilities[_index].abilityModuleData.projectionScale);
            }
            else
            {                
                if(MySlime.AbilityTimers[_index].ActivationCheck())
                {//instant cast
                    MySlime.data.abilities[_index].AbilityActivated();//drain energy too!!!
                    MySlime.DrainEnergy(MySlime.data.abilities[_index].abilityCost);
                    CurrentIndex = -1;
                    MySlime.BasicAttackTimer.Timeout = true;
                }
            }
        }
    }
    private Transform AssignCastPoint(BaseAbility _ability)
    {
        if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Lane)
            return laneSpawn;
        else if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Cone)
            return coneSpawn;
        else if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Free)
            return freeCircleSpawn;
        else if (_ability.abilityModuleData.projection == AbilityModulesData.Projection.Bound ||
            _ability.abilityModuleData.projection == AbilityModulesData.Projection.Instant)
            return boundCircleSpawn;

        return transform;
    }
    
    private void AbilityInputsCheck()
    {
        if (RestrictCasting)
            return;

        if (!AbilityToggled)
        {
            CheckAbilityInput(slimeInputMap.Ability01Hit, 0);
            CheckAbilityInput(slimeInputMap.Ability02Hit, 1);
            CheckAbilityInput(slimeInputMap.Ability03Hit, 2);
        }     

        if (slimeInputMap.OnActionHit > 0.35f)
        {
            if(AbilityToggled && MySlime.AbilityTimers[currentIndex].ActivationCheck())
            {//cast ability | Read->(l-click/r-trigger hit)
                //MySlime.MyStatusControls.SetSlowDuration(1, .25f);
                MySlime.data.abilities[currentIndex].AbilityActivated(AssignCastPoint(MySlime.data.abilities[currentIndex]), MySlime);//drain energy too!!!
                MySlime.DrainEnergy(MySlime.data.abilities[currentIndex].abilityCost);
                CurrentIndex = -1;
                CurrentForcast.EnableVisual(false);
                MySlime.BasicAttackTimer.Timeout = true;
            }
            else
            {//basic attack
                if(MySlime.BasicAttackTimer.ActivationCheck())
                {
                    MySlime.MyStatusControls.SetSlowDuration(MySlime.basicAttack.globalCD, .25f);
                    MySlime.basicAttack.AbilityActivated(laneSpawn, MySlime);
                    animator.PlayAnimEvent("Ranged Attack Basic");
                }
            }
        }
        if (slimeInputMap.OnCancelHit > 0.35f)
        {
            if (!AbilityToggled)
                return;

            CancelToggledAbility();
        }
    }
    public override void CancelToggledAbility()
    {
        if (AbilityToggled)
        {//cancel ability | Read->(r-click/l-trigger hit)
            CurrentIndex = -1;
            CurrentForcast.EnableVisual(false);
        }
    }
}
