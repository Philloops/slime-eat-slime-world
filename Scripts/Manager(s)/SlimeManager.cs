using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlimeManager : MonoBehaviour
{
    private SimulatedSlimes simulatedSlimes;
    public List<SlimeData> controlledSlimes
    {//temp solution for dev-log
        get
        {
            if (simulatedSlimes == null)
                simulatedSlimes = FindObjectOfType<SimulatedSlimes>();

            return simulatedSlimes.slimes;
        }
    }
    public List<WorldSlime> automatedSlimes = new List<WorldSlime>();
    public string ArenaSceneName;

    private bool initializedCombat;
    public bool InitializedCombat
    {
        get { return initializedCombat; }
        set
        {
            initializedCombat = value;

            if (initializedCombat)
            {
                transition.gameObject.SetActive(true);              
                Invoke("AddCombatScene", 1.1f);
            }
            else if (!initializedCombat)
            {
                transition.gameObject.SetActive(true);
                Invoke("SetInitializedCombat", 1.1f);
            }
        }
    }
    private void SetInitializedCombat()
    {
        Slime[] slimes = FindObjectsOfType<Slime>();
        if(slimes.Length > 0)
        {
            for (int i = 0; i < slimes.Length; i++)
            {
                slimes[i].ResetPooledAbilityObjs();
            }
        }
        DeallocateBehavior();
        SceneManager.UnloadSceneAsync(ArenaSceneName);
        AbilityManager.Instance.SetCanvas = false;
        for (int i = 0; i < AbilityManager.Instance.pathOfFire.AbilityObjects.Count; i++)
        {
            for (int j = 0; j < AbilityManager.Instance.pathOfFire.AbilityObjects[i].objectPool.Count; j++)
            {
                AbilityManager.Instance.pathOfFire.AbilityObjects[i].objectPool[j].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < AbilityManager.Instance.pathOfWater.AbilityObjects.Count; i++)
        {
            for (int j = 0; j < AbilityManager.Instance.pathOfWater.AbilityObjects[i].objectPool.Count; j++)
            {
                AbilityManager.Instance.pathOfWater.AbilityObjects[i].objectPool[j].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < AbilityManager.Instance.pathOfNature.AbilityObjects.Count; i++)
        {
            for (int j = 0; j < AbilityManager.Instance.pathOfNature.AbilityObjects[i].objectPool.Count; j++)
            {
                AbilityManager.Instance.pathOfNature.AbilityObjects[i].objectPool[j].gameObject.SetActive(false);
            }
        }
        AbilityManager.Instance.playerCombatCanvas.ClearDefeatedSlimeData();
        AbilityManager.Instance.playerCombatCanvas.ResetStatusUIObjs();
        AbilityManager.Instance.enemyCombatCanvas.ClearDefeatedSlimeData();
        AbilityManager.Instance.enemyCombatCanvas.ResetStatusUIObjs();
    }
    void AddCombatScene()
    {
        SceneManager.LoadScene(ArenaSceneName, LoadSceneMode.Additive);
        AbilityManager.Instance.SetCanvas = true;
    }

    public TestTransition transition;
    private PlayerRefContainer[] players;
    public void DisablePlayerMotor()
    {
        players = FindObjectsOfType<PlayerRefContainer>();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Movement.playerCam.enabled = false;
            players[i].Input.enabled = false;
        }
    }
    private void DeallocateBehavior()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Movement.playerCam.enabled = true;
            players[i].Input.enabled = true;
        }
    }
    public void RemoveAutomatedSlime()
    {
        if(automatedSlimes.Count > 0)
        {
            Debug.LogError(automatedSlimes[0].name + " is in capture ready state");
            automatedSlimes[0].ConvertToCaptureReady();
            automatedSlimes.Remove(automatedSlimes[0]);
        }
    }
}
