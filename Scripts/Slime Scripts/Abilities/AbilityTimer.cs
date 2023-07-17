using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityTimer
{
    public bool OnCooldown { get; set; }
    public float GlobalCD { get; set; }
    public float CooldownTimer { get; set; }

    public bool Timeout { get; set; }
    private float timeoutTracker;

    public bool ActivationCheck()
    {
        if (OnCooldown || Timeout)
            return false;

        OnCooldown = true;
        return true;
    }
    public void UpdateMethod()
    {//update ability meter, and mask enable states
        if (OnCooldown)
        {
            CooldownTimer += Time.deltaTime;

            if (CooldownTimer >= GlobalCD)
            {
                OnCooldown = false;
                CooldownTimer = 0;
            }
        }
        if(Timeout)
        {//adds a delay to basic attacks after skill use
            timeoutTracker += Time.deltaTime;
            
            if (timeoutTracker >= .25f)
            {
                Timeout = false;
                timeoutTracker = 0;
            }
        }
    }
}
