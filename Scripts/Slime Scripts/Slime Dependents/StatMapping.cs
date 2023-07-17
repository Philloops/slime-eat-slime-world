using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerationMapping
{
    public enum Generation { one, two, three, four, five, six }
    public Generation gen;

    [Header("level cap")]
    private int genOneCap = 40;
    private int genTwoCap = 60;
    private int genThreeCap = 80;
    private int genFourCap = 100;
    private int genFiveCap = 120;
    private int genSixCap = 140;

    public int ReturnGenCap()
    {
        if (gen == Generation.one)
            return genOneCap;
        else if (gen == Generation.two)
            return genTwoCap;
        else if (gen == Generation.three)
            return genThreeCap;
        else if (gen == Generation.four)
            return genFourCap;
        else if (gen == Generation.five)
            return genFiveCap;
        else
            return genSixCap;
    }
}

[System.Serializable]
public class StatMapping
{ 
    public Stat strength;
    public Stat agility;
    public Stat intellect;
    public Stat endurance;
    public Stat spirit;

    [Tooltip("Controls setting our generation and associated level cap")]
    public GenerationMapping genMap;

    private int currentLevel;
    private int currentMaxLevel;
    private int levelFlux;

    public void GenerateStats(int _level, int _levelFlux)//main function call for generation
    {
        currentLevel = _level;
        levelFlux = _levelFlux;
        currentMaxLevel = GenerationDefinedLevelCap();

        strength.InitializeStat(currentLevel, levelFlux, currentMaxLevel, genMap.gen);
        agility.InitializeStat(currentLevel, levelFlux, currentMaxLevel, genMap.gen);
        intellect.InitializeStat(currentLevel, levelFlux, currentMaxLevel, genMap.gen);
        endurance.InitializeStat(currentLevel, levelFlux, currentMaxLevel, genMap.gen);
        spirit.InitializeStat(currentLevel, levelFlux, currentMaxLevel, genMap.gen);

        SetStats();
    }
    public void ReadThruStats(SlimeData _info)
    {
        currentLevel = _info.levelMapping.level;
        currentMaxLevel = GenerationDefinedLevelCap();

        strength.CurrentStatValue = _info.statMapping.strength.CurrentStatValue;
        agility.CurrentStatValue = _info.statMapping.agility.CurrentStatValue;
        intellect.CurrentStatValue = _info.statMapping.intellect.CurrentStatValue;
        endurance.CurrentStatValue = _info.statMapping.endurance.CurrentStatValue;
        spirit.CurrentStatValue = _info.statMapping.spirit.CurrentStatValue;
    }
    public void SetStats(int _level)
    {
        currentLevel = _level;
        SetStats();
    }
    public void SetStats()
    {//formula -> stat = (baseStat + level) + (affiinity * lvl)
        strength.CurrentStatValue = strength.BaseStatValue + (strength.StatAffinity * currentLevel);
        agility.CurrentStatValue = agility.BaseStatValue + (agility.StatAffinity * currentLevel);
        intellect.CurrentStatValue = intellect.BaseStatValue + (intellect.StatAffinity * currentLevel);
        endurance.CurrentStatValue = endurance.BaseStatValue + (endurance.StatAffinity * currentLevel);
        spirit.CurrentStatValue = spirit.BaseStatValue + (spirit.StatAffinity * currentLevel);
    }
    public int GenerationDefinedLevelCap()
    {
        return genMap.ReturnGenCap();       
    } 
}
