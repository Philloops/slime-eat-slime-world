using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public LayerMask desiredLayers;
    public Camera cam;
    private Collider[] allHits;
    private SortedList<float, GameObject> interactSortedByAngle = new SortedList<float, GameObject>();
    private PlayerInputMap input;

    private IInteractable objectToInteractWith;
    public IInteractable ObjectToInteractWith
    {
        get { return objectToInteractWith; }
        set
        {            
            if (objectToInteractWith != value)
            {
                if (objectToInteractWith != null)//before value updates object
                    objectToInteractWith.OnHoverExit();

                objectToInteractWith = value;

                if (objectToInteractWith != null)//update to new value
                    objectToInteractWith.OnHoverEnter();
            }        
        }
    }

    void Start()
    {
        input = GetComponent<PlayerInputMap>();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(cam.transform.position + cam.transform.forward * 1.5f, .75f);
    }
    void Update()
    {
        if (objectToInteractWith != null)
        {
            Vector3 interactablePos = ((MonoBehaviour)objectToInteractWith).transform.position;
            Debug.DrawRay(cam.transform.position, interactablePos - cam.transform.position, Color.green);

            if(input.Interact && !AbilityManager.Instance.slimeManager.InitializedCombat)
            {//Change later when more then just combat as interaction option
                objectToInteractWith.Interact();
                //stop robot input
            }
        }
    }
    void FixedUpdate()
    {
        QuerySphereCastHitData();
    }
    private void QuerySphereCastHitData()
    {
        Vector3 castOffset = cam.transform.position + cam.transform.forward * 1.5f;
        allHits = Physics.OverlapSphere(castOffset, .75f, desiredLayers);
        if(allHits.Length == 0)
        {
            ObjectToInteractWith = null;
            return;
        }
        else if(allHits.Length == 1)
        {
            ObjectToInteractWith = GetSuitableInteraction(allHits[0].gameObject);
            return;
        }
        else if(allHits.Length > 1)
        {
            CollectInteractables();
            ObjectToInteractWith = GetSuitableInteraction();
            interactSortedByAngle.Clear();
        }
    }   
    private IInteractable GetSuitableInteraction(GameObject _obj)
    {
        return _obj.GetComponent<IInteractable>();
    }
    private IInteractable GetSuitableInteraction()
    {
        IInteractable returnCandidate;
        foreach (KeyValuePair<float, GameObject> kvp in interactSortedByAngle)
        {
            returnCandidate = GetSuitableInteraction(kvp.Value);
            if (returnCandidate == null)
                continue;
            return returnCandidate;
        }
        return null;
    }
    private void CollectInteractables()
    {
        Vector3 dir = new Vector3();
        float angle;
        for (int i = 0; i < allHits.Length; i++)
        {
            if (allHits[i].GetComponent<IInteractable>() == null)
                continue;
            dir = allHits[i].transform.position - cam.transform.position;
            angle = Vector3.Angle(cam.transform.forward, dir);
            interactSortedByAngle.Add(angle, allHits[i].gameObject);
        }
    }
}
