using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldSlimeSpawner : MonoBehaviour
{
    public float unitSphereRadius;
    //[Tooltip("Set this to the current Y of spawn area, can aleave calls due to inside unit sphere casting high or low")]
    //public float offsetY;
    public GameObject SlimePrefab;

    [Range(1, 140)]
    public int minLevelHere;
    [Range(1, 140)]
    public int maxLevelHere;

    [Tooltip("added to the random range call of lvl gen, for rare slimes")]
    [Range(1, 140)]
    public int rareLevelIncrease;

    [Range(0, 1)]
    public float rareSpawnChance;

    public List<GenerationMapping.Generation> commonGensInArea;
    public List<GenerationMapping.Generation> rareGensInArea;

    public List<SlimeData.Archetype> commonTypesInArea;
    public List<SlimeData.Archetype> rareTypesInArea;

    private int abilitySlotCount = 3;
    private NavMeshHit navHit;
    private Vector3 spawnPos;
    private Vector3 finalPos;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            SlimeSetup();
        }
    }
    public Vector3 RelativeRandomPosition()
    {//World slime method call
        Vector3 pos = Random.insideUnitSphere * unitSphereRadius;
        //pos += transform.position;
        pos.y = transform.position.y;//Helps us have fewer calls in long run
        return pos;
    }

    public void SpawnSlime(NavMeshAgent _agent)
    {
        int testBreak = 0;
        bool successful = false;
        while(!successful)
        {
            testBreak++;
            if (testBreak >= 25)
            {
                Debug.LogError("Break");
                successful = true;
            }

            spawnPos = RelativeRandomPosition() + transform.position;

            if (NavMesh.SamplePosition(spawnPos, out navHit, 20, 1))
            {
                successful = true;
                finalPos = navHit.position;
                _agent.transform.position = finalPos;
            }
        }      
    }
    private void SlimeSetup()
    {
        GameObject newSlime = Instantiate(SlimePrefab, Vector3.zero, Quaternion.identity);
        WorldSlime slime = newSlime.GetComponent<WorldSlime>();

        float typeChance = Random.value;

        if (typeChance <= rareSpawnChance)
        {//rare spawn
            slime.data.levelMapping.level = Random.Range(maxLevelHere, maxLevelHere += rareLevelIncrease);

            for (int i = 0; i < rareTypesInArea.Count; i++)
                slime.data.archetype = rareTypesInArea[Random.Range(0, rareTypesInArea.Count)];

            for (int i = 0; i < rareGensInArea.Count; i++)
                slime.data.statMapping.genMap.gen = rareGensInArea[Random.Range(0, rareGensInArea.Count)];
        }
        else
        {//normal Spawn
            slime.data.levelMapping.level = Random.Range(minLevelHere, maxLevelHere++);

            for (int i = 0; i < commonTypesInArea.Count; i++)
                slime.data.archetype = commonTypesInArea[Random.Range(0, commonTypesInArea.Count)];

            for (int i = 0; i < commonGensInArea.Count; i++)
                slime.data.statMapping.genMap.gen = commonGensInArea[Random.Range(0, commonGensInArea.Count)];
        }
        slime.data.levelMapping.LevelToExperience();
        slime.data.statMapping.GenerateStats(slime.data.levelMapping.level, slime.data.levelMapping.levelFlux);
        slime.data.TrackedLevel = slime.data.levelMapping.level;

        for (int i = 0; i < abilitySlotCount; i++)
            slime.data.abilities.Add(AbilityManager.Instance.AbilityMapRequest(slime.data));

        SlimeAppearanceControls appearance = slime.GetComponent<SlimeAppearanceControls>();
        appearance.SetAppearance(slime.data);//appearance set at spawn

        SpawnSlime(slime.agent);
        newSlime.transform.parent = transform;
        newSlime.SetActive(true);
        slime.agent.enabled = true;
    }
}
