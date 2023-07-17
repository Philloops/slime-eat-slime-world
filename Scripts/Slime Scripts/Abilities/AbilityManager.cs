using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityManager : MonoBehaviour
{
    #region Initializes AbilityManager on call
    private static AbilityManager instance;
    public static AbilityManager Instance
    {
        get
        {
            if (instance)
                return instance;
            else
                instance = CreateAbilityManager();

            return instance;
        }
    } 
    private static AbilityManager CreateAbilityManager()
    {
        //GameObject newAM = new GameObject("AbilityTimer Manager");
        //AbilityManager newAbilityManager = newAM.AddComponent<AbilityManager>();
        GameObject newAM = GameObject.Instantiate(Resources.Load("Prefabs/Abilities/Ability Manager") as GameObject);
        AbilityManager newAbilityManager = newAM.GetComponent<AbilityManager>();

        return newAbilityManager;
    }
    #endregion

    #region Path Management
    public SlimeManager slimeManager;
    public GameObject combatCanvasRoot;
    private bool setCanvas;
    public bool SetCanvas
    {
        get { return setCanvas; }
        set
        {
            setCanvas = value;
            if (setCanvas)
                combatCanvasRoot.SetActive(true);
            else if (!setCanvas)
                combatCanvasRoot.SetActive(false);
        }
    }
    public CombatCanvas playerCombatCanvas;
    public CombatCanvas enemyCombatCanvas;

    [Header("Ability Handlers")]
    public PathDelegation pathOfFire;
    public PathDelegation pathOfWater;
    public PathDelegation pathOfNature;

    public void RegisterItemToPool(PooledAbilityObject _object)
    {
        if (_object.path == PooledAbilityObject.Path.Fire)
            pathOfFire.CheckAbilityPoolRegistration(pathOfFire.AbilityObjects[_object.id], _object);
        else if(_object.path == PooledAbilityObject.Path.Water)
            pathOfWater.CheckAbilityPoolRegistration(pathOfWater.AbilityObjects[_object.id], _object);
        else if (_object.path == PooledAbilityObject.Path.Nature)
            pathOfNature.CheckAbilityPoolRegistration(pathOfNature.AbilityObjects[_object.id], _object);
    }
    public void RequestBasicAttack(Transform _castPoint, Slime _slime)
    {
        if (_slime.data.archetype == SlimeData.Archetype.Fire)
            pathOfFire.RequestAbilityProjectile(_castPoint, _slime, pathOfFire.AbilityObjects[0]);
        else if (_slime.data.archetype == SlimeData.Archetype.Water)
            pathOfWater.RequestAbilityProjectile(_castPoint, _slime, pathOfWater.AbilityObjects[0]);
        else if (_slime.data.archetype == SlimeData.Archetype.Nature)
            pathOfNature.RequestAbilityProjectile(_castPoint, _slime, pathOfNature.AbilityObjects[0]);
    }
    public void RequestAbilityAttack(Transform _castPoint, Slime _slime, int _id)
    {
        if (_slime.data.archetype == SlimeData.Archetype.Fire)
            pathOfFire.RequestAbilityProjectile(_castPoint, _slime, pathOfFire.AbilityObjects[_id]);
        else if (_slime.data.archetype == SlimeData.Archetype.Water)
            pathOfWater.RequestAbilityProjectile(_castPoint, _slime, pathOfWater.AbilityObjects[_id]);
        else if (_slime.data.archetype == SlimeData.Archetype.Nature)
            pathOfNature.RequestAbilityProjectile(_castPoint, _slime, pathOfNature.AbilityObjects[_id]);
    }
    public BaseAbility AbilityMapRequest(SlimeData _slime)
    {
        if (_slime.archetype == SlimeData.Archetype.Fire)
            return pathOfFire.AbilityMap(_slime);
        else if (_slime.archetype == SlimeData.Archetype.Water)
            return pathOfWater.AbilityMap(_slime);
        else if (_slime.archetype == SlimeData.Archetype.Nature)
            return pathOfNature.AbilityMap(_slime);

        Debug.LogError("Ability Map Request Couldn't be met");
        return null;
    }
    #endregion


}
