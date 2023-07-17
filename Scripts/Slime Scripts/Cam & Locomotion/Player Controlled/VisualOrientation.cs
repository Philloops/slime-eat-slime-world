using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualOrientation : MonoBehaviour
{
    public Transform targetVisual;
    public Transform slimeVisual;
    public float rotSpeed = 10f;

    private Camera cam;
    private BaseLocomotion locomotion;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        locomotion = GetComponent<BaseLocomotion>();
    }
    void Update()
    {
        SetRotation(targetVisual);
    }

    private void SetRotation(Transform visual)
    {
        Vector3 lookPos = visual.position - cam.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        visual.rotation = Quaternion.Slerp(visual.rotation, rotation, Time.deltaTime * rotSpeed);

        if(locomotion.Controller.isGrounded)
        {//fixed looking away on jump(above)
            if (locomotion.Controller.velocity.magnitude > 0)
            {
                Vector3 newPos = slimeVisual.position - (locomotion.transform.position - locomotion.Controller.velocity);
                newPos.y = 0;
                var slimeRot = Quaternion.LookRotation(newPos);
                slimeVisual.rotation = Quaternion.Slerp(slimeVisual.rotation, slimeRot, Time.deltaTime * (rotSpeed / 2f));
            }
            else
            {
                //slimeVisual.rotation = slimeVisual.parent.transform.rotation;
                Quaternion desiredRot = slimeVisual.parent.transform.rotation;
                slimeVisual.rotation = Quaternion.Slerp(slimeVisual.rotation, desiredRot, Time.deltaTime * (rotSpeed / 2.5f));
            }
        }      
    }
}
