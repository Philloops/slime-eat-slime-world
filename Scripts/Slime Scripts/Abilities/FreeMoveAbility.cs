using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMoveAbility : MonoBehaviour
{
    [Header("Direct References")]
    public CustomCamera cam;
    public Transform controlledObj;
    public Transform visualObj;

    [Header("Start/End Points")]
    public Transform origin;
    public Transform endPoint;

    [Header("Controlled values")]
    public float stepDist = 2f;
    public float stepSpeed = 12f;
    public float visualSpd = 5f;

    [Header("Numerical Visualizations")]
    public int camHeightIndex;
    public int vectorPercent;

    private float reach;
    public float Reach
    {
        get { return reach; }
        set
        {
            reach = value;
            AdjustReach();
        }
    }

    private float minCamHeight = 1.65f;
    private float maxCamHeight = 4.67f;

    public void AdjustReach()
    {
        endPoint.position = origin.position + origin.forward * reach;
    }

    void LateUpdate()//update, fixedUpdate
    {//minCamHeight -> 1.650001 && maxCamHeight -> 4.665044
        float camValue = (cam.transform.localPosition.y - minCamHeight) / (maxCamHeight - minCamHeight);
        camValue *= 100;
        camHeightIndex = (int)camValue;
        camHeightIndex = Mathf.Clamp(camHeightIndex, 0, 100);


        float posVal = (controlledObj.localPosition.z - origin.localPosition.z) / (endPoint.localPosition.z - origin.localPosition.z);
        posVal *= 100;
        vectorPercent = (int)posVal;
        vectorPercent = Mathf.Clamp(vectorPercent, 0, 100);

        if (vectorPercent < camHeightIndex)//forwards
        {
            //controlledObj.transform.position = Vector3.Lerp(controlledObj.transform.position, 
            //    (controlledObj.transform.position + (controlledObj.transform.forward / 2)), Time.deltaTime * 18f);
            controlledObj.position = controlledObj.position + (controlledObj.forward * stepDist) * Time.deltaTime * stepSpeed;
        }
        else if (vectorPercent > camHeightIndex)//backwards
        {
            //controlledObj.transform.position = Vector3.Lerp(controlledObj.transform.position, 
            //    (controlledObj.transform.position  + (-controlledObj.transform.forward / 2)), Time.deltaTime * 18f);
            controlledObj.position = controlledObj.position + (-controlledObj.forward * stepDist) * Time.deltaTime * stepSpeed;
        }
        visualObj.position = Vector3.Lerp(visualObj.position, controlledObj.position, Time.deltaTime * visualSpd);
    }
}
