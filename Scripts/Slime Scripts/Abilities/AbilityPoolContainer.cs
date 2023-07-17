using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityPoolContainer
{
    public GameObject prefabObj;
    public List<PooledAbilityObject> objectPool = new List<PooledAbilityObject>();

    public bool ContainCheck(PooledAbilityObject _object)
    {
        if (!objectPool.Contains(_object))
            return false;
        else
            return true;
    }
}
