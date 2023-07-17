using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlimeData
{
    public List<BaseAbility> abilities = new List<BaseAbility>();

    public Archetype archetype;
    public enum Archetype { Undefined, Fire, Water, Nature }

    public LevelMapping levelMapping;
    public int TrackedLevel { get; set; }

    public StatMapping statMapping;
}
