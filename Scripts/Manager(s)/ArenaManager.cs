using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    #region Singleton
    public static ArenaManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            this.enabled = false;
    }
    #endregion

    public float arenaRadius;

    public GameObject controlledSlime;
    public GameObject automatedSlime;

    public List<SlimeData> controlledSlimes = new List<SlimeData>();
    public List<SlimeData> automatedSlimes = new List<SlimeData>();
    //private List<SlimeData> lineup = new List<SlimeData>();

    public Transform spawnA, spawnB;

    private AbilityManager ab;

    void Start()
    {
        ReadSlimeManager();
    }

    public Vector3 RelativeRandomPosition()
    {
        Vector3 pos = Random.insideUnitSphere * arenaRadius;
        //pos += transform.position;
        pos.y = transform.position.y;
        return pos;
    }
    private void ReadSlimeManager()
    {
        ab = AbilityManager.Instance;

        for (int i = 0; i < ab.slimeManager.automatedSlimes.Count; i++)
            automatedSlimes.Add(ab.slimeManager.automatedSlimes[i].data);
        for (int i = 0; i < ab.slimeManager.controlledSlimes.Count; i++)
            controlledSlimes.Add(ab.slimeManager.controlledSlimes[i]);

        StartingCombatants();
    }

    public void StartingCombatants()
    {
        //add delay in spawning
        //potential cam pan before combat
        SpawnControlledSlime(controlledSlimes[0]);
        SpawnAutomatedSlime(automatedSlimes[0]);

        ab.slimeManager.DisablePlayerMotor();
    }

    public void SpawnControlledSlime(SlimeData _info)
    {
        GameObject newSlime = Instantiate(controlledSlime, Vector3.zero, Quaternion.identity);
        Slime combatant = newSlime.GetComponent<Slime>();

        combatant.data.levelMapping.level = _info.levelMapping.level;
        combatant.data.archetype = _info.archetype;
        combatant.data.statMapping.genMap.gen = _info.statMapping.genMap.gen;
        combatant.data.levelMapping.LevelToExperience();
        combatant.data.statMapping.ReadThruStats(_info);
        combatant.data.TrackedLevel = _info.levelMapping.level;

        for (int i = 0; i < _info.abilities.Count; i++)
            combatant.data.abilities.Add(_info.abilities[i]);

        SlimeAppearanceControls appearance = combatant.GetComponent<SlimeAppearanceControls>();
        appearance.SetAppearance(combatant.data);//appearance set at spawn

        newSlime.transform.parent = spawnA;
        newSlime.transform.position = spawnA.position;
        newSlime.transform.rotation = spawnA.rotation;       

        combatant.OnSpawnToWorld();
        newSlime.SetActive(true);
    }
    public void SpawnAutomatedSlime(SlimeData _info)
    {
        GameObject newSlime = Instantiate(automatedSlime, Vector3.zero, Quaternion.identity);
        Slime combatant = newSlime.GetComponent<Slime>();

        combatant.data.levelMapping.level = _info.levelMapping.level;
        combatant.data.archetype = _info.archetype;
        combatant.data.statMapping.genMap.gen = _info.statMapping.genMap.gen;
        combatant.data.levelMapping.LevelToExperience();
        combatant.data.statMapping.ReadThruStats(_info);
        combatant.data.TrackedLevel = _info.levelMapping.level;

        for (int i = 0; i < _info.abilities.Count; i++)
            combatant.data.abilities.Add(_info.abilities[i]);

        SlimeAppearanceControls appearance = combatant.GetComponent<SlimeAppearanceControls>();
        appearance.SetAppearance(combatant.data);//appearance set at spawn

        newSlime.transform.parent = spawnB;
        newSlime.transform.position = spawnB.position;
        newSlime.transform.rotation = spawnB.rotation;        

        combatant.OnSpawnToWorld();
        newSlime.SetActive(true);
    }
}
