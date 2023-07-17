using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMethod : MonoBehaviour
{
    public BaseAbility ability;
    public Transform pos;

    void Update()
    {
        if (TempInputManager.Instance.B_Key)
            ability.AbilityActivated(pos, GetComponent<Slime>());
    }
}
