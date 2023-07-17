using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : BaseSlime
{
    #region Notes
    //further define variables and properies
    //further add to methods for procedural stat and affinity on slimes

    /* Defining base stats on generation
     --------------------------------------
      baseStat = 
        if(gen 1)
            {generated stat based off level}
        else if(!gen 1)
            {(inherited stat / 2)}

        stat = baseStat + (affinity * lvl)
     --------------------------------------
    gen 1 level 1
    Strength -> Random.Range(1, lvl +/- value)
    Agility -> Random.Range(1, lvl +/- value)
    Intellect -> Random.Range(1, lvl +/- value)
    Endurance -> Random.Range(1, lvl +/- value)
    Spirit -> Random.Range(1, lvl +/- value)

    gen 1 level 3 
    Strength -> Random.Range(1, lvl +/- value)
    Agility -> Random.Range(1, lvl +/- value)
    Intellect -> Random.Range(1, lvl +/- value)
    Endurance -> Random.Range(1, lvl +/- value)
    Spirit -> Random.Range(1, lvl +/- value)

    gen 1 level 9
    Strength -> Random.Range(1, lvl +/- value)
    Agility -> Random.Range(1, lvl +/- value)
    Intellect -> Random.Range(1, lvl +/- value)
    Endurance -> Random.Range(1, lvl +/- value)
    Spirit -> Random.Range(1, lvl +/- value)
    */

    //Formula -> Affinity aka IV ------ stat = generated stat + (affinity * lvl) + (inherited stat / 2) 
    //Gen 1 level 5 slime ->
    /* 
     Strength(A^1) -> 5 + (1 * 5) + 0;
     Agility -> 5
     Intellect -> 5
     Endurance -> 5
     Spirit -> 5
    */
    //Gen 2 level 10 slime
    /* slime A + slime B | Highest stats for each stat is what is kept, then divided by 2.
     Strength(A^1) -> 10 + (1 * 10) + (37/2);
     Agility(A^2) -> 10 + (2 * 10) + (40/2);
     Intellect -> 10 + (30/2);
     Endurance(A^1) -> 10 + (1 * 10) + (37/2);
     Spirit -> (A^2) -> 10 + (2 * 10) + (40/2);
    */
    #endregion

    #region properties & variables
    [Header("Slime")]
    public GameObject defeatVFX;
    [SerializeField]
    private bool isAlive;
    public bool IsAlive { get { return isAlive; } }

    private StatusController myStatusControls;
    public StatusController MyStatusControls
    {//Read only
        get
        {
            if (myStatusControls == null)
                myStatusControls = GetComponent<StatusController>();

            return myStatusControls;
        }
    }

    public enum SlimeControlType { Player, AI }
    public SlimeControlType slimeControlType;

    [Header("Slime Type")]   
    public List<Sprite> slimePortrait = new List<Sprite>();
    public Sprite MyPortrait { get; set; }

    public AbilityTimer BasicAttackTimer { get; set; }
    public List<AbilityTimer> AbilityTimers { get; set; }


    public Color FullColor, EmptyColor;
    public Image healthRadialObj;
    private CombatCanvas myCombatCanvas;
    public CombatCanvas MyCombatCanvas
    {
        get
        {
            if(myCombatCanvas == null)
            {
                if(slimeControlType == SlimeControlType.Player)
                    return myCombatCanvas = AbilityManager.Instance.playerCombatCanvas;
                else if(slimeControlType == SlimeControlType.AI)
                    return myCombatCanvas = AbilityManager.Instance.enemyCombatCanvas;
            }
            return myCombatCanvas;
        }
    }

    public override float CurrentHealth
    {//Might need to be float for smoother Fill Meter
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
            UpdateHealthRadial(currentHealth, MaxHealth);
            MyCombatCanvas.SetHealthFillMeter(currentHealth, MaxHealth);
        }
    }

    [Header("Energy Mapping")]
    public float energyRegenSpeed;

    public override float CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            currentEnergy = value;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
            MyCombatCanvas.SetEnergyFillMeter(currentEnergy, maxEnergy);
        }
    }
    #endregion

    #region Initalize Slime methods
    void OnUpdateCombatUI()
    { 
        if (slimeControlType == SlimeControlType.AI)
        {
            for (int i = 0; i < data.abilities.Count; i++)
                MyCombatCanvas.SetAbilityIcon(i, data.abilities[i].abilityIcon);

            MyCombatCanvas.SetSlime(this);//portrait set
        }
    }
    private void InitializeAbilities()
    {       
        AbilityTimers = new List<AbilityTimer>();
        for (int i = 0; i < data.abilities.Count; i++)
        {
            AbilityTimers.Add(new AbilityTimer());

            if(slimeControlType == SlimeControlType.Player)
                MyCombatCanvas.SetAbilityIcon(i, data.abilities[i].abilityIcon);
        }

        if (data.archetype == SlimeData.Archetype.Fire)
            MyPortrait = slimePortrait[0];
        else if (data.archetype == SlimeData.Archetype.Water)
            MyPortrait = slimePortrait[1];
        else if (data.archetype == SlimeData.Archetype.Nature)
            MyPortrait = slimePortrait[2];

        BasicAttackTimer = new AbilityTimer();
        BasicAttackTimer.GlobalCD = basicAttack.globalCD;

        for (int i = 0; i < AbilityTimers.Count; i++)
            AbilityTimers[i].GlobalCD = data.abilities[i].globalCD;

        if(slimeControlType == SlimeControlType.Player)
            MyCombatCanvas.SetSlime(this);//portrait set
    }
    public void OnCombatEnd()
    {
        UpdateSlimeCheck();
        //restore health?
        //restore energy?
        //etc....
    }
    public override void OnSpawnToWorld()
    {
        isAlive = true;
        base.OnSpawnToWorld();
        InitializeAbilities();
    }
    public void UpdateSlimeCheck()
    {
        if(data.TrackedLevel != data.levelMapping.level)
        {
            data.TrackedLevel = data.levelMapping.level;
            data.statMapping.strength.BaseStatValue++;
            data.statMapping.agility.BaseStatValue++;
            data.statMapping.intellect.BaseStatValue++;
            data.statMapping.endurance.BaseStatValue++;
            data.statMapping.spirit.BaseStatValue++;
            data.statMapping.SetStats(data.levelMapping.level);
        }
    }
    #endregion

    #region Health & Energy methods
    public void UpdateHealthRadial(float _value, float _max)
    {
        if(healthRadialObj != null && healthRadialObj.gameObject.activeSelf)
        {
            float t = _value / _max;
            healthRadialObj.fillAmount = t;
            healthRadialObj.color = Color.Lerp(EmptyColor, FullColor, t);
        }
    }
    public void DrainEnergy(int _drainAmt)
    {
        CurrentEnergy -= _drainAmt;
        Mathf.Clamp(CurrentEnergy, 0, maxEnergy);
    }
    public void TakeDamage(float _damage)
    {
        if (!isAlive)
            return;

        //if protections applied cut damage by fixed percentage that i decide(20%, 40%, etc...)
        float trueDamage = 0;
        if (MyStatusControls.ApplyProtection)
            trueDamage = _damage * .8f;//20% shaved off damage due to protections
        else
            trueDamage = _damage;

        if (CurrentHealth > 0)
        {
            Debug.Log("Inured + " + this.name);
            CurrentHealth -= trueDamage;

            if (slimeControlType == SlimeControlType.AI)
                OnUpdateCombatUI();
        }

        if(CurrentHealth <= 0)
            Die();
    }
    public void HealDamage(int _heal)
    {
        CurrentHealth += _heal;
        Mathf.Clamp(CurrentHealth, 0, MaxHealth);
    }
    public void Die()
    {
        isAlive = false;

        MyCombatCanvas.RemoveSlimeUI(this);
        for (int i = 0; i < data.abilities.Count; i++)
        {
            MyCombatCanvas.SetAbilityIcon(i, MyCombatCanvas.abilityUIMask);
            MyCombatCanvas.SetAbilityMask(i, false);
        }
        MyCombatCanvas.SetHealthFillMeter(1, 1);
        MyCombatCanvas.SetEnergyFillMeter(1, 1);

        ResetPooledAbilityObjs();

        defeatVFX.SetActive(true);
        MyStatusControls.SlimeLocomotion.visualOrientation.gameObject.SetActive(false);

        Invoke("DelayedResponse", 1.5f);
        //Played Knocked out vfx/sfx
        //Change Anim to wobble
        //Let whichever class know it's in a capture ready state
    }
    public void ResetPooledAbilityObjs()
    {
        foreach (PooledAbilityObject obj in gameObject.GetComponentsInChildren<PooledAbilityObject>())
        {
            obj.transform.parent = obj.MyParent;
            obj.gameObject.SetActive(false);
        }
    }
    private void DelayedResponse()
    {       
        if (!ArenaManager.instance)
            gameObject.SetActive(false);
        else if (ArenaManager.instance)
            RemoveSlimeData();
    }
    private void RemoveSlimeData()
    {       
        if (slimeControlType == SlimeControlType.AI)
        {
            gameObject.SetActive(false);

            ArenaManager.instance.automatedSlimes.Remove(ArenaManager.instance.automatedSlimes[0]);
            AbilityManager.Instance.slimeManager.RemoveAutomatedSlime();

            if (ArenaManager.instance.automatedSlimes.Count == 0)
            {
                Debug.LogError("Leave arena scene, -> AI loss");
                //Reset All pooled objects before deloading of scene
                AbilityManager.Instance.slimeManager.InitializedCombat = false;
            }

            if (ArenaManager.instance.automatedSlimes.Count > 0)
                ArenaManager.instance.SpawnAutomatedSlime(ArenaManager.instance.automatedSlimes[0]);
        }
        else if(slimeControlType == SlimeControlType.Player)
        {
            ArenaManager.instance.controlledSlimes.Remove(ArenaManager.instance.controlledSlimes[0]);

            if(ArenaManager.instance.controlledSlimes.Count == 0)
            {
                Debug.LogError("Leave arena scene, -> player loss");
                //Reset All pooled objects before deloading of scene
                AbilityManager.Instance.slimeManager.InitializedCombat = false;
            }

            if (ArenaManager.instance.controlledSlimes.Count > 0)
            {
                gameObject.SetActive(false);
                ArenaManager.instance.SpawnControlledSlime(ArenaManager.instance.controlledSlimes[0]);
            }
        }
    }
    #endregion
}
