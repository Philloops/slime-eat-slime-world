using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModuleData
{
    public string name;
    public float modifier;
}

[System.Serializable]
public class AbilityModulesData
{
    [Header("Placement Data")]
    public List<ModuleData> moduleData;

    [Header("Projection Data")]
    public Projection projection;
    public enum Projection { Instant, Bound, Free, Lane, Cone }
    public Vector3 projectionScale;
}
