using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDelegation : MonoBehaviour
{
    public Transform poolParent;
    public List<AbilityPoolContainer> AbilityObjects;
    public List<BaseAbility> abilitiesPool;

    #region ability methods
    public BaseAbility AbilityMap(SlimeData _slime)
    {
        int abilityIndex = 0;
        bool set = false;

        while (!set)
        {
            abilityIndex = Random.Range(0, abilitiesPool.Count);

            if (!_slime.abilities.Contains(abilitiesPool[abilityIndex]))
                set = true;
        }

        Debug.Log("Should've attached -> " + abilitiesPool[abilityIndex]);
        return abilitiesPool[abilityIndex];
    }
    #endregion

    #region basicAttack methods
    public void CheckAbilityPoolRegistration(AbilityPoolContainer _pool, PooledAbilityObject _object)
    {
        if (_pool.ContainCheck(_object) == false)
        {
            _pool.objectPool.Add(_object);
            _object.MyParent = poolParent;
        }
    }
    public void CheckAbilityObjSpawn(GameObject _object, Transform _castPoint, Slime _caller)
    {
        GameObject _newPoolObj = Instantiate(_object, _castPoint.position, _castPoint.rotation);
        PooledAbilityObject pooledAbilityObject = _newPoolObj.GetComponent<PooledAbilityObject>();

        pooledAbilityObject.SetSlime(_caller);
        if (pooledAbilityObject.anchor)
            pooledAbilityObject.transform.parent = _castPoint;

        _newPoolObj.SetActive(true);
    }
    public void RequestAbilityProjectile(Transform _castPoint, Slime _caller, AbilityPoolContainer _abilityPool)
    {
        bool requestComplete = false;

        if (_abilityPool.objectPool.Count > 0)
        {
            for (int i = 0; i < _abilityPool.objectPool.Count; i++)
            {
                if (!_abilityPool.objectPool[i].gameObject.activeSelf
                    && _abilityPool.objectPool[i].transform.parent != null)
                {
                    requestComplete = true;
                    _abilityPool.objectPool[i].SetSlime(_caller);
                    _abilityPool.objectPool[i].transform.parent = _castPoint;
                    _abilityPool.objectPool[i].transform.position = _castPoint.position;
                    _abilityPool.objectPool[i].transform.rotation = _castPoint.rotation;

                    if (!_abilityPool.objectPool[i].anchor)
                    {
                        _abilityPool.objectPool[i].transform.parent = AbilityManager.Instance.transform;
                        _abilityPool.objectPool[i].transform.parent = null;
                    }

                    _abilityPool.objectPool[i].gameObject.SetActive(true);
                    return;
                }
            }
            if (!requestComplete)
            {
                CheckAbilityObjSpawn(_abilityPool.prefabObj, _castPoint, _caller);
            }
        }
        else
        {
            CheckAbilityObjSpawn(_abilityPool.prefabObj, _castPoint, _caller);
        }
    }
    #endregion
}
