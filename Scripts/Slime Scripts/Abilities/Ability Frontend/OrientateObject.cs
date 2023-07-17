using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientateObject : MonoBehaviour
{
    public float rotSpeed;
    public bool enableRotation;
    public enum RotationAxis { X_Axis, Y_Axis, Z_Axis }
    public RotationAxis rotationAxis;
    public enum SimSpace { World, Local }
    public SimSpace simSpace;

    private Vector3 currentRot;

    public void Awake()
    {
        if (simSpace == SimSpace.World)
            currentRot = transform.rotation.eulerAngles;
        else
            currentRot = transform.localEulerAngles;
    }
    public void Update()
    {
        if(enableRotation)
        {
            if (simSpace == SimSpace.World)
                currentRot = transform.rotation.eulerAngles;
            else
                currentRot = transform.localEulerAngles;

            if (rotationAxis == RotationAxis.X_Axis)
                currentRot.x = currentRot.x + (rotSpeed * Time.deltaTime);
            else if (rotationAxis == RotationAxis.Y_Axis)
                currentRot.y = currentRot.y + (rotSpeed * Time.deltaTime);
            else if (rotationAxis == RotationAxis.Z_Axis)
                currentRot.z = currentRot.z + (rotSpeed * Time.deltaTime);

            if (simSpace == SimSpace.World)
                transform.eulerAngles = currentRot;
            else
                transform.localEulerAngles = currentRot;
        }
    }
}
