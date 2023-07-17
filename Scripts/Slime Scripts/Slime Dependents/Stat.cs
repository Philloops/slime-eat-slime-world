using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    private int baseStatValue;
    public int BaseStatValue
    {
        get { return baseStatValue; }
        set
        {
            value = Mathf.Clamp(value, 1, 1000);
            baseStatValue = value;
        }
    }

    public int GeneratedBaseStat { get; set; }
    public int StatAffinity { get; set; }

    [SerializeField]
    private int currentStatValue;
    public int CurrentStatValue
    {
        get { return currentStatValue; }
        set
        {
            currentStatValue = value;
            Mathf.Clamp(currentStatValue, 1, Mathf.Infinity);
        }
    }

    private float affinityRNG = .25f;
    private int level;
    private int levelFlux;
    private int maxLevel;
    private GenerationMapping.Generation gen;

    public void InitializeStat(int _level, int _levelFlux, int _maxLevel, GenerationMapping.Generation _gen)
    {
        level = _level;
        levelFlux = _levelFlux;
        maxLevel = _maxLevel;
        gen = _gen;

        GenerateBaseStat();
    }

    public void GenerateBaseStat()
    {
        GeneratedBaseStat = Random.Range(level - levelFlux, (level + levelFlux) + 1);
        Mathf.Clamp(GeneratedBaseStat, 1, 1000);

        AffinityCheck();

        if (gen != GenerationMapping.Generation.one)
            ProcessGenData();

        BaseStatValue = GeneratedBaseStat;
    }
    public void AffinityCheck()
    {
        int loopIterations = 0;

        if (gen == GenerationMapping.Generation.one)
            loopIterations = 1;
        else if (gen == GenerationMapping.Generation.two)
            loopIterations = 2;
        else if (gen == GenerationMapping.Generation.three)
            loopIterations = 3;
        else if (gen == GenerationMapping.Generation.four)
            loopIterations = 4;
        else if (gen == GenerationMapping.Generation.five)
            loopIterations = 5;
        else if (gen == GenerationMapping.Generation.six)
            loopIterations = 6;

        for (int i = 0; i < loopIterations; i++)
        {
            if (Random.value <= affinityRNG)
                StatAffinity++;
        }
    }
    public void ProcessGenData()
    {
        int inheritedBaseStat = (GeneratedBaseStat / 2);
        GeneratedBaseStat += inheritedBaseStat;

        int affinityModifier = level * StatAffinity;
        GeneratedBaseStat += affinityModifier;

        Mathf.Clamp(GeneratedBaseStat, 1, 1000);
    }
}
