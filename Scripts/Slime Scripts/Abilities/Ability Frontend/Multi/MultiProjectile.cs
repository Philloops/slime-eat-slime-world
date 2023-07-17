using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiProjectile : PooledAbilityObject
{
    [Header("Lifetime Involved")]
    public float duration;
    private float durationValue;
    private bool isAlive = true;

    [Header("Projectile Involved")]
    public GameObject ProjectileParent;
    public List<BaseProjectile> projectiles;
    private bool launchedProjectiles;
    public float projectileDelay;
    private float delayValue;

    private List<Vector3> ProjectilePositions = new List<Vector3>();
    private List<Quaternion> ProjectileRotations = new List<Quaternion>();

    private void SetProjectileLocations()
    {
        if (ProjectilePositions.Count > 0)
        {
            for (int i = 0; i < ProjectilePositions.Count; i++)
            {
                projectiles[i].transform.localPosition = ProjectilePositions[i];
                projectiles[i].transform.localRotation = ProjectileRotations[i];
            }
        }
        else
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                ProjectilePositions.Add(projectiles[i].transform.localPosition);
                ProjectileRotations.Add(projectiles[i].transform.localRotation);
            }
        }
    }
    void OnEnable()
    {
        SetProjectileLocations();
    }
    public void Update()
    {
        if(isAlive)
        {
            ManageDuration();
            ManageProjectiles();
        }       
    }
    private void ManageProjectiles()
    {
        if (!launchedProjectiles)
        {
            delayValue += Time.deltaTime;
            if (delayValue >= projectileDelay)
            {
                launchedProjectiles = true;
                delayValue = 0;
                ProjectileParent.SetActive(true);
            }
        }
    }
    private void ManageDuration()
    {
        durationValue += Time.deltaTime;
        if (durationValue >= duration)
            Fadeout();
    }
    private void Fadeout()
    {
        isAlive = false;
        ResetToPool();
    }
    public override void SetSlime(Slime _caller)
    {
        MySlime = _caller;

        for (int i = 0; i < projectiles.Count; i++)
        {
            projectiles[i].SetSlime(_caller);
            projectiles[i].MyParent = ProjectileParent.transform;
        }
    }
    public void ResetToPool()
    {
        transform.parent = MyParent;
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        durationValue = 0;
        delayValue = 0;
        launchedProjectiles = false;
        ProjectileParent.SetActive(false);
        isAlive = true;

        SetProjectileLocations();
        for (int i = 0; i < projectiles.Count; i++)
            projectiles[i].gameObject.SetActive(true);
    }
}
