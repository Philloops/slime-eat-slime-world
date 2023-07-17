using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class WSlimeBehavior
{
    public float speed;
    public int intervalTicks;
    public float duration;
    public float Timer { get; set; }
}

public class WorldSlime : MonoBehaviour, IInteractable
{
    [Header("Control Behavior")]
    public GameObject interactVisual;
    public GameObject unconciousVFX;
    public NavMeshAgent agent;
    public Animator anim;
    public float intervalCheck;
    public float allyCallRange;
    public LayerMask desiredLayer;//interactable
    public enum States { Wander, Idle, Unconcious }
    public States currentState;

    [Header("SlimeData module")]
    public SlimeData data;

    [Header("Behavior modules")]
    public WSlimeBehavior wander;
    public WSlimeBehavior idle;

    private WorldSlimeSpawner spawner;
    public WorldSlimeSpawner Spawner
    {
        get
        {
            if (spawner == null)
                spawner = GetComponentInParent<WorldSlimeSpawner>();

            return spawner;
        }
    }
    private SlimeFaceControls faceControl;
    public SlimeFaceControls FaceControl
    {
        get
        {
            if (faceControl == null)
                faceControl = GetComponent<SlimeFaceControls>();

            return faceControl;
        }
    }

    private float interval;
    private Vector3 randomPoint;
    private Vector3 finalPos;
    private NavMeshHit hit;


    public void Update()
    {
        DetermineStates();
    }

    #region IInteractable Behavior
    public void ConvertToCaptureReady()
    {
        Debug.Log(this.name + " is in a capture ready state");
        //agent.enabled = false;
        unconciousVFX.SetActive(true);
        FaceControl.FacialBehavior = SlimeFaceControls.FacialBehaviors.Unconcious;
        currentState = States.Unconcious;
        agent.speed = 0;
        anim.CrossFade("Unconcious", .5f);
    }
    public void OnHoverEnter()
    {
        Debug.Log("OnHoverEnter thru -> " + transform.name);
        interactVisual.SetActive(true);
    }
    public void OnHoverExit()
    {
        Debug.Log("OnHoverExit thru -> " + transform.name);
        interactVisual.SetActive(false);
    }

    private Collider[] allies;
    public void Interact()
    {
        Debug.Log("Interacted with -> " + transform.name);
        AbilityManager.Instance.slimeManager.automatedSlimes.Add(this);
        allies = Physics.OverlapSphere(transform.position, allyCallRange, desiredLayer);
        if(allies.Length > 0)
        {
            for (int i = 0; i < allies.Length; i++)
            {
                if(allies[i].GetComponent<WorldSlime>() && allies[i].gameObject != this.gameObject)
                {
                    Debug.Log("Found an ally in -> " + allies[i].transform.name);
                    AbilityManager.Instance.slimeManager.automatedSlimes.Add(allies[i].GetComponent<WorldSlime>());
                }
            }
        }
        AbilityManager.Instance.slimeManager.InitializedCombat = true;
    }
    #endregion  

    #region States Behavior
    private void DetermineStates()
    {
        if (currentState == States.Unconcious)
            return;

        interval -= Time.deltaTime;
        if (interval <= 0)
        {
            idle.Timer = 0;
            wander.Timer = 0;

            currentState = (States)Random.Range(0, 2);
            interval = intervalCheck * TickMultiplier();
        }
        RunStates();
    }
    private int TickMultiplier()
    {
        int tickValue = 1;

        if (currentState == States.Wander)
            tickValue = wander.intervalTicks;
        else if (currentState == States.Idle)
            tickValue = idle.intervalTicks;

        Mathf.Clamp(tickValue, 1, Mathf.Infinity);
        return tickValue;
    }
    private void RunStates()
    {
        if (!agent.enabled)
            return;

        if (currentState == States.Wander)
            Wander();
        else if (currentState == States.Idle)
            Idle();
    }
    private void Wander()
    {
        wander.Timer -= Time.deltaTime;

        if (wander.Timer <= 0)
        {
            agent.speed = wander.speed;
            bool successful = false;
            
            int safetyNet = 0;
            while(!successful)
            {               
                safetyNet++;
                if (safetyNet > 25)
                    successful = true;               
                randomPoint = Spawner.RelativeRandomPosition() + Spawner.transform.position;
                //randomPoint = Spawner.RelativeRandomPosition();
                if (NavMesh.SamplePosition(randomPoint, out hit, 2, 1))
                {
                    successful = true;
                    finalPos = hit.position;
                    agent.SetDestination(finalPos);

                    //SpawnSphere(finalPos);
                }
            }           
            wander.Timer = wander.duration;
        }      
    }
    //private void SpawnSphere(Vector3 _pos)
    //{
    //    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //    sphere.transform.position = _pos;
    //}
    private void Idle()
    {
        idle.Timer -= Time.deltaTime;

        if(idle.Timer <= 0)
        {
            agent.speed = idle.speed;
            idle.Timer = idle.duration;
        }
    }
    #endregion
}
