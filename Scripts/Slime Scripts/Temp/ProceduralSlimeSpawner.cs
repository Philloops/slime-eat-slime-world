using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSlimeSpawner : MonoBehaviour
{
    public List<Slime> playerSlimes;
    public List<Slime> aiSlimes;
    public List<Transform> spawnNodes;

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


    void Start()
    {
        for (int i = 0; i < playerSlimes.Count; i++)
            SlimeSetup(playerSlimes[i], spawnNodes[0]);

        for (int i = 0; i < aiSlimes.Count; i++)
            SlimeSetup(aiSlimes[i], spawnNodes[1]);
    }

    private void SlimeSetup(BaseSlime _slime, Transform _spawnNode)
    {
        //int spawnNode = Random.Range(0, spawnNodes.Count);

        _slime.transform.parent = _spawnNode;
        _slime.transform.position = _spawnNode.position;
        _slime.transform.rotation = _spawnNode.rotation;

        float typeChance = Random.value;

        if (typeChance <= rareSpawnChance)
        {//rare spawn
            _slime.data.levelMapping.level = Random.Range(maxLevelHere, maxLevelHere += rareLevelIncrease);

            for (int i = 0; i < rareTypesInArea.Count; i++)
                _slime.data.archetype = rareTypesInArea[Random.Range(0, rareTypesInArea.Count)];

            for (int i = 0; i < rareGensInArea.Count; i++)
                _slime.data.statMapping.genMap.gen = rareGensInArea[Random.Range(0, rareGensInArea.Count)];
        }
        else
        {//normal Spawn
            _slime.data.levelMapping.level = Random.Range(minLevelHere, maxLevelHere++);

            for (int i = 0; i < commonTypesInArea.Count; i++)
                _slime.data.archetype = commonTypesInArea[Random.Range(0, commonTypesInArea.Count)];

            for (int i = 0; i < commonGensInArea.Count; i++)
                _slime.data.statMapping.genMap.gen = commonGensInArea[Random.Range(0, commonGensInArea.Count)];
        }

        _slime.OnSpawnToWorld();
        _slime.gameObject.SetActive(true);
    }
}
