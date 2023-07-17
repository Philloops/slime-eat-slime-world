using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    //Sensitivity & inverted options
    public enum MouseState { Standard, Inverted }
    public MouseState mouseState;
    public Transform focusPoint;
    public float sensitivity;
    [Range(1, 5)]
    public float damping;//1.5, 3, 5
    public float yAngleMin;
    public float yAngleMax;
    public Vector3 camOffset;

    public float CurrentX { get; set; }
    public float CurrentY { get; set; }

    private SlimeInputMap slimeInputMap;
    private Vector3 newCamPos;
    private Quaternion newCamRot;
    //private Vector3 cameraMask;
    //private const float distance = 7.5f;
    //private float pushValue;

    void Awake()
    {
        slimeInputMap = GetComponentInParent<SlimeInputMap>();
    }
    private void Update()
    {
        GetInput();
    }
    private void LateUpdate()
    {
        UpdateCameraPosition();
    }
    private void GetInput()
    {
        CurrentX += slimeInputMap.LookData.x * sensitivity;
        if(mouseState == MouseState.Inverted)
            CurrentY += slimeInputMap.LookData.y * sensitivity;
        else
            CurrentY -= slimeInputMap.LookData.y * sensitivity;
        CurrentY = Mathf.Clamp(CurrentY, yAngleMin, yAngleMax);
    }
    private void UpdateCameraPosition()
    {
        newCamRot = Quaternion.Euler(CurrentY, CurrentX, 0);

        newCamPos = focusPoint.position + newCamRot * camOffset;
        //cameraMask = focusPoint.position + newCamRot * camOffset;

        //CheckObstructions();
        transform.position = Vector3.Lerp(transform.position, newCamPos, damping * Time.deltaTime);
        transform.LookAt(focusPoint);
    }

    //Camera Collision before removed
    /*private void CheckObstructions()
    {
        pushValue = distance / damping;
        RaycastHit hit;
        if (Physics.Linecast(focusPoint.position, cameraMask, out hit))
        {
            newCamPos = new Vector3(hit.point.x + hit.normal.x * distance, transform.position.y, hit.point.z + hit.normal.z * distance);
        }
        Debug.DrawLine(focusPoint.position, cameraMask);
    }*/
}
